using Sheltos.Data;
using Sheltos.ViewModel.MessageViewModel;

namespace Sheltos.Models.Repositories
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<ChatViewModel> GetMessages(string selectedUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageUserListViewModel>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
