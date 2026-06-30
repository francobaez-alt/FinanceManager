using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>();

        public DbSet<Wallet> Wallets => Set<Wallet>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Transaction> Transactions => Set<Transaction>();

        public DbSet<ScheduledTransaction> ScheduledTransactions => Set<ScheduledTransaction>();

        public DbSet<Budget> Budgets => Set<Budget>();

        public DbSet<TransactionHistory> TransactionHistories => Set<TransactionHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
