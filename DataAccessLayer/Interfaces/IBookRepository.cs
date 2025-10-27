using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task<List<BookEntity>> GetAllNoTrackingAsync(CancellationToken cancellationToken);
        Task<List<BookEntity>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);
        Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);

        Task<BookEntity?> GetByIdWithAuthorAsync(Guid id, CancellationToken cancellationToken = default);
        Task<BookEntity?> GetByIdWithAuthorNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
