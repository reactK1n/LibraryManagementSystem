using LibraryManagementSystem.Application.Dtos;
using LibraryManagementSystem.Domain.Dtos;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> Login(AuthDtos.LoginRequest request);
        Task<ApiResponse> Register(AuthDtos.RegisterDto request);
    }
}
