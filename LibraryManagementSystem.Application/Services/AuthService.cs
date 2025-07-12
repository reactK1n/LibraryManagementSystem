using System.Text;
using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Dtos;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using static LibraryManagementSystem.Application.Dtos.AuthDtos;

namespace LibraryManagementSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService
        (
            UserManager<ApplicationUser> userManager,
            ILogger<AuthService> logger,
            IMapper mapper,
            ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> Register(RegisterDto request)
        {
            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning($"User registration failed for {request.Email}. Errors: {errorMessages}");

                return new ApiResponse
                {
                    Message = "Registration failed. Please check your input and try again."
                };
            }

            return new ApiResponse
            {
                Status = true,
                Message = "User created successfully"
            };
        }

        public async Task<ApiResponse> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Password change failed: User with ID {UserId} not found", request.UserId);
                return new ApiResponse
                {
                    Message = "User not found.",
                };
            }

            // Manually hash the new password
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password change failed for user {Email}: {Errors}", user.Email, errorMessages);

                return new ApiResponse
                {
                    Message = "Failed to update the password.",
                };
            }

            return new ApiResponse
            {
                Status = true,
                Message = "Password updated successfully."
            };
        }

        public async Task<ApiResponse> Login(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            // Validate credentials
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User with email {Email} not found", request.Email);
                return new ApiResponse
                {
                    Message = "Invalid email or password.",
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogWarning("Login failed: Incorrect password for user {Email}", request.Email);
                return new ApiResponse
                {
                    Status = false,
                    Message = "Invalid email or password.",
                    Data = null
                };
            }

            // Generate refresh token and save it
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);


            // Generate token
            var token = _tokenService.GetToken(user);

            // Prepare response DTO
            var loginResponse = _mapper.Map<LoginResponse>(user);
            loginResponse.Token = token;

            _logger.LogInformation("Login successful for {Email}", user.Email);

            return new ApiResponse
            {
                Status = true,
                Message = "Login successful",
                Data = loginResponse
            };
        }

        public async Task<ApiResponse> ResetPassword(ResetPasswordRequest request)
        {

            _logger.LogInformation("Reset password attempt");

            var email = DecodeToken(request.EncodedEmail);
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);

            // Manually hash the new password
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password change failed for user {Email}: {Errors}", user.Email, errorMessages);

                return new ApiResponse
                {
                    Message = "Failed to update the password."
                };
            }

            return new ApiResponse
            {
                Status = true,
                Message = "Password updated successfully."
            };
        }

        public async Task Logout()
        {
            var headers = _httpContextAccessor.HttpContext.Request.Headers;
            headers.Remove("Authorization");
            _logger.LogInformation("User logged out successfully");
            // If you need to perform any other signout logic (e.g., invalidating tokens), add it here
            await Task.CompletedTask;
        }

        private string EncodeToken(string token)
        {
            var encodedToken = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(encodedToken);
        }

        private string DecodeToken(string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            return Encoding.UTF8.GetString(decodedToken);
        }
    }
}
