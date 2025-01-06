using Microsoft.EntityFrameworkCore;
using Productify.Domain.Entities;

namespace Productify.Infrastructure.Data;

public class ProductifyDbContext(DbContextOptions<ProductifyDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    
    // Fluent mapping applied
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductifyDbContext).Assembly);
        
        // ✅ Categories seeding
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" }
        );

        // ✅ Products seeding
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Smartphone", Price = 1500, CategoryId = 1 },
            new Product { Id = 2, Name = "Notebook", Price = 4500, CategoryId = 1 },
            new Product { Id = 3, Name = "T shirt", Price = 50, CategoryId = 2 }
        );
    }
}