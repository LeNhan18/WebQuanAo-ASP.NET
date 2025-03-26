//using Microsoft.EntityFrameworkCore;
//using Productt.Models;
//using System;
//using System.Threading.Tasks;

//namespace Productt.Repositories
//{
//    public class CartRepository : ICartRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public CartRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        //Get By User Id

//        public async Task<CartItem> GetByUserIdAsync(string userId)
//        {
//            return await _context.CartItems
//                .Include(c => c.Id)
//                .FirstOrDefaultAsync(c => c.UserId == userId);
//        }

//        public async Task<Cart> AddAsync(Cart cart)
//        {
//            _context.Carts.Add(cart);
//            await _context.SaveChangesAsync();
//            return cart;
//        }

//        public async Task UpdateAsync(Cart cart)
//        {
//            _context.Entry(cart).State = EntityState.Modified;
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var cart = await _context.Carts.FindAsync(id);
//            if (cart != null)
//            {
//                _context.Carts.Remove(cart);
//                await _context.SaveChangesAsync();
//            }
//        }
//        async Task<CartItem> ICartRepository.AddItemAsync(string userId, int productId, int quantity, string size, string color)
//        {
//            var cart = await GetByUserIdAsync(userId);
//            if (cart == null)
//            {
//                cart = new CartItem { UserId = userId };
//                _context.CartItems.Add(cart);
//                await _context.SaveChangesAsync();
//            }
//            var existingItem = await _context.CartItems
//                .FirstOrDefaultAsync(ci => ci.Id == cart.Id &&
//                ci.ProductId == productId &&
//                ci.Size == size &&
//                ci.Color == color);
//            if (existingItem != null)
//            {
//                existingItem.Quantity += quantity;
//                await _context.SaveChangesAsync();
//                return existingItem;
//            }
//            var cartItem = new CartItem
//            {
//                Id = cart.Id,
//                ProductId = productId,
//                Quantity = quantity,
//                Size = size,
//                Color = color,
//                DateAdded = DateTime.Now

//            };
//            _context.Add(cartItem);
//            await _context.SaveChangesAsync();
//            return cartItem;

//        }
//        public async Task UpdateItemQuantityAsync(int cartItemId, int quantity)
//        {
//            var cartItem = await _context.CartItems.FindAsync(cartItemId);
//            if (cartItem != null)
//            {
//                cartItem.Quantity = quantity;
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task RemoveItemAsync(int cartItemId)
//        {
//            var cartItem = await _context.CartItems.FindAsync(cartItemId);
//            if (cartItem != null)
//            {
//                _context.CartItems.Remove(cartItem);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public async Task ClearCartAsync(string userId)
//        {
//            var cart = await GetByUserIdAsync(userId);
//            if (cart != null)
//            {
//                _context.CartItems.RemoveRange(cart);
//                await _context.SaveChangesAsync();
//            }
//        }

//        public Task<CartItem> AddAsync(CartItem cart)
//        {
//            throw new NotImplementedException();
//        }

//        public Task UpdateAsync(CartItem cart)
//        {
//            throw new NotImplementedException();
//        }

//        Task<Cart> ICartRepository.GetByUserIdAsync(string userId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
