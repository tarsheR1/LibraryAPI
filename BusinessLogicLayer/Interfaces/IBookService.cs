using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBookService
    {
        Task<Book> GetByIdAsync(Guid id);
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetByAuthorIdAsync(Guid authorId);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
