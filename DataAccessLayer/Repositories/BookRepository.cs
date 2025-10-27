using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : BaseRepository<BookEntity>, IBookRepository
    {
        private readonly DbSet<BookEntity> _dbSet;

        public BookRepository(LibraryDbContext context) : base(context)
        {
        }

        public Task<List<BookEntity>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            var books = _dbSet.Where(b => b.AuthorId == authorId).ToList();
            return Task.FromResult(books);
        }
                
        public Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            var exists = _dbSet.Any(b => b.AuthorId == authorId);
            return Task.FromResult(exists);
        }
    }
}

