using Application.Exceptions;
using Application.Interfaces.Security;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed
{
    public class DatabaseSeeder
    {
        private readonly FinanceDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public DatabaseSeeder(FinanceDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedPermissions();
            await _context.SaveChangesAsync();

            await SeedRoles();
            await _context.SaveChangesAsync();

            await SeedUsers();
            await _context.SaveChangesAsync();
        }

        // =========================
        // PERMISSIONS
        // =========================
        public async Task SeedPermissions()
        {
            if (await _context.Permissions.AnyAsync())
                return;

            var permissions = new List<Permission>
            {
                new() { Name = "Users.Create" },
                new() { Name = "Users.Read" },
                new() { Name = "Users.Update" },
                new() { Name = "Users.Delete" },

                new() { Name = "Roles.Create" },
                new() { Name = "Roles.Read" },
                new() { Name = "Roles.Update" },
                new() { Name = "Roles.Delete" },
                new() { Name = "Roles.AssignPermissions" },

                new() { Name = "Wallet.Read" },
                new() { Name = "Wallet.Deposit" },
                new() { Name = "Wallet.Withdraw" },
                new() { Name = "Wallet.Transfer" },

                new() { Name = "Transactions.Create" },
                new() { Name = "Transactions.Read" },
                new() { Name = "Transactions.Update" },
                new() { Name = "Transactions.Delete" },

                new() { Name = "ScheduledTransactions.Create" },
                new() { Name = "ScheduledTransactions.Read" },
                new() { Name = "ScheduledTransactions.Update" },
                new() { Name = "ScheduledTransactions.Delete" },

                new() { Name = "Categories.Create" },
                new() { Name = "Categories.Read" },
                new() { Name = "Categories.Update" },
                new() { Name = "Categories.Delete" }
            };

            await _context.Permissions.AddRangeAsync(permissions);
        }

        // =========================
        // ROLES + ROLEPERMISSIONS
        // =========================
        public async Task SeedRoles()
        {
            if (await _context.Roles.AnyAsync())
                return;

            var adminRole = new Role { Name = "Admin" };
            var userRole = new Role { Name = "User" };

            await _context.Roles.AddRangeAsync(adminRole, userRole);
            await _context.SaveChangesAsync(); // IMPORTANTE para obtener IDs

            var permissions = await _context.Permissions.ToListAsync();

            var rolePermissions = new List<RolePermissions>();

            // =========================
            // ADMIN → TODOS LOS PERMISOS
            // =========================
            rolePermissions.AddRange(
                permissions.Select(p => new RolePermissions
                {
                    RoleId = adminRole.Id,
                    PermissionId = p.Id
                })
            );

            // =========================
            // USER → PERMISOS LIMITADOS
            // =========================
            var userPermissionNames = new HashSet<string>
            {
                "Wallet.Read",
                "Wallet.Deposit",
                "Wallet.Transfer",

                "Transactions.Create",
                "Transactions.Read",
                "Transactions.Update",
                "Transactions.Delete",

                "Categories.Read",

                "ScheduledTransactions.Create",
                "ScheduledTransactions.Read",
                "ScheduledTransactions.Update"
            };

            var userPermissions = permissions
                .Where(p => userPermissionNames.Contains(p.Name));

            rolePermissions.AddRange(
                userPermissions.Select(p => new RolePermissions
                {
                    RoleId = userRole.Id,
                    PermissionId = p.Id
                })
            );

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
        }

        // =========================
        // USERS
        // =========================
        private async Task SeedUsers()
        {
            if (await _context.Users.AnyAsync())
                return;

            var adminRole = await _context.Roles
                .SingleOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
                throw new NotFoundException("Admin role not found. SeedRoles must run first.");

            var admin = new User
            {
                Name = "Admin",
                Email = "admin@system.com",
                PasswordHash = _passwordHasher.Hash("123"),
                RoleId = adminRole.Id,
                IsEmailConfirmed = false,
                IsActive = true
            };

            await _context.Users.AddAsync(admin);
        }
    }
}
