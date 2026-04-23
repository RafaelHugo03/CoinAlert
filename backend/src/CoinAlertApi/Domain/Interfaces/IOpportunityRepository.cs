using CoinAlertApi.Domain.Entities;

namespace CoinAlertApi.Domain.Interfaces;

public interface IOpportunityRepository : IRepository<Opportunity>
{
    Task<List<Opportunity>> GetActiveAsync();
}
