using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : BaseRepository<BookEntity>, IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookEntity>> GetAllNoTrackingAsync(CancellationToken cancellationToken)
        {
            return await _context.Books.
                AsNoTracking()
                .Include(b => b.Author)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<BookEntity>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            return await _context.Books
                .AnyAsync(b => b.AuthorId == authorId, cancellationToken);
        }

        public async Task<BookEntity?> GetByIdWithAuthorAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<BookEntity?> GetByIdWithAuthorNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .Include(b => b.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
    }
}

