using Microsoft.AspNetCore.Identity;

namespace Sheltos.Models.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetAdminAsync();
    }
}