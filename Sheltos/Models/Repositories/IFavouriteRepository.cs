namespace Sheltos.Models.Repositories
{
    public interface IFavouriteRepository
    {
        Task<List<int>> GetAllFavouritesById(string userId);
    }
}