//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Productt.Models
//{
//    public class OrderDetail
//    {
//        public OrderDetail()
//        {
//            Size = string.Empty;
//            Color = string.Empty;
//        }

//        [Key]
//        public int OrderDetailId { get; set; }

//        [Required]
//        public int OrderId { get; set; }

//        [Required]
//        public int ProductId { get; set; }

//        [Required]
//        [Range(1, 100)]
//        public int Quantity { get; set; }

//        [Required]
//        [Column(TypeName = "decimal(18,2)")]
//        public decimal UnitPrice { get; set; }

//        [Required]
//        [StringLength(10)]
//        public string Size { get; set; }

//        [Required]
//        [StringLength(20)]
//        public string Color { get; set; }

//        public virtual Order? Order { get; set; }
//        public virtual Product? Product { get; set; }
//    }
//}
