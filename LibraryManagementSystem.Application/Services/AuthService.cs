using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Dtos;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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

        public AuthService
        (
            UserManager<ApplicationUser> userManager,
            ILogger<AuthService> logger,
            IMapper mapper,
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
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
    }
}
