using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Productt.Models;
using Productt.Models.Enums;
using Productt.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Productt.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public OrderController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            UserManager<ApplicationUser> userManager,
            PaymentService paymentService,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _paymentService = paymentService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (order.UserId != userId)
                return Forbid();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string shippingAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(shippingAddress))
                {
                    TempData["Error"] = "Vui lòng nhập địa chỉ giao hàng";
                    return RedirectToAction("Index", "Cart");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var cart = await _cartRepository.GetCartAsync(userId);

                if (cart == null || !cart.CartItems.Any())
                {
                    TempData["Error"] = "Giỏ hàng trống";
                    return RedirectToAction("Index", "Cart");
                }

                var order = new Order
                {
                    UserId = userId,
                    FirstName = user.FullName.Split(' ')[0],
                    LastName = string.Join(" ", user.FullName.Split(' ').Skip(1)),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber ?? "",
                    Address = user.Address ?? "",
                    ShippingAddress = shippingAddress,
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

                // Lưu đơn hàng
                await _orderRepository.CreateOrderAsync(order);
                
                // Xóa giỏ hàng
                await _cartRepository.ClearCartAsync(userId);

                TempData["Success"] = "Đặt hàng thành công!";
                return RedirectToAction("Details", new { id = order.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra khi đặt hàng";
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var success = await _orderRepository.UpdateOrderStatusAsync(id, status);
            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var url = _paymentService.CreatePaymentUrl(order, HttpContext);
            return Redirect(url);
        }

        public async Task<IActionResult> PaymentCallback()
        {
            var vnpayData = HttpContext.Request.Query;
            var pay = new VnPayLibrary();

            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    pay.AddResponseData(key, value.ToString());
                }
            }

            var orderId = Convert.ToInt32(pay.GetResponseData("vnp_TxnRef"));
            var vnPayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = pay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = pay.GetResponseData("vnp_SecureHash");
            var orderInfo = pay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = pay.ValidateSignature(vnpSecureHash, _configuration["Vnpay:HashSecret"]);

            if (checkSignature)
            {
                if (vnpResponseCode == "00")
                {
                    // Thanh toán thành công
                    var order = await _orderRepository.GetOrderByIdAsync(orderId);
                    if (order != null)
                    {
                        order.Status = OrderStatus.Processing;
                        await _orderRepository.UpdateOrderAsync(order);
                    }
                    TempData["Success"] = "Thanh toán thành công!";
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra trong quá trình thanh toán!";
                }
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra trong quá trình thanh toán!";
            }

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
