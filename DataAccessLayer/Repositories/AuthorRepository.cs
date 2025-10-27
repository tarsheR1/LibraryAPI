using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class AuthorRepository : BaseRepository<AuthorEntity>, IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<AuthorEntity?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<AuthorEntity?> GetByIdWithBooksNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<AuthorEntity>> GetAllWithBooksAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AuthorEntity>> GetAllWithBooksNoTrackingAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
