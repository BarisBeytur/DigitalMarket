using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarket.Data.Configuration
{
    public class DigitalWalletConfiguration : IEntityTypeConfiguration<DigitalWallet>
    {
        public void Configure(EntityTypeBuilder<DigitalWallet> builder)
        {
            builder.Property(x => x.PointBalance)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.InsertUser)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(x => x.UserId)
                .IsUnique();
        }
    }
}
