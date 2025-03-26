//using Microsoft.EntityFrameworkCore;
//using Productt.Models;
//using Productt.Models.Enums;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Productt.Repositories
//{
//    public class OrderRepository : IOrderRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public OrderRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Order> GetOrderByIdAsync(int orderId)
//        {
//            return await _context.Orders
//                .Include(o => o.OrderDetails)
//                .ThenInclude(od => od.Product)
//                .FirstOrDefaultAsync(o => o.OrderId == orderId);
//        }

//        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
//        {
//            return await _context.Orders
//                .Include(o => o.OrderDetails)
//                .ThenInclude(od => od.Product)
//                .Where(o => o.UserId == userId)
//                .OrderByDescending(o => o.OrderDate)
//                .ToListAsync();
//        }

//        public async Task<Order> CreateOrderAsync(Order order)
//        {
//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync();
//            return order;
//        }

//        public async Task<Order> UpdateOrderAsync(Order order)
//        {
//            _context.Entry(order).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//            return order;
//        }

//        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
//        {
//            var order = await _context.Orders.FindAsync(orderId);
//            if (order == null)
//                return false;

//            order.Status = status;
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> DeleteOrderAsync(int orderId)
//        {
//            var order = await _context.Orders.FindAsync(orderId);
//            if (order == null)
//                return false;

//            _context.Orders.Remove(order);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//    }
//}
