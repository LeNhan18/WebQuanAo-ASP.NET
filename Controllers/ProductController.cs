using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Productt.Models;
using Productt.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Productt.Constants;

namespace Productt.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inject the repository and context via the constructor
        public ProductController(
            IProductRepository productRepository, 
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(ProductSearchModel searchModel)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.SearchTerm))
            {
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchModel.SearchTerm}%") ||
                                       EF.Functions.Like(p.Description ?? "", $"%{searchModel.SearchTerm}%"));
            }

            if (searchModel.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= searchModel.MinPrice.Value);
            }

            if (searchModel.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= searchModel.MaxPrice.Value);
            }

            if (searchModel.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == searchModel.CategoryId);
            }

            if (!string.IsNullOrEmpty(searchModel.Gender))
            {
                query = query.Where(p => p.Gender == searchModel.Gender);
            }

            if (!string.IsNullOrEmpty(searchModel.Brand))
            {
                query = query.Where(p => p.Brand == searchModel.Brand);
            }

            var products = await query.ToListAsync();

            // Populate filter options
            var categories = await _context.Categories
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            searchModel.Brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            searchModel.Genders = await _context.Products.Select(p => p.Gender).Distinct().ToListAsync();

            ViewBag.SearchModel = searchModel;
            return View(products);
        }


        //// Cho phép tất cả người dùng xem danh sách
        //[AllowAnonymous]
        //public async Task<IActionResult> Index(int? categoryId)
        //{
        //    var query = _context.Products
        //        .Include(p => p.Category)
        //        .AsQueryable();

        //    if (categoryId.HasValue)
        //    {
        //        query = query.Where(p => p.CategoryId == categoryId);
        //    }

        //    var products = await query.ToListAsync();
        //    return View(products);
        //}

        // Cho phép tất cả người dùng xem chi tiết
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Chỉ Admin mới có quyền thêm sản phẩm
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Price2,CategoryId,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (product.ImageFile != null)          
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }

                    product.imageUrl = "/images/products/" + uniqueFileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Chỉ Admin mới có quyền sửa
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Price2,CategoryId,ImageUrl,ImageFile")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý upload ảnh mới nếu có
                    if (product.ImageFile != null)
                    {
                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(product.imageUrl))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                                product.imageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload ảnh mới
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }

                        product.imageUrl = "/images/products/" + uniqueFileName;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Chỉ Admin mới có quyền xóa
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                // Xóa file ảnh
                if (!string.IsNullOrEmpty(product.imageUrl))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                        product.imageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                await _productRepository.DeleteAsync(product.Id);
            }

            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetSearchSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return Json(new List<object>());

            var suggestions = await _context.Products
                .Where(p => p.Name.ToLower().Contains(term.ToLower()) ||
                          (p.Description != null && p.Description.ToLower().Contains(term.ToLower())))
                .Take(5)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    ImageUrl = p.imageUrl
                })
                .ToListAsync();

            return Json(suggestions);
        }
        public IActionResult GetSearchSuggestionss(string term, int? minPrice, int? maxPrice)
        {
            var query = _context.Products.Where(p => p.Name.Contains(term));

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = query.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                imageUrl = p.imageUrl
            }).ToList();

            return Json(products);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
