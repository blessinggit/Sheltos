using Microsoft.EntityFrameworkCore;
using Sheltos.Models;

namespace Sheltos.Models.Repositories
{
    public interface IPagesRepository
    {
        Task AddMessageAsync(ContactUs contact);
        Task<ContactUs> GetMessageByIdAsync(int id);
        Task<List<ContactUs>> GetAllMessagesAsync();
        Task DeleteMessageAsync(int id);
        Task Markreplied(int id);
    }
}
