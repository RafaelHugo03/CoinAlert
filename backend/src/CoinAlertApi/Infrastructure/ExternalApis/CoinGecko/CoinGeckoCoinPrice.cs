using System.Text.Json.Serialization;

namespace CoinAlertApi.Infrastructure.ExternalApis.CoinGecko;

public class CoinGeckoCoinPrice
{
    [JsonPropertyName("usd")]
    public decimal Usd { get; set; }

    [JsonPropertyName("usd_24h_change")]
    public decimal Usd24hChange { get; set; }

}
