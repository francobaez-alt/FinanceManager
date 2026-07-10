using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdWithRoleAsync(int id);
        Task<User?> GetByEmailWithPermissionsAsync(string email);
        Task<bool> ExistAsync(int id); 
        Task<bool> ExistEmailAsync(string email);
    }
}
