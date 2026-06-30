using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("TRANSACTIONS");

            // PK
            builder.HasKey(x => x.Id);
            // TransactionType
            builder.Property(x => x.Type)
                .HasConversion<byte>()
                .IsRequired();
            // Amount
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
            // Description
            builder.Property(x => x.Description)
                .HasMaxLength(500);
            // TransactionDate
            builder.Property(x => x.TransactionDate)
                .IsRequired();
            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            // Navigation Properties

            // Wallet
            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);
            // Category
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // TransactionHistories
            builder.HasMany(x => x.TransactionHistories)
                .WithOne(x => x.Transaction)
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.TransactionDate);

            builder.HasIndex(x => x.WalletId);

            builder.HasIndex(x => x.CategoryId);
        }
    }
}
