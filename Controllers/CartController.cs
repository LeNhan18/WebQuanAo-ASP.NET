using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productt.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Productt.Repositories;
using Productt.Models.Enums;
using Microsoft.Extensions.Logging;

namespace Productt.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            IOrderRepository orderRepository,
            ApplicationDbContext context,
            ILogger<CartController> logger)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Lấy giỏ hàng với đầy đủ thông tin sản phẩm
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart 
                { 
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
            }

            return View(cart);
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string size, string color, int quantity = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại";
                    return RedirectToAction("Index", "Product");
                }

                var cartItem = await _cartRepository.AddItemAsync(userId, productId, quantity, size, color);
                if (cartItem != null)
                {
                    TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi thêm vào giỏ hàng";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartRepository.RemoveItemAsync(cartItemId);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity > 0)
            {
                await _cartRepository.UpdateItemQuantityAsync(cartItemId, quantity);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Thanh toán giỏ hàng
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var cart = await _cartRepository.GetCartAsync(userId);

                if (cart == null || !cart.CartItems.Any())
                {
                    return RedirectToAction("Index");
                }

                var order = new Order
                {
                    UserId = userId,
                    FirstName = user.FullName.Split(' ')[0],
                    LastName = string.Join(" ", user.FullName.Split(' ').Skip(1)),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "",
                    Address = user.Address ?? "",
                    ShippingAddress = user.Address ?? "",
                    City = "Default City",
                    Status = OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
                };

                var orderDetails = cart.CartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price,
                    Size = ci.Size,
                    Color = ci.Color
                }).ToList();

                order.OrderDetails = orderDetails;

                await _orderRepository.CreateOrderAsync(order);
                await _cartRepository.ClearCartAsync(userId);

                return RedirectToAction("Details", "Order", new { id = order.OrderId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Checkout - {ex.Message}");
                return StatusCode(500, new { message = "Có lỗi xảy ra khi thanh toán." });
            }
        }
    }
}
