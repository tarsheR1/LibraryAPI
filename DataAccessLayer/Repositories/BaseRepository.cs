using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;

    protected BaseRepository(DbContext context)
    {
        _context = context;
    }

    public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<T?> GetByIdAsyncNoTracking(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().AnyAsync(e => e.Id == id);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<Guid> DeleteAsync(T entity, CancellationToken cancellationToken)
    {

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}