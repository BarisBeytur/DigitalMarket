using DigitalMarket.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarket.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(p => p.Email).IsUnique();

            builder.Property(p => p.Role)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.HasOne(p => p.DigitalWallet)
                .WithOne(p => p.User)
                .HasForeignKey<User>(p => p.DigitalWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Orders)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }

}
