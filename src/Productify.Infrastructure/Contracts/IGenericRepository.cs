namespace Productify.Infrastructure.Contracts;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetRangeAsync(int skip, int take);
    Task<TEntity?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}