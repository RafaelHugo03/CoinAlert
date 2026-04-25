using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;

namespace CoinAlertApi.Domain.Entities;

public class Opportunity : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string CryptoId { get; set; } = string.Empty;
    public OpportunityType Type { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public OpportunityStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? TriggeredAt { get; set; }
}