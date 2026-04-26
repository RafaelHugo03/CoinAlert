using CoinAlertApi.Application.DTOs;

namespace CoinAlertApi.Application.Interfaces;

public interface ICryptoPriceService
{
    Task<IReadOnlyList<PriceUpdateDto>?> GetAllAndTransmitAsync();
}
