using Productt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productt.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
    }
}
