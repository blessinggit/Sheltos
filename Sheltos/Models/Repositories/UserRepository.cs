using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> GetAdminAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return users.FirstOrDefault();
        }

    }
}
