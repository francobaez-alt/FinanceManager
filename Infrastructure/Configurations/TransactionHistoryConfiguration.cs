using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable("TRANSACTION_HISTORY");

            // PK
            builder.HasKey(x => x.Id);
            // PreviousAmount
            builder.Property(x => x.PreviousAmount)
                .HasPrecision(18, 2);
            // NewAmount
            builder.Property(x => x.NewAmount)
                .HasPrecision(18, 2);
            // PreviousDescription
            builder.Property(x => x.PreviousDescription)
                .HasMaxLength(500);
            // NewDescription
            builder.Property(x => x.NewDescription)
                .HasMaxLength(500);
            // ModifiedAt
            builder.Property(x => x.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            // Navigation Properties

            // Transaction
            builder.HasOne(x => x.Transaction)
                .WithMany(x => x.TransactionHistories)
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
            // ModifiedByUser
            builder.HasOne(x => x.ModifiedByUser)
                .WithMany(x => x.ModifiedHistories)
                .HasForeignKey(x => x.ModifiedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.TransactionId);

            builder.HasIndex(x => x.ModifiedAt);
        }
    }
}
