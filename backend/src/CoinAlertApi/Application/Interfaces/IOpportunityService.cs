using CoinAlertApi.Application.DTOs;

namespace CoinAlertApi.Application.Interfaces;

public interface IOpportunityService
{
    Task<List<OpportunityDto>> GetAllAsync();
    Task<OpportunityDto> CreateAsync(CreateOpportunityDto dto);
    Task<bool> DeleteAsync(string id);
}
