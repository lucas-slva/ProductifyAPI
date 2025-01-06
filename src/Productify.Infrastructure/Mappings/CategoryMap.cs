using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Productify.Domain.Entities;

namespace Productify.Infrastructure.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}