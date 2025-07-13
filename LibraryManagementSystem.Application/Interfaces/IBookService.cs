using LibraryManagementSystem.Application.Dtos;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task<ApiResponse> CreateAsync(BookDtos.CreateRequest dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse> GetAllAsync(BookDtos.BookFilterRequest request);
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> UpdateAsync(int id, BookDtos.UpdateRequest dto);
    }
}
