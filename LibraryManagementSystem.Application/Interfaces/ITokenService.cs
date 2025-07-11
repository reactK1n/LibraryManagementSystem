using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface ITokenService
    {
        string GetToken(ApplicationUser user);
    }
}
