using CoinAlertApi.Domain.Enums;

namespace CoinAlertApi.Application.DTOs;

public class CreateOpportunityDto
{
    public string CryptoId { get; set; } = string.Empty;
    public OpportunityType Type { get; set; }
    public decimal TargetPrice { get; set; }
}
