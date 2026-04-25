using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;

namespace CoinAlertApi.Application.DTOs;

public class OpportunityDto
{
    public string Id { get; set; } = string.Empty;
    public string CryptoId { get; set; } = string.Empty;
    public OpportunityType Type { get; set; }
    public decimal TargetPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public OpportunityStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? TriggeredAt { get; set; }

    public static OpportunityDto FromEntity(Opportunity o) => new()
    {
        Id = o.Id,
        CryptoId = o.CryptoId,
        Type = o.Type,
        TargetPrice = o.TargetPrice,
        Status = o.Status,
        CreatedAt = o.CreatedAt,
        TriggeredAt = o.TriggeredAt
    };
}
