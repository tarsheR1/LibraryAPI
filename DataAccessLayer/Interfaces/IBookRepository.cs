using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task<List<BookEntity>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);
        Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);
    }
}
