using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productt.Models;
using Productt.Repositories;
using System.Threading.Tasks;

namespace Productt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(categories);
        }
        public async Task<IActionResult> CategoryMenu()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return PartialView("_CategoryMenu", categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await _categoryRepository.ExistsAsync(id))
                {
                    return NotFound();
                }

                await _categoryRepository.UpdateAsync(category);
                TempData["Success"] = "Category updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _categoryRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _categoryRepository.DeleteAsync(id);
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Category/Products/5
        public async Task<IActionResult> Products(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}