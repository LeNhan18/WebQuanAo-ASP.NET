//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Productt.Models;
//using Productt.Models.Enums;
//using Productt.Repositories;
//using System.Security.Claims;

//namespace Productt.Controllers
//{
//    [Authorize]
//    public class OrderController : Controller
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly ICartRepository _cartRepository;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public OrderController(
//            IOrderRepository orderRepository,
//            ICartRepository cartRepository,
//            UserManager<ApplicationUser> userManager)
//        {
//            _orderRepository = orderRepository;
//            _cartRepository = cartRepository;
//            _userManager = userManager;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
//            return View(orders);
//        }

//        public async Task<IActionResult> Details(int id)
//        {
//            var order = await _orderRepository.GetOrderByIdAsync(id);
//            if (order == null)
//                return NotFound();

//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (order.UserId != userId)
//                return Forbid();

//            return View(order);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(string shippingAddress)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var user = await _userManager.FindByIdAsync(userId);
//            var cart = await _cartRepository.GetByUserIdAsync(userId);

//            if (cart == null || !cart.CartItems.Any())
//                return RedirectToAction("Index", "Cart");

//            var order = new Order
//            {
//                UserId = userId,
//                FirstName = user.FullName.Split(' ')[0],
//                LastName = string.Join(" ", user.FullName.Split(' ').Skip(1)),
//                Email = user.Email,
//                PhoneNumber = user.PhoneNumber,
//                Address = user.Address,
//                ShippingAddress = shippingAddress,
//                City = "Default City", // You might want to get this from user profile or form
//                Status = OrderStatus.Pending,
//                OrderDate = DateTime.Now,
//                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
//            };

//            var orderDetails = cart.CartItems.Select(ci => new OrderDetail
//            {
//                ProductId = ci.ProductId,
//                Quantity = ci.Quantity,
//                UnitPrice = ci.Product.Price,
//                Size = ci.Size,
//                Color = ci.Color
//            }).ToList();

//            order.OrderDetails = orderDetails;

//            await _orderRepository.CreateOrderAsync(order);
//            await _cartRepository.ClearAsync(userId);

//            return RedirectToAction(nameof(Details), new { id = order.Id });
//        }

//        [HttpPost]
//        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
//        {
//            var success = await _orderRepository.UpdateOrderStatusAsync(id, status);
//            if (!success)
//                return NotFound();

//            return RedirectToAction(nameof(Details), new { id });
//        }
//    }
//}
