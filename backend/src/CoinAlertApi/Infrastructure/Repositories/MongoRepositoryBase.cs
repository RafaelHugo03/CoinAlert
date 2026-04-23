using CoinAlertApi.Domain.Interfaces;
using MongoDB.Driver;

namespace CoinAlertApi.Infrastructure.Repositories;

public abstract class MongoRepositoryBase<T> : IRepository<T> where T : IEntity
{
    public readonly IMongoCollection<T> Collection;

    public MongoRepositoryBase(IMongoDatabase database, string collectionName)
    {
        Collection = database.GetCollection<T>(collectionName);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task InsertAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task<bool> UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        var result = await Collection.ReplaceOneAsync(filter, entity);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        var result = await Collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}
