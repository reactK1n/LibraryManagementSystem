using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using LibraryManagementSystem.Domain.IRepository.Base;
using LibraryManagementSystem.Infrastructure.Data.Context;
using LibraryManagementSystem.Infrastructure.Repository.Base;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly LibraryDbContext _context;
        private IGenericRepository<Book> _bookRepository;
        private bool _disposed = false;

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Book> BookRepository
            => _bookRepository ??= new GenericRepository<Book>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Public Dispose method
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation to allow overriding
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
