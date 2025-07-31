using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class PagesRepository : IPagesRepository
    {
        private readonly ApplicationDbContext _context;
        public PagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddMessageAsync(ContactUs contact)
        {
            _context.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<ContactUs> GetMessageByIdAsync(int id)
        {
            return await _context.ContactUs.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ContactUs>> GetAllMessagesAsync()
        {
            return await _context.ContactUs.ToListAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = await GetMessageByIdAsync(id);
            if (message != null)
            {
                _context.ContactUs.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Markreplied(int id)
        {
            var message = await _context.ContactUs.FindAsync(id);
            if (message != null)
            {
                message.IsReplied = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
