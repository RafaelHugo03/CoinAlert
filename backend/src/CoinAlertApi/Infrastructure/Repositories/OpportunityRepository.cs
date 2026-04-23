using CoinAlertApi.Domain.Entities;
using CoinAlertApi.Domain.Enums;
using CoinAlertApi.Domain.Interfaces;
using CoinAlertApi.Infrastructure.Persistence;
using MongoDB.Driver;

namespace CoinAlertApi.Infrastructure.Repositories;

public class OpportunityRepository : MongoRepositoryBase<Opportunity>, IOpportunityRepository
{
    public OpportunityRepository(IMongoDatabase database)
        : base(database, CollectionNames.OpportunityCollectionName) { }

    public async Task<List<Opportunity>> GetActiveAsync()
    {
        var filter = Builders<Opportunity>.Filter.Eq(o => o.Status, OpportunityStatus.Active);
        return await Collection.Find(filter).ToListAsync();
    }
}
