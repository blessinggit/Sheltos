using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CardRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Card>> GetCardsByUserId(string userId)
        {
            var cards = await _context.Cards
                 .Where(c => c.UserId == userId)
                 .ToListAsync();
            return cards;
        }

        public async Task AddCardAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task<Card> GetCardByIdAsync(int id)
        {
            return await _context.Cards
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateCardAsync(Card card)
        {
            _context.Cards.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCardAsync(Card card)
        {
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
        }
    }
}
    