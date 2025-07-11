using System.Linq.Expressions;
using LibraryManagementSystem.Domain.IRepository.Base;
using LibraryManagementSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repository.Base
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly LibraryDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> filter)
        {
            var result = await _dbSet.FirstOrDefaultAsync(filter);
            return result;
        }

        public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await _dbSet.ToListAsync();
            }
            var result = await _dbSet.Where(filter).ToListAsync();
            return result;
        }

        public DbSet<TEntity> GetContext()
        {
            return _dbSet;
        }


        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }   

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}