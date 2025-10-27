using BusinessLogicLayer.Dto.Requests.Book;
using BusinessLogicLayer.Dto.Responses.Book;
    
namespace BusinessLogicLayer.Interfaces
{
    public interface IBookService
    {
        Task<BookResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<BookResponseDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<BookResponseDto>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken);
        Task<BookResponseDto> CreateAsync(CreateBookDto bookDto, CancellationToken cancellationToken);
        Task<BookResponseDto> UpdateAsync(Guid id, UpdateBookDto bookDto, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}