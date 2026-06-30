using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("CATEGORIES");

            // PK
            builder.HasKey(x => x.Id);
            // Name
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            // CategoryType
            builder.Property(x => x.Type)
                .HasConversion<byte>()
                .IsRequired();
            // Icon
            builder.Property(x => x.Icon)
                .HasMaxLength(100);
            // IsActive
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            // Navigation Properties

            // User
            builder.HasOne(x => x.User)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Transactions
            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // ScheduledTransactions
            builder.HasMany(x => x.ScheduledTransactions)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // Budgets
            builder.HasMany(x => x.Budgets)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.Name,
                x.Type
            }).IsUnique();

        }
    }
}
