namespace Sheltos.Models.Repositories
{
    public interface ICardRepository
    {
        Task<List<Card>> GetCardsByUserId(string userId);
        Task AddCardAsync(Card card);
        Task<Card> GetCardByIdAsync(int id);
        Task UpdateCardAsync(Card card);
        Task DeleteCardAsync(Card card);
    }
}