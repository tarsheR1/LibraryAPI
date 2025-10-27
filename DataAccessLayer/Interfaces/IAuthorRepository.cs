using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IAuthorRepository : IRepository<AuthorEntity>
    {
        Task<AuthorEntity?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AuthorEntity?> GetByIdWithBooksNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<AuthorEntity>> GetAllWithBooksAsync(CancellationToken cancellationToken = default);
        Task<List<AuthorEntity>> GetAllWithBooksNoTrackingAsync(CancellationToken cancellationToken = default);
    }
}
