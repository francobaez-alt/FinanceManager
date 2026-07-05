using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly FinanceDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(FinanceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();
        public async Task<TEntity> GetByIdAsync(long id)
            => await _dbSet.FindAsync(id);
        public async Task Add(TEntity entity)
            => await _dbSet.AddAsync(entity);
        public void Update(TEntity entity)
            => _dbSet.Update(entity);
        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);
        public Task Save()
            => _context.SaveChangesAsync();
    }
}
