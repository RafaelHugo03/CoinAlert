using CoinAlertApi.Application.DTOs;

namespace CoinAlertApi.Application.Hubs;

public interface ICryptoPriceHubClient
{
    Task ReceivePriceUpdate(PriceUpdateDto priceUpdate);
    Task OpportunityTriggered(OpportunityTriggeredDto trigger);
}
