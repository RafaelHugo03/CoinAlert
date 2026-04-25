using CoinAlertApi.Application.DTOs;
using CoinAlertApi.Application.Hubs;
using CoinAlertApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CoinAlertApi.Controllers;

[ApiController]
[Route("api/mock")]
public class MockController(IHubContext<CryptoPriceHub, ICryptoPriceHubClient> hubContext) : ControllerBase
{

    [HttpPost("price/random")]
    public async Task<IActionResult> BroadcastRandomPrices()
    {
        var rng = Random.Shared;

        var prices = new[]
        {
            new PriceUpdateDto("bitcoin",  Math.Round((decimal)(rng.NextDouble() * 20000 + 85000), 2), Math.Round((decimal)(rng.NextDouble() * 10 - 5), 2)),
            new PriceUpdateDto("ethereum", Math.Round((decimal)(rng.NextDouble() * 500  +  1600), 2), Math.Round((decimal)(rng.NextDouble() * 10 - 5), 2)),
            new PriceUpdateDto("solana",   Math.Round((decimal)(rng.NextDouble() * 50   +   130), 2), Math.Round((decimal)(rng.NextDouble() * 10 - 5), 2)),
        };

        await Task.WhenAll(prices.Select(p => hubContext.Clients.All.ReceivePriceUpdate(p)));

        return Ok(new { broadcasted = true, prices });
    }

    [HttpPost("trigger/random")]
    public async Task<IActionResult> BroadcastRandomTrigger()
    {
        var rng    = Random.Shared;
        var coins  = new[] { "bitcoin", "ethereum", "solana" };
        var coin   = coins[rng.Next(coins.Length)];
        var type   = rng.Next(2) == 0 ? OpportunityType.Buy : OpportunityType.Sell;
        var target = Math.Round((decimal)(rng.NextDouble() * 10000 + 80000), 2);
        var current= Math.Round(target * (decimal)(1 + (rng.NextDouble() * 0.02 - 0.01)), 2);

        var dto = new OpportunityTriggeredDto(
            Id:           Guid.NewGuid().ToString(),
            CryptoId:     coin,
            Type:         type,
            TargetPrice:  target,
            CurrentPrice: current,
            TriggeredAt:  DateTime.UtcNow);

        await hubContext.Clients.All.OpportunityTriggered(dto);

        return Ok(new { broadcasted = true, dto });
    }
}
