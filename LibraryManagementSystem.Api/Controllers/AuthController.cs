using LibraryManagementSystem.Application.Interfaces;
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
    }
}
