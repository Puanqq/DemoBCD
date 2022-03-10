using Demo.EntityFramework;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.UnitOfWork.Base
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey> where TPrimaryKey : struct
    {
        internal ApplicationDbContext _dbContext;
        internal DbSet<TEntity> _dbSet;

        IQueryable<TEntity> IRepository<TEntity, TPrimaryKey>.Query => _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task DeleteAsync(TPrimaryKey id)
        {
            return Task.Run(async () => {
                var find = await GetAsync(id);
                _dbSet.Remove(find);
                await SaveAsync();
            });
        }

        public async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));
        }

        public async Task<List<TEntity>> GetListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
            await SaveAsync();
            return entity;
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Task.Run(async () => { 
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                await SaveAsync();
            });
        }

        public Task SaveAsync()
        {
            return Task.Run(() => _dbContext.SaveChangesAsync());
        }
    }
}
