using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("BUDGETS");

            // PK
            builder.HasKey(x => x.Id);
            // Amount
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
            // IsActive
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            // Navigation Properties

            // User
            builder.HasOne(x => x.User)
                .WithMany(x => x.Budgets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Category
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Budgets)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.CategoryId,
                x.StartDate,
                x.EndDate
            });
        }
    }
}
