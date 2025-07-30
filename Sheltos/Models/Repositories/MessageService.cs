using Microsoft.EntityFrameworkCore;
using Sheltos.Data;
using Sheltos.ViewModel.MessageViewModel;

namespace Sheltos.Models.Repositories
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public MessageService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<ChatViewModel> GetMessages(string selectedUserId)
        {
            var currentUserId = _currentUserService.UserId;

            var selectedUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == selectedUserId);
            var selectedUserName = "";
            if(selectedUser != null)
            {
                selectedUserName = selectedUser.UserName;
            }
            var chatViewModel = new ChatViewModel()
            {
                CurrentUserId = currentUserId,
                ReceiverId = selectedUserId,
                ReceiverName = selectedUserName,
            };
            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == selectedUserId) ||
                            (m.SenderId == selectedUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new UserMessageViewModel
                {
                    Id = m.Id,
                    Text = m.Content,
                    Date = m.Timestamp.ToShortDateString(),
                    Time = m.Timestamp.ToShortTimeString(),
                    IsCurrentUserSentMessage = m.SenderId == currentUserId
                })
                .ToListAsync();
            chatViewModel.Messages = messages;
            return chatViewModel;
        }

        public async Task<IEnumerable<MessageUserListViewModel>> GetUsers()
        {
            var currentUserId = _currentUserService.UserId;

            var users = await _context.Users
                .Where(u => u.Id != currentUserId)
                .Select(u => new MessageUserListViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    LastMessage = _context.ChatMessages.Where(m => (m.SenderId == currentUserId || m.SenderId == u.Id) && (m.ReceiverId == currentUserId || m.ReceiverId == u.Id))
                        .OrderByDescending(m => m.Timestamp)
                        .Select(m => m.Content)
                        .FirstOrDefault()

                })
                .ToListAsync();
            return users;
        }
    }
}
