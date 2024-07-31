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
                .IsRequired(true);

            builder.Property(x => x.InsertUser)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Tags)
                .HasMaxLength(500);

        }
    }
}
