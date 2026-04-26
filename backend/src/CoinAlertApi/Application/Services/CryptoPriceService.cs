using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Hubs;
using CoinAlertApi.Application.Interfaces;
using CoinAlertApi.Infrastructure.ExternalApis.CoinGecko;
using Microsoft.AspNetCore.SignalR;

namespace CoinAlertApi.Application.Services;

public class CryptoPriceService(
    CoinGeckoPriceClient coinGeckoClient,
    IHubContext<CryptoPriceHub, ICryptoPriceHubClient> hubContext) : ICryptoPriceService
{
    public async Task<IReadOnlyList<PriceUpdateDto>?> GetAllAndTransmitAsync()
    {
        var prices = await coinGeckoClient.GetPricesAsync();
        if (prices is null) return null;

        var updates = prices
            .Select(kvp => new PriceUpdateDto(kvp.Key, kvp.Value.Usd, kvp.Value.Usd24hChange))
            .ToList();

        await Task.WhenAll(updates.Select(u => hubContext.Clients.All.ReceivePriceUpdate(u)));

        return updates;
    }
}
