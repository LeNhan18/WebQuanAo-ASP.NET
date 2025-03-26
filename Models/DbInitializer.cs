using Productt.Models;

namespace Productt.Models
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Đảm bảo database được tạo
            context.Database.EnsureCreated();

            // Kiểm tra đã có categories chưa
            if (context.Categories.Any())
            {
                return; // Đã có dữ liệu
            }

            // Thêm categories
            var categories = new Category[]
            {
                new Category { Name = "Áo Nam" },
                new Category { Name = "Quần Nam" },
                new Category { Name = "Áo Nữ" },
                new Category { Name = "Quần Nữ" },
                new Category { Name = "Phụ Kiện" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            // Thêm products
            var products = new Product[]
            {
                new Product {
                    Name = "Áo Thun Nam Basic",
                    Price = 199000,
                    Price2 = 299000,
                    Description = "Áo thun nam cổ tròn, chất liệu cotton 100%",
                    imageUrl = "aothunnam.jpg",
                    CategoryId = 1
                },
                new Product {
                    Name = "Áo Sơ Mi Nam Dài Tay",
                    Price = 399000,
                    Price2 = 499000,
                    Description = "Áo sơ mi nam dài tay, chất liệu lụa cao cấp",
                    imageUrl = "sominam.jpg",
                    CategoryId = 1
                },
                new Product {
                    Name = "Quần Jean Nam Slim Fit",
                    Price = 499000,
                    Price2 = 599000,
                    Description = "Quần jean nam ống đứng, màu xanh đậm",
                    imageUrl = "quanjeannam.jpg",
                    CategoryId = 2
                },
                new Product {
                    Name = "Áo Kiểu Nữ Công Sở",
                    Price = 299000,
                    Price2 = 399000,
                    Description = "Áo kiểu nữ tay lỡ, phong cách công sở",
                    imageUrl = "aokieunu.jpg",
                    CategoryId = 3
                },
                new Product {
                    Name = "Quần Tây Nữ Ống Rộng",
                    Price = 459000,
                    Price2 = 559000,
                    Description = "Quần tây nữ ống rộng, chất liệu thoáng mát",
                    imageUrl = "quantaynu.jpg",
                    CategoryId = 4
                },
                new Product {
                    Name = "Thắt Lưng Da Nam",
                    Price = 299000,
                    Price2 = 399000,
                    Description = "Thắt lưng da bò thật 100%",
                    imageUrl = "thatlung.jpg",
                    CategoryId = 5
                },
                new Product {
                    Name = "Áo Khoác Denim Nam",
                    Price = 699000,
                    Price2 = 799000,
                    Description = "Áo khoác jean nam phong cách trẻ trung",
                    imageUrl = "aokhoacnam.jpg",
                    CategoryId = 1
                },
                new Product {
                    Name = "Váy Liền Công Sở",
                    Price = 559000,
                    Price2 = 659000,
                    Description = "Váy liền thân công sở thanh lịch",
                    imageUrl = "vaynu.jpg",
                    CategoryId = 4
                }
            };

            foreach (var p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}