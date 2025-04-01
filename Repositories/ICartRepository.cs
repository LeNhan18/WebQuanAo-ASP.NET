using Productt.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productt.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string userId);
        Task<CartItem> AddItemAsync(string userId, int productId, int quantity, string size, string color);
        Task<bool> RemoveItemAsync(int cartItemId);
        Task<bool> UpdateItemQuantityAsync(int cartItemId, int quantity);
        Task<bool> ClearCartAsync(string userId);
    }
}
