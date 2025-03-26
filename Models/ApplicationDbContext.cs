using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Productt.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(p => p.Price2)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(p => p.Description)
                    .IsRequired();

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.Description)
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName)
                    .HasMaxLength(50);

                entity.Property(u => u.Address)
                    .HasMaxLength(200);
            });
        }
    }
}
