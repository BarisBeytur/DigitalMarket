using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarket.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.InsertUser)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Tags)
                .HasMaxLength(500);

            builder.HasMany(c => c.ProductCategories)
            .WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
