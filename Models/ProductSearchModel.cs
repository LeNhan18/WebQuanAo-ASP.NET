using System.Collections.Generic;

namespace Productt.Models
{
    public class ProductSearchModel
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public string? Gender { get; set; }
        public string? Brand { get; set; }
        public List<string> Categories { get; set; } = new();
        public List<string> Brands { get; set; } = new();
        public List<string> Genders { get; set; } = new();
    }
}
