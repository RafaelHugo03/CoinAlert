using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Hubs;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CoinAlertApi.Application.Workers;

public class PriceMonitorService(
    ICryptoPriceService cryptoPriceService,
    ICacheService cacheService,
    IHubContext<CryptoPriceHub, ICryptoPriceHubClient> hubContext,
    IServiceScopeFactory scopeFactory,
    ILogger<PriceMonitorService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Price monitor cycle failed");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken ct)
    {
        var updates = await cryptoPriceService.GetAllAndTransmitAsync();
        if (updates is null)
        {
            logger.LogWarning("CoinGecko returned null response");
            return;
        }

        var priceMap = updates.ToDictionary(u => u.CryptoId, u => u.Usd);
        await using var scope = scopeFactory.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOpportunityRepository>();

        var active = await repository.GetActiveAsync();
        logger.LogInformation("Price monitor: checking {Count} active opportunities", active.Count);

        await Parallel.ForEachAsync(active, ct, async (opportunity, token) =>
        {
            if (!priceMap.TryGetValue(opportunity.CryptoId, out var price))
                return;

            var isTriggered = opportunity.Type == OpportunityType.Buy
                ? price <= opportunity.TargetPrice
                : price >= opportunity.TargetPrice;

            if (!isTriggered)
                return;

            opportunity.Status = OpportunityStatus.Triggered;
            opportunity.CurrentPrice = price;
            opportunity.TriggeredAt = DateTime.UtcNow;

            await repository.UpdateAsync(opportunity.Id, opportunity);
            await cacheService.InvalidateAsync(CacheKeys.Opportunities);

            logger.LogInformation(
                "Opportunity triggered — Id: {Id} | {CryptoId} {Type} | target: {Target} | current: {Current} usd",
                opportunity.Id, opportunity.CryptoId, opportunity.Type,
                opportunity.TargetPrice, price);

            await TransmitOpportunity(opportunity, price);
        });

    }

    private async Task TransmitOpportunity(Opportunity opportunity, decimal price)
    {
            await hubContext.Clients.All.OpportunityTriggered(new OpportunityTriggeredDto(
            opportunity.Id,
            opportunity.CryptoId,
            opportunity.Type,
            opportunity.TargetPrice,
            price,
            opportunity.TriggeredAt!.Value));
    }
}
