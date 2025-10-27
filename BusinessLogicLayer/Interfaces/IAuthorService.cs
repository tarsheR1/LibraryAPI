using BusinessLogicLayer.Dto.Requests.Author;
using BusinessLogicLayer.Dto.Responses.Author;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<AuthorResponseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<AuthorResponseDto> CreateAsync(CreateAuthorDto authorDto, CancellationToken cancellationToken);
        Task<AuthorResponseDto> UpdateAsync(Guid id, UpdateAuthorDto authorDto, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}