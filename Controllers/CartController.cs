//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Productt.Models;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Productt.Controllers
//{
//    [Authorize]
//    public class CartController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public CartController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var cartItems = await _context.Carts
//                .Include(c => c.CartItems)
//                .Where(c => c.UserId == userId)
//                .ToListAsync();

//            return View(cartItems);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddToCart(int productId, int quantity, string size, string color)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var existingItem = await _context.Carts
//                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId 
//                    && c.Size == size && c.Color == color);

//            if (existingItem != null)
//            {
//                existingItem.Quantity += quantity;
//            }
//            else
//            {
//                var cartItem = new Cart
//                {
//                    UserId = userId,
//                    ProductId = productId,
//                    Quantity = quantity,
//                    Size = size,
//                    Color = color
//                };
//                _context.Carts.Add(cartItem);
//            }

//            await _context.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPost]
//        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var cartItem = await _context.Carts
//                .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);

//            if (cartItem == null)
//                return NotFound();

//            if (quantity <= 0)
//            {
//                _context.Carts.Remove(cartItem);
//            }
//            else
//            {
//                cartItem.Quantity = quantity;
//            }

//            await _context.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPost]
//        public async Task<IActionResult> RemoveFromCart(int cartId)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var cartItem = await _context.Carts
//                .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);

//            if (cartItem == null)
//                return NotFound();

//            _context.Carts.Remove(cartItem);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }
//    }
//}
