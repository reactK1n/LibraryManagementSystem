using LibraryManagementSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Api.Extensions
{
    public static class ConnectionConfigurationExtension
    {

        public static void AddLibraryMSDbContext(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            string connectionString;

           if (env.IsDevelopment())
            {
                connectionString = config["ConnectionStrings:DefaultConnection"]; //for development enviroment
            }
            else
            {
                // Fallback or production/staging connection string
                connectionString = config["ConnectionStrings:ProductionConnection"]
                                   ?? throw new InvalidOperationException("Production connection string is not configured.");
            } 

            services.AddDbContextPool<LibraryDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
