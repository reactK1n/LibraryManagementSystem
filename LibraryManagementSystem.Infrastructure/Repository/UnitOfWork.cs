using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using LibraryManagementSystem.Domain.IRepository.Base;
using LibraryManagementSystem.Infrastructure.Data.Context;
using LibraryManagementSystem.Infrastructure.Repository.Base;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        private IGenericRepository<Book> _bookRepository;


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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
