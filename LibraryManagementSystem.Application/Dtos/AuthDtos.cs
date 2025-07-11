using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using static LibraryManagementSystem.Application.Utilities.Validators.AuthValidations;

namespace LibraryManagementSystem.Application.Dtos
{
    public class AuthDtos
    {
        public class RegisterDto
        {
            [ValidEmail]
            public required string Email { get; set; }

            [StrongPassword]
            public required string Password { get; set; }

            public required string Firstname { get; set; }

            public required string Lastname { get; set; }
        }

        public class LoginRequest
        {
            public required string Email { get; set; }

            public required string Password { get; set; }
        }

        public class ChangePasswordRequest
        {
            [StrongPassword]
            public required string NewPassword { get; set; }

            [JsonIgnore]
            public long UserId { get; set; }
        }

        public class LoginResponse
        {
            public string Id { get; set; }

            public string Email { get; set; }

            public string Token { get; set; }

            public string RefreshToken { get; set; }

        }

        public class ResetPasswordRequest
        {
            public string NewPassword { get; set; }

            public string EncodedEmail { get; set; }
        }
    }
}
