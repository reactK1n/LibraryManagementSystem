using LibraryManagementSystem.Domain.IRepository;
using LibraryManagementSystem.Infrastructure.Repository;

namespace LibraryManagementSystem.Api.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static void AddServiceRegistrations(this IServiceCollection services)
        {
            //repository DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
