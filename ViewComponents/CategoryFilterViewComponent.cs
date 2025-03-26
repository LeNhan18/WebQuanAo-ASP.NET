using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productt.Models;

namespace Productt.ViewComponents
{
    public class CategoryFilterViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryFilterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentCategoryId)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CurrentCategoryId = currentCategoryId;
            return View(categories);
        }
    }
} 