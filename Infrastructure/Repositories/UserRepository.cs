using Application.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FinanceDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdWithRoleAsync(int id)
        {
            return await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<User?> GetByEmailWithPermissionsAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ActiveAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.IsActive, true))
                > 0;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.IsActive, false))
                > 0;
        }

        public async Task<bool> UpdatePasswordAsync(int id, string newPassword)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(u => u.PasswordHash, newPassword))
                > 0;
        }

        public async Task<string> GetPasswordByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => u.PasswordHash)
                .FirstOrDefaultAsync();
        }
    }
}
