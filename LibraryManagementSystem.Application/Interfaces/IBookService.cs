using LibraryManagementSystem.Application.Dtos;
using LibraryManagementSystem.Domain.Dtos;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task<ApiResponse> CreateAsync(BookDtos.CreateRequest dto);
        Task<ApiResponse> DeleteAsync(long id);
        Task<ApiResponse> GetAllAsync(BookDtos.BookFilterRequest request);
        Task<ApiResponse> GetByIdAsync(long id);
        Task<ApiResponse> UpdateAsync(long id, BookDtos.UpdateRequest dto);
    }
}
