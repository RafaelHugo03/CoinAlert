using CoinAlertApi.Domain.Enums;

namespace CoinAlertApi.Application.DTOs;

public record OpportunityTriggeredDto(
    string Id,
    string CryptoId,
    OpportunityType Type,
    decimal TargetPrice,
    decimal CurrentPrice,
    DateTime TriggeredAt);
