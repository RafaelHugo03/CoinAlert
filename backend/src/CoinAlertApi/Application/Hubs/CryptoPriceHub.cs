using Microsoft.AspNetCore.SignalR;

namespace CoinAlertApi.Application.Hubs;

public class CryptoPriceHub : Hub<ICryptoPriceHubClient>
{

}
