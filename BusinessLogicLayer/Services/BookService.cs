using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new Exception($"Book with ID {id} not found");

            return _mapper.Map<Book>(book);
        }

        public async Task<List<Book>> GetAllAsync()
        {
            var bookEntities = await _bookRepository.GetAllAsync();

            return _mapper.Map<List<Book>>(bookEntities);
        }

        public async Task<List<Book>> GetByAuthorIdAsync(Guid authorId)
        {
            var authorExists = await _authorRepository.ExistsAsync(authorId);
            if (!authorExists)
                throw new Exception($"Author with ID {authorId} not found");

            var bookEntities = await _bookRepository.GetByAuthorIdAsync(authorId);

            return _mapper.Map<List<Book>>(bookEntities);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new Exception("Book title cannot be empty");

            if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year + 1)
                throw new Exception("Invalid publication year");

            var authorExists = await _authorRepository.ExistsAsync(book.AuthorId);
            if (!authorExists)
                throw new Exception($"Author with ID {book.AuthorId} not found");

            if (book.Id == Guid.Empty)
                book.Id = Guid.NewGuid();

            await _bookRepository.CreateAsync(_mapper.Map<BookEntity>(book));
            return _mapper.Map<Book>(book);
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var existingBook = await _bookRepository.GetByIdAsync(book.Id);
            if (existingBook == null)
                throw new Exception($"Book with ID {book.Id} not found");

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new Exception("Book title cannot be empty");

            if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year + 1)
                throw new Exception("Invalid publication year");

            var authorExists = await _authorRepository.ExistsAsync(book.AuthorId);
            if (!authorExists)
                throw new Exception($"Author with ID {book.AuthorId} not found");

            return _mapper.Map<Book>(book);
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                throw new Exception($"Book with ID {id} not found");

            await _bookRepository.DeleteAsync(book);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            return book != null;
        }
    }
}
