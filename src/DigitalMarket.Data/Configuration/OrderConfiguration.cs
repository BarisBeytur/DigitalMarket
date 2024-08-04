using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarket.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.Property(p => p.UserId)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.InsertUser)
                .IsRequired().HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(p => p.CouponAmount)
                .IsRequired(false)
                .HasPrecision(18, 2)
                .HasDefaultValue(0m);

            builder.Property(p => p.PointAmount)
                .IsRequired(false)
                .HasPrecision(18, 2)
                .HasDefaultValue(0m);

            builder.Property(p => p.BasketAmount)
                .IsRequired(false)
                .HasPrecision(18, 2)
                .HasDefaultValue(0m);

            builder.Property(p => p.CouponCode)
                .IsRequired(false)
                .HasMaxLength(50);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.OrderDetails)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
