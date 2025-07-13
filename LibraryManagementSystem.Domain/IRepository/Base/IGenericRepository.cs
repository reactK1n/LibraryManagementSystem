using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Domain.IRepository.Base
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        Task InsertAsync(TEntity entity);
        Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> filter);
        Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity> GetByIdAsync(int id);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        DbSet<TEntity> GetContext();
    }
}
