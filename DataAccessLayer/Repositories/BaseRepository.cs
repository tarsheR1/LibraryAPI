using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly List<T> _dataSet;

    protected BaseRepository(List<T> dataSet)
    {
        _dataSet = dataSet;
    }
    public virtual Task<bool> ExistsAsync(Guid id)
    {
        return Task.FromResult(_dataSet.Any(e => e.Id == id));
    }

    public virtual Task<List<T>> GetAllAsync()
    {
        return Task.FromResult(_dataSet.ToList());
    }

    public virtual Task<T?> GetByIdAsync(Guid id) 
    {
        var entity = _dataSet.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(entity);
    }

    public virtual Task<T> CreateAsync(T entity)
    {
        _dataSet.Add(entity);
        return Task.FromResult(entity);
    }

    public virtual Task DeleteAsync(T entity)  
    {
        _dataSet.Remove(entity);
        
        return Task.CompletedTask;
    }
}