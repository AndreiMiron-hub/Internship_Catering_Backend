namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task DeleteAsync(object id);
        Task<TEntity> DeleteAsync(TEntity entityToDelete);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate);
    }
}
