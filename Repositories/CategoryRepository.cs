using Microsoft.EntityFrameworkCore;
using Productt.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Productt.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                    .ThenInclude(p => p.Category)
                .Where(c => c.Products.Any())
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
