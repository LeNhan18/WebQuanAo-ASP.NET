//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Productt.Models
//{
//    public class CartItem
//    {
//        public CartItem()
//        {
//            UserId = string.Empty;
//            Size = string.Empty;
//            Color = string.Empty;
//        }

//        public int Id { get; set; }

//        [Required]
//        public string UserId { get; set; }

//        [Required]
//        public int ProductId { get; set; }

//        [Required]
//        [Range(1, 100)]
//        public int Quantity { get; set; }

//        [Required]
//        [StringLength(10)]
//        public string Size { get; set; }

//        [Required]
//        [StringLength(20)]
//        public string Color { get; set; }

//        [Required]
//        public DateTime DateAdded { get; set;} =DateTime.Now;
//        public virtual ApplicationUser? User { get; set; }
//        public virtual Product? Product { get; set; }
//    }
//}
