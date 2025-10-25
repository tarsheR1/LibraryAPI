using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                throw new Exception($"Author with ID {id} not found");

            return _mapper.Map<Author>(author);
        }

        public async Task<List<Author>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return _mapper.Map<List<Author>>(authors);
        }

        public async Task<Author> CreateAsync(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
                throw new Exception("Author name cannot be empty");

            if (author.DateOfBirth > DateTime.Now)
                throw new Exception("Birth date cannot be in the future");

            if (author.Id == Guid.Empty)
                author.Id = Guid.NewGuid();

            await _authorRepository.CreateAsync(_mapper.Map<AuthorEntity>(author));
            return author;
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(author.Id);
            if (existingAuthor == null)
                throw new Exception($"Author with ID {author.Id} not found");

            if (string.IsNullOrWhiteSpace(author.Name))
                throw new Exception("Author name cannot be empty");

            return author;
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                throw new Exception($"Author with ID {id} not found");

            var allBooks = await _bookRepository.GetAllAsync();
            var hasBooks = allBooks.Any(b => b.AuthorId == id);

            if (hasBooks)
                throw new Exception("Cannot delete author with existing books");

            await _authorRepository.DeleteAsync(author);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return author != null;
        }
    }
}
