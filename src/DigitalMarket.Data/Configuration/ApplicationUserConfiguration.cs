using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(150);

            builder.HasOne(x => x.DigitalWallet)
                .WithOne(x => x.User)
                .HasForeignKey<ApplicationUser>(x => x.DigitalWalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
