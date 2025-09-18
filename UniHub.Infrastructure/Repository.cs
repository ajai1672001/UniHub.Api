using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniHub.Domain.Entities;

namespace UniHub.Infrastructure;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Get all records
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Get records by predicate
    public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>> predicate = null,
        bool ignoreQueryFilter = false,
        bool hasNoTracking = false)
    {
        IQueryable<T> query = _dbSet;

        if (ignoreQueryFilter)
            query = query.IgnoreQueryFilters();

        if (hasNoTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    // Insert single record
    public async Task<T> InsertAsync(T entity)
    {
        var entry = await _dbSet.AddAsync(entity);
        return entry.Entity;
    }

    // Insert multiple records
    public async Task<IEnumerable<T>> BulkInsertAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities;
    }

    // Update single record
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // Update multiple records
    public void BulkUpdate(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    // Delete single record
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    // Delete multiple records
    public void BulkDelete(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}