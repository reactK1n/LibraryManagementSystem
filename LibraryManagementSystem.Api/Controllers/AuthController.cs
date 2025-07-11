using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagementSystem.Application.Dtos.AuthDtos;

namespace LibraryManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.Register(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Logs a user in and returns a token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _authService.ChangePassword(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Resets a user's password using an encoded email token.
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPassword(request);
            return StatusCode(result.Status ? 200 : 400, result);
        }

        /// <summary>
        /// Logs the user out by removing the authorization header.
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok(new ApiResponse
            {
                Status = true,
                Message = "User logged out successfully",
                Data = null
            });
        }
    }
}
