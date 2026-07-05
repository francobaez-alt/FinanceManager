using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS");

            // PK
            builder.HasKey(x => x.Id);
            // Name
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            // Email
            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();
            // PasswordHash
            builder.Property(x => x.PasswordHash)
                .IsRequired();
            // ProfileImage
            builder.Property(x => x.ProfileImagen)
                .HasMaxLength(300);
            // IsEmailConfirmed
            builder.Property(x => x.IsEmailConfirmed)
                .HasDefaultValue(false);
            // IsActive
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
            // CreatedAt
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Navigation Properties
            
            // Role
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            // Wallets
            builder.HasMany(x => x.Wallets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Categorias
            builder.HasMany(x => x.Categories)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Budget
            builder.HasMany(x => x.Budgets)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // TransactionHistory
            builder.HasMany(x => x.ModifiedHistories)
                .WithOne(x => x.ModifiedByUser)
                .HasForeignKey(x => x.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            

        }
    }
}
