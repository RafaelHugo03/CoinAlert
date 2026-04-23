namespace CoinAlertApi.Domain.Interfaces;

public interface IRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetAllAsync();
    Task InsertAsync(T entity);
    Task<bool> UpdateAsync(string id, T entity);
    Task<bool> DeleteAsync(string id);
}
