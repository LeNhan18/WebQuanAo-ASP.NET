using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productt.Constants;
using Productt.Models;

namespace Productt.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
} 