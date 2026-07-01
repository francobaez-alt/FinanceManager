using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ScheduledTransactionConfiguration : IEntityTypeConfiguration<ScheduledTransaction>
    {
        public void Configure(EntityTypeBuilder<ScheduledTransaction> builder)
        {
            builder.ToTable("SCHEDULED_TRANSACTIONS");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<byte>()
                .IsRequired();

            builder.Property(x => x.Frequency)
                .HasConversion<byte>()
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.ScheduledTransactions)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.ScheduledTransactions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.NextExecution);

            builder.HasIndex(x => x.IsActive);
        }
    }
}
