using Demo.EntityFramework;
using Demo.EntityFramework.Entities;
using Demo.UnitOfWork.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.UnitOfWork.Base
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey> where TPrimaryKey : struct
    {
        internal ApplicationDbContext _dbContext;
        internal DbSet<TEntity> _dbSet;
        internal Guid? userId;

        IQueryable<TEntity> IRepository<TEntity, TPrimaryKey>.Query => _dbSet;

        public Repository(
            ApplicationDbContext context,
            IHttpContextAccessor _httpContextAccessor)
        {
            _dbContext = context;

            _dbSet = context.Set<TEntity>();

            var nameIdentifier = _httpContextAccessor.HttpContext?.User?.Claims?.ToList()?.Find(f => f.Type == ClaimTypes.NameIdentifier)?.Value;

            if(nameIdentifier != null)
            {
                userId = Guid.Parse(nameIdentifier);
            }
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
                _dbSet.Update(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                await SaveAsync();
            });
        }

        public Task UpdateListAsync(ICollection<TEntity> entities)
        {
            return Task.Run(async () => {
                _dbSet.UpdateRange(entities);

                foreach (var entity in entities)
                {
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }
                await SaveAsync();
            });
        }

        public async Task SaveAsync()
        {
            var entries = _dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entity<TPrimaryKey>)entityEntry.Entity).CreatedTime = DateTime.Now;
                    ((Entity<TPrimaryKey>)entityEntry.Entity).CreatedUser = userId;
                }
                else
                {
                    ((Entity<TPrimaryKey>)entityEntry.Entity).ModifiedTime = DateTime.Now;
                    ((Entity<TPrimaryKey>)entityEntry.Entity).ModifiedUser = userId;
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
