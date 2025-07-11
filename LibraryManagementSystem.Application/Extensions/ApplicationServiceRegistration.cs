using System.Reflection;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Services;
using LibraryManagementSystem.Application.Services.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationSevicesInjection(this IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Sevices DI
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
        }
    }
}