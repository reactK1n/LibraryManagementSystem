using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Api.Extensions
{
    public static class IdentityExtension
    {
        public static void AddIdentityConfig(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<LibraryDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
            });
        }
    }
}
