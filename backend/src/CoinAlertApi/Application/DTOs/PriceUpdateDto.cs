namespace CoinAlertApi.Application.DTOs;

public record PriceUpdateDto(string CryptoId, decimal Usd, decimal Usd24hChange);
