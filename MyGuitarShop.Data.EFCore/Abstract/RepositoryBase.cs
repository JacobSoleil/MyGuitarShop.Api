using Microsoft.EntityFrameworkCore;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.EFCore.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.EFCore.Abstract
{
    public abstract class RepositoryBase<TEntity>(
        MyGuitarShopContext dbContext)
        : IRepository<TEntity, int>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null) return false;

            _dbSet.Remove(entity);

            return await dbContext.SaveChangesAsync() == 1;
        }

        public async Task<TEntity?> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<bool> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return await dbContext.SaveChangesAsync() == 1;
        }

        public async Task<bool> UpdateAsync(int id, TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity == null) return false;

            _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);

            return await dbContext.SaveChangesAsync() == 1;
        }
    }
}
