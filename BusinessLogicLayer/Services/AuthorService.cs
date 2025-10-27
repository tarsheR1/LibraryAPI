using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using BusinessLogicLayer.Dto.Requests.Author;
using BusinessLogicLayer.Dto.Responses.Author;

namespace BusinessLogicLayer.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var authorEntity = await _authorRepository.GetByIdWithBooksAsync(id, cancellationToken);
            if (authorEntity == null)
                throw new Exception($"Author with ID {id} not found");

            return _mapper.Map<AuthorResponseDto>(authorEntity);
        }

        public async Task<List<AuthorResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllWithBooksNoTrackingAsync(cancellationToken);
            return _mapper.Map<List<AuthorResponseDto>>(authors);
        }

        public async Task<AuthorResponseDto> CreateAsync(CreateAuthorDto authorDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(authorDto.Name))
                throw new Exception("Author name cannot be empty");

            if (authorDto.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
                throw new Exception("Birth date cannot be in the future");

            var authorEntity = _mapper.Map<AuthorEntity>(authorDto);
            var createdEntity = await _authorRepository.CreateAsync(authorEntity, cancellationToken);

            return _mapper.Map<AuthorResponseDto>(createdEntity);
        }

        public async Task<AuthorResponseDto> UpdateAsync(Guid id, UpdateAuthorDto authorDto, CancellationToken cancellationToken)
        {
            var existingEntity = await _authorRepository.GetByIdWithBooksAsync(id, cancellationToken);
            if (existingEntity == null)
                throw new Exception($"Author with ID {id} not found");

            if (string.IsNullOrWhiteSpace(authorDto.Name))
                throw new Exception("Author name cannot be empty");

            _mapper.Map(authorDto, existingEntity);
            await _authorRepository.UpdateAsync(existingEntity, cancellationToken);

            return _mapper.Map<AuthorResponseDto>(existingEntity);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdWithBooksAsync(id, cancellationToken);
            if (author == null)
                throw new Exception($"Author with ID {id} not found");

            if (author.Books.Any())
                throw new Exception("Cannot delete author with existing books");

            await _authorRepository.DeleteAsync(author, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _authorRepository.ExistsAsync(id, cancellationToken);
        }
    }
}