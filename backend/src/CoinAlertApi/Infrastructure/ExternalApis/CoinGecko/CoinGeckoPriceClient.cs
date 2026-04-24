using System.Net.Http.Json;

namespace CoinAlertApi.Infrastructure.ExternalApis.CoinGecko;

public class CoinGeckoPriceClient(IHttpClientFactory factory)
{
    private const string ClientName = "coingecko";
    private const string PricePath =
        "/api/v3/simple/price?ids=bitcoin,ethereum,solana&vs_currencies=usd,brl&include_24hr_change=true";

    public async Task<Dictionary<string, CoinGeckoCoinPrice>?> GetPricesAsync()
    {
        using var client = factory.CreateClient(ClientName);
        return await client.GetFromJsonAsync<Dictionary<string, CoinGeckoCoinPrice>>(PricePath);
    }
}
