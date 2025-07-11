using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagementSystem.Application.Services
{

    namespace Application.Services
    {
        public class TokenService : ITokenService
        {
            private readonly IConfiguration _config;

            public TokenService(IConfiguration config)
            {
                _config = config;
            }

            public string GetToken(ApplicationUser user)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.Lastname} {user.Firstname}")
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]));

                var getToken = new JwtSecurityToken(
                    audience: _config["JwtSettings:ValidAudience"],
                    issuer: _config["JwtSettings:ValidIssuer"],
                    claims: authClaims,
                    expires: DateTime.UtcNow.AddDays(2),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(getToken);
            }
        }
    }
}