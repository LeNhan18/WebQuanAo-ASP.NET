//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Productt.Models.Enums;

//namespace Productt.Models
//{
//    public class Order
//    {
//        public Order()
//        {
//            UserId = string.Empty;
//            FirstName = string.Empty;
//            LastName = string.Empty;
//            Address = string.Empty;
//            ShippingAddress = string.Empty;
//            City = string.Empty;
//            PhoneNumber = string.Empty;
//            Email = string.Empty;
//            OrderDetails = new List<OrderDetail>();
//            Status = OrderStatus.Pending;
//        }

//        [Key]
//        public int OrderId { get; set; }

//        [Required]
//        public string UserId { get; set; }
//        public virtual ApplicationUser? User { get; set; }

//        [Required]
//        [StringLength(50)]
//        public string FirstName { get; set; }

//        [Required]
//        [StringLength(50)]
//        public string LastName { get; set; }

//        [Required]
//        [StringLength(200)]
//        public string Address { get; set; }

//        [Required]
//        [StringLength(200)]
//        public string ShippingAddress { get; set; }

//        [Required]
//        [StringLength(50)]
//        public string City { get; set; }

//        [Required]
//        [StringLength(20)]
//        [Phone]
//        public string PhoneNumber { get; set; }

//        [Required]
//        [EmailAddress]
//        [StringLength(100)]
//        public string Email { get; set; }

//        public DateTime OrderDate { get; set; } = DateTime.Now;

//        [Required]
//        [Column(TypeName = "decimal(18,2)")]
//        public decimal TotalAmount { get; set; }

//        [Required]
//        public OrderStatus Status { get; set; }

//        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
//    }
//}
