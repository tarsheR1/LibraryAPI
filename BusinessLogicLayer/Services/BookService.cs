using BusinessLogicLayer.Dto.Requests.Book;
using BusinessLogicLayer.Dto.Responses.Book;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<BookResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var bookEntity = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (bookEntity == null)
                throw new Exception($"Book with ID {id} not found");

            var author = await _authorRepository.GetByIdAsync(bookEntity.AuthorId, cancellationToken);
            var bookAge = DateTime.Now.Year - bookEntity.PublishedYear;

            return new BookResponseDto(
                bookEntity.Id,
                bookEntity.Title,
                bookEntity.PublishedYear,
                bookEntity.AuthorId,
                author?.Name ?? string.Empty,
                bookAge
            );
        }

        public async Task<List<BookResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var bookEntitiesTask = _bookRepository.GetAllAsync(cancellationToken);
            var allAuthorsTask = _authorRepository.GetAllAsync(cancellationToken);

            await Task.WhenAll(bookEntitiesTask, allAuthorsTask);

            var bookEntities = await bookEntitiesTask;
            var allAuthors = await allAuthorsTask;

            var authorsDict = allAuthors.ToDictionary(a => a.Id, a => a.Name);

            var currentYear = DateTime.Now.Year;
            return bookEntities.Select(bookEntity => new BookResponseDto(
                bookEntity.Id,
                bookEntity.Title,
                bookEntity.PublishedYear,
                bookEntity.AuthorId,
                authorsDict.GetValueOrDefault(bookEntity.AuthorId, string.Empty),
                currentYear - bookEntity.PublishedYear
            )).ToList();
        }

        public async Task<List<BookResponseDto>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken)
        {
            var authorExists = await _authorRepository.ExistsAsync(authorId, cancellationToken);
            if (!authorExists)
                throw new Exception($"Author with ID {authorId} not found");

            var bookEntities = await _bookRepository.GetByAuthorIdAsync(authorId, cancellationToken);
            var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken);
            var currentYear = DateTime.Now.Year;

            return bookEntities.Select(bookEntity => new BookResponseDto(
                bookEntity.Id,
                bookEntity.Title,
                bookEntity.PublishedYear,
                bookEntity.AuthorId,
                author?.Name ?? string.Empty,
                currentYear - bookEntity.PublishedYear
            )).ToList();
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

            var bookEntity = new BookEntity(
                Guid.NewGuid(),
                bookDto.Title,
                bookDto.PublishedYear,
                bookDto.AuthorId
            );

            var createdEntity = await _bookRepository.CreateAsync(bookEntity, cancellationToken);
            var author = await _authorRepository.GetByIdAsync(createdEntity.AuthorId, cancellationToken);
            var bookAge = DateTime.Now.Year - createdEntity.PublishedYear;

            return new BookResponseDto(
                createdEntity.Id,
                createdEntity.Title,
                createdEntity.PublishedYear,
                createdEntity.AuthorId,
                author?.Name ?? string.Empty,
                bookAge
            );
        }

        public async Task<BookResponseDto> UpdateAsync(Guid id, UpdateBookDto bookDto, CancellationToken cancellationToken)
        {
            var existingEntity = await _bookRepository.GetByIdAsync(id, cancellationToken);
            if (existingEntity == null)
                throw new Exception($"Book with ID {id} not found");

            if (string.IsNullOrWhiteSpace(bookDto.Title))
                throw new Exception("Book title cannot be empty");

            if (bookDto.PublishedYear < 1000 || bookDto.PublishedYear > DateTime.Now.Year + 1)
                throw new Exception("Invalid publication year");

            var authorExists = await _authorRepository.ExistsAsync(bookDto.AuthorId, cancellationToken);
            if (!authorExists)
                throw new Exception($"Author with ID {bookDto.AuthorId} not found");

            var updatedEntity = new BookEntity(
                existingEntity.Id,
                bookDto.Title,
                bookDto.PublishedYear,
                bookDto.AuthorId
            )
            {
                Author = existingEntity.Author
            };

            await _bookRepository.UpdateAsync(updatedEntity, cancellationToken);

            var author = await _authorRepository.GetByIdAsync(updatedEntity.AuthorId, cancellationToken);
            var bookAge = DateTime.Now.Year - updatedEntity.PublishedYear;

            return new BookResponseDto(
                updatedEntity.Id,
                updatedEntity.Title,
                updatedEntity.PublishedYear,
                updatedEntity.AuthorId,
                author?.Name ?? string.Empty,
                bookAge
            );
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