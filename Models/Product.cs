using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Productt.Models
{
    public class Product
    {
        public Product()
        {
            Name = string.Empty;
            Description = string.Empty;
            imageUrl = string.Empty;
            Brand = string.Empty;
            Gender = string.Empty;
            AvailableSizes = new List<string>();
            AvailableColors = new List<string>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán")]
        [Range(0, 100000000000, ErrorMessage = "Giá bán phải lớn hơn 0")]
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá gốc")]
        [Range(0, 100000000000, ErrorMessage = "Giá gốc phải lớn hơn 0")]
        [Display(Name = "Giá gốc")]
        public decimal Price2 { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? imageUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        public bool IsAvailable { get; set; } = true;

        [NotMapped]
        public List<string> AvailableSizes { get; set; }

        [NotMapped]
        public List<string> AvailableColors { get; set; }

        [Range(0, 1000)]
        public int StockQuantity { get; set; }
    }
}