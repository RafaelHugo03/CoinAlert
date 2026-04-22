using CoinAlertApi.Domain.Interfaces;

namespace CoinAlertApi.Domain.Entities;

public class Crypto : IEntity
{
    public string Id { get; set; } = string.Empty;
    public CryptoPrice Price { get; set; } = new();
}