using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Productt.Models
{
    public class Cart
    {
        public Cart()
        {
            UserId = string.Empty;
            CartItems = new List<CartItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }

        [NotMapped]
        public decimal TotalAmount => CartItems.Sum(item => item.Product.Price * item.Quantity);

        [NotMapped]
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }
}
