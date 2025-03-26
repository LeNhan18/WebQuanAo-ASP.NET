using Microsoft.AspNetCore.Mvc;
using Productt.Repositories;

namespace Productt.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryMenuViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }
    }
}
