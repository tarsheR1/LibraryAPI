using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using BusinessLogicLayer.Dto.Requests.Book;
using BusinessLogicLayer.Dto.Responses.Book;

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

        public async Task<BookResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var bookEntity = await _bookRepository.GetByIdWithAuthorAsync(id, cancellationToken);
            if (bookEntity == null)
                throw new Exception($"Book with ID {id} not found");

            return _mapper.Map<BookResponseDto>(bookEntity);
        }

        public async Task<List<BookResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var bookEntities = await _bookRepository.GetAllNoTrackingAsync(cancellationToken);
            return _mapper.Map<List<BookResponseDto>>(bookEntities);
        }

        public async Task<List<BookResponseDto>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdWithBooksAsync(authorId, cancellationToken);
            if (author == null)
                throw new Exception($"Author with ID {authorId} not found");

            return _mapper.Map<List<BookResponseDto>>(author.Books);
        }

        public async Task<BookResponseDto> CreateAsync(CreateBookDto bookDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(bookDto.Title))
                throw new Exception("Book title cannot be empty");

            if (bookDto.PublishedYear < 1000 || bookDto.PublishedYear > DateTime.Now.Year + 1)
                throw new Exception("Invalid publication year");

            var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId, cancellationToken);
            if (!authorExists)
                throw new Exception($"Author with ID {bookDto.AuthorId} not found");

            var bookEntity = _mapper.Map<BookEntity>(bookDto);
            var createdEntity = await _bookRepository.CreateAsync(bookEntity, cancellationToken);

            return _mapper.Map<BookResponseDto>(createdEntity);
        }

        public async Task<BookResponseDto> UpdateAsync(Guid id, UpdateBookDto bookDto, CancellationToken cancellationToken)
        {
            var existingBook = await _bookRepository.GetByIdWithAuthorAsync(id, cancellationToken);
            if (existingBook == null)
                throw new Exception($"Book with ID {id} not found");

            if (string.IsNullOrWhiteSpace(bookDto.Title))
                throw new Exception("Book title cannot be empty");

            if (bookDto.PublishedYear < 1000 || bookDto.PublishedYear > DateTime.Now.Year + 1)
                throw new Exception("Invalid publication year");

            var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId, cancellationToken);
            if (!authorExists)
                throw new Exception($"Author with ID {bookDto.AuthorId} not found");

            _mapper.Map(bookDto, existingBook);
            await _bookRepository.UpdateAsync(existingBook, cancellationToken);

            return _mapper.Map<BookResponseDto>(existingBook);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (book == null)
                throw new Exception($"Book with ID {id} not found");

            await _bookRepository.DeleteAsync(book, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _bookRepository.ExistsAsync(id, cancellationToken);
        }
    }
}