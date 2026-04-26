using System.Net;
using System.Net.Http.Json;

namespace CoinAlertApi.Infrastructure.ExternalApis.CoinGecko;

public class CoinGeckoPriceClient(IHttpClientFactory factory, ILogger<CoinGeckoPriceClient> logger)
{
    private const string ClientName = "coingecko";
    private const string PricePath =
        "/api/v3/simple/price?ids=bitcoin,ethereum,solana&vs_currencies=usd,brl&include_24hr_change=true";

    private static readonly Dictionary<string, (decimal Min, decimal Max)> MockRanges = new()
    {
        ["bitcoin"]  = (10_000m, 100_000m),
        ["ethereum"] = (1_000m,   3_000m),
        ["solana"]   = (1m,         100m),
    };

    public async Task<Dictionary<string, CoinGeckoCoinPrice>?> GetPricesAsync()
    {
        using var client = factory.CreateClient(ClientName);
        using var response = await client.GetAsync(PricePath);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            logger.LogWarning("CoinGecko rate limit hit (429) — returning mocked prices");
            return GenerateMockPrices();
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Dictionary<string, CoinGeckoCoinPrice>>();
    }

    private static Dictionary<string, CoinGeckoCoinPrice> GenerateMockPrices()
    {
        var rng = Random.Shared;
        return MockRanges.ToDictionary(
            kvp => kvp.Key,
            kvp => new CoinGeckoCoinPrice
            {
                Usd = Math.Round(kvp.Value.Min + (decimal)rng.NextDouble() * (kvp.Value.Max - kvp.Value.Min), 2),
                Usd24hChange = Math.Round((decimal)(rng.NextDouble() * 10 - 5), 2),
            });
    }
}
