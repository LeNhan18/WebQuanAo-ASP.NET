using Microsoft.EntityFrameworkCore;
using Productt.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Productt.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(ApplicationDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<CartItem> AddItemAsync(string userId, int productId, int quantity, string size, string color)
        {
            try
            {
                var cart = await GetCartAsync(userId);
                
                if (cart == null)
                {
                    cart = new Cart 
                    { 
                        UserId = userId,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity,
                    Size = size,
                    Color = color,
                    DateAdded = DateTime.Now
                };

                cart.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();

                return cartItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddItemAsync");
                return null;
            }
        }

        public async Task<bool> RemoveItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null) 
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null) 
                return false;

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            
            if (cart == null || !cart.CartItems.Any()) 
                return false;

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartItem> AddAsync(CartItem cart)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CartItem cart)
        {
            throw new NotImplementedException();
        }
    }
}
