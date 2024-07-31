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
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(e => e.OrderId)
                .IsRequired();

            builder.Property(e => e.ProductId)
                .IsRequired();

            builder.Property(e => e.Quantity)
                .IsRequired();

            builder.Property(e => e.Price)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(e => e.PointAmount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0);
        }
    }
}
