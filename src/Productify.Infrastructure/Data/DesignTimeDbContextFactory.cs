using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Productify.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductifyDbContext>
{
    public ProductifyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductifyDbContext>();
        
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "productify.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new ProductifyDbContext(optionsBuilder.Options);
    }
}