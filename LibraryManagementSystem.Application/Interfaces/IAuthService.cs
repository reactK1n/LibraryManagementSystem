using LibraryManagementSystem.Application.Dtos;
using LibraryManagementSystem.Domain.Dtos;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> ChangePassword(AuthDtos.ChangePasswordRequest request);
        Task<ApiResponse> Login(AuthDtos.LoginRequest request);
        Task Logout();
        Task<ApiResponse> Register(AuthDtos.RegisterDto request);
        Task<ApiResponse> ResetPassword(AuthDtos.ResetPasswordRequest request);
    }
}
