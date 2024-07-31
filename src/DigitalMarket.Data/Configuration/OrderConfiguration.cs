using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.Property(p => p.UserId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.InsertUser).IsRequired().HasMaxLength(50);

            builder.Property(p => p.CouponAmount).IsRequired(false).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(p => p.PointAmount).IsRequired(false).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(p => p.BasketAmount).IsRequired(false).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(p => p.CouponCode).IsRequired(false).HasMaxLength(50);

            builder.HasIndex(x => x.UserId).IsUnique();
            builder.HasIndex(x => x.OrderDetailId).IsUnique();
        }
    }
}
