using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository.Base;

namespace LibraryManagementSystem.Domain.IRepository
{
    public interface IUnitOfWork
    {
        IGenericRepository<Book> BookRepository { get; }

        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
