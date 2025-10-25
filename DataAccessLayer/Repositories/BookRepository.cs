using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : BaseRepository<BookEntity>, IBookRepository
    {
        public BookRepository(DataContext context) : base(context.Books)
        {
        }

        public Task<List<BookEntity>> GetByAuthorIdAsync(Guid authorId)
        {
            var books = _dataSet.Where(b => b.AuthorId == authorId).ToList();
            return Task.FromResult(books);
        }

        public Task<bool> ExistsByAuthorIdAsync(Guid authorId)
        {
            var exists = _dataSet.Any(b => b.AuthorId == authorId);
            return Task.FromResult(exists);
        }
    }
}

