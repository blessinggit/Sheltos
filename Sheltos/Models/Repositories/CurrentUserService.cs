
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public string UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
                if (string.IsNullOrEmpty(id))
                {
                    id = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                return id;
            }
        }

        public async Task<ApplicationUser> GetUser()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == UserId);
            return user;
        }
    }
}
