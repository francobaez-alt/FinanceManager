using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("WALLETS");

            // PK
            builder.HasKey(x => x.Id);

            // CurrencyType
            builder.Property(x => x.CurrencyType)
                .HasConversion<byte>()
                .IsRequired();
            // Name
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            // Description
            builder.Property(x => x.Description)
                .HasMaxLength(300);
            // Balance
            builder.Property(x => x.Balance)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);
            // IsActive
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            
            // Navigation Properties

            // User
            builder.HasOne(x => x.User)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Transactions
            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.Wallet)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);
            // ScheduledTransactions
            builder.HasMany(x => x.ScheduledTransactions)
                .WithOne(x => x.Wallet)
                .HasForeignKey(x =>x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.Name,
                x.CurrencyType
            }).IsUnique();

        }
    }
}
