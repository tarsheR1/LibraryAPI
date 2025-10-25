using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetByIdAsync(Guid id);
        Task<List<Author>> GetAllAsync();
        Task<Author> CreateAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
