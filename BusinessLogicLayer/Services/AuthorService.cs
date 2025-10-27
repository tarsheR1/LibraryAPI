using AutoMapper;
using BusinessLogicLayer.Dto.Requests.Author;
using BusinessLogicLayer.Dto.Responses.Author;
using BusinessLogicLayer.Interfaces;
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

        public async Task<AuthorResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var authorEntity = await _authorRepository.GetByIdAsync(id, cancellationToken);
            if (authorEntity == null)
                throw new Exception($"Author with ID {id} not found");

            var books = await _bookRepository.GetByAuthorIdAsync(id, cancellationToken);
            var age = DateTime.Now.Year - authorEntity.DateOfBirth.Year;

            return new AuthorResponseDto(
                authorEntity.Id,
                authorEntity.Name,
                authorEntity.DateOfBirth,
                age,
                books.Count
            );
        }

        public async Task<List<AuthorResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var authorEntities = await _authorRepository.GetAllAsync(cancellationToken);

            var allBooks = await _bookRepository.GetAllAsync(cancellationToken);

            var booksByAuthor = allBooks
                .GroupBy(b => b.AuthorId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var authorDtos = new List<AuthorResponseDto>();
            var today = DateOnly.FromDateTime(DateTime.Now);

            foreach (var authorEntity in authorEntities)
            {
                booksByAuthor.TryGetValue(authorEntity.Id, out var authorBooks);
                var booksCount = authorBooks?.Count ?? 0;

                var age = today.Year - authorEntity.DateOfBirth.Year;

                authorDtos.Add(new AuthorResponseDto(
                    authorEntity.Id,
                    authorEntity.Name,
                    authorEntity.DateOfBirth,
                    age,
                    booksCount
                ));
            }

            return authorDtos;
        }

        public async Task<AuthorResponseDto> CreateAsync(CreateAuthorDto authorDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(authorDto.Name))
                throw new Exception("Author name cannot be empty");

            if (authorDto.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
                throw new Exception("Birth date cannot be in the future");

            var authorEntity = new AuthorEntity(
                Guid.NewGuid(),
                authorDto.Name,
                authorDto.DateOfBirth
            );

            var createdEntity = await _authorRepository.CreateAsync(authorEntity, cancellationToken);

            return new AuthorResponseDto(
                createdEntity.Id,
                createdEntity.Name,
                createdEntity.DateOfBirth,
                DateTime.Now.Year - createdEntity.DateOfBirth.Year,
                0 
            );
        }

        public async Task<AuthorResponseDto> UpdateAsync(Guid id, UpdateAuthorDto authorDto, CancellationToken cancellationToken)
        {
            var existingEntity = await _authorRepository.GetByIdAsync(id, cancellationToken);
            if (existingEntity == null)
                throw new Exception($"Author with ID {id} not found");

            if (string.IsNullOrWhiteSpace(authorDto.Name))
                throw new Exception("Author name cannot be empty");

            var updatedEntity = new AuthorEntity(
                existingEntity.Id,
                authorDto.Name,
                authorDto.DateOfBirth
            )
            {
                Books = existingEntity.Books
            };

            await _authorRepository.UpdateAsync(updatedEntity, cancellationToken);

            var books = await _bookRepository.GetByAuthorIdAsync(id, cancellationToken);
            var age = DateTime.Now.Year - updatedEntity.DateOfBirth.Year;

            return new AuthorResponseDto(
                updatedEntity.Id,
                updatedEntity.Name,
                updatedEntity.DateOfBirth,
                age,
                books.Count
            );
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(id, cancellationToken);
            if (author == null)
                throw new Exception($"Author with ID {id} not found");

            var hasBooks = await _bookRepository.ExistsByAuthorIdAsync(id, cancellationToken);
            if (hasBooks)
                throw new Exception("Cannot delete author with existing books");

            await _authorRepository.DeleteAsync(author, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _authorRepository.ExistsAsync(id, cancellationToken);
        }
    }
}