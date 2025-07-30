namespace Sheltos.Models.Repositories
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Task<ApplicationUser> GetUser();
    }
}