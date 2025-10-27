using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbSet.Any(e => e.Id == id);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<Guid> DeleteAsync(T entity, CancellationToken cancellationToken)
    {

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}