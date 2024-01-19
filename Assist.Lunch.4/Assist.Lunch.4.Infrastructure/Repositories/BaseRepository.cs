using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;

            dbSet = context.Set<TEntity>();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await dbSet.FindAsync(id);

            await DeleteAsync(entity);
        }

        public async Task<TEntity> DeleteAsync(TEntity entityToDelete)
        {
            dbSet.Remove(entityToDelete);

            await context.SaveChangesAsync();

            return entityToDelete;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entityList = await dbSet.ToListAsync();

            return entityList;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(id);

            return entity;
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);

            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entityToUpdate)
        {
            await context.SaveChangesAsync();

            return entityToUpdate;
        }
    }
}
