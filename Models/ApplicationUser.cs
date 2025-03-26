using Microsoft.AspNetCore.Identity;

namespace Productt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Address { get; set; }
        public string? Avatar { get; set; }
    }
}
