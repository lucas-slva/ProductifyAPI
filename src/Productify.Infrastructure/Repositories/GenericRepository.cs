using Microsoft.EntityFrameworkCore;
using Productify.Infrastructure.Contracts;
using Productify.Infrastructure.Data;

namespace Productify.Infrastructure.Repositories;

public class GenericRepository<TEntity>(ProductifyDbContext context) : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ProductifyDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    
    public async Task<IEnumerable<TEntity>> GetRangeAsync(int skip, int take) =>
        await _dbSet
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<TEntity?> GetByIdAsync(int id) => 
        await _dbSet.FindAsync(id);

    public async Task<bool> ExistsAsync(int id) =>
        await _dbSet.FindAsync(id) != null;

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(int skip, int take, params string[] includes)
    {
        var query = includes.Aggregate<string?, IQueryable<TEntity>>(_dbSet, (current, include) =>
            current.Include(include ?? throw new ArgumentNullException(nameof(include))));

        return await query.Skip(skip).Take(take).ToListAsync();
    }
}