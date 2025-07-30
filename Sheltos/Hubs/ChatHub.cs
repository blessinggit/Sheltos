using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Sheltos.Data;
using Sheltos.Models;
using Sheltos.Models.Repositories;

namespace Sheltos.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        
        public ChatHub(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task SendMessage(string recieverId, string message)
        {
            var NewDate = DateTime.UtcNow;
            var date = NewDate.ToShortDateString();
            var time = NewDate.ToShortTimeString();

            string senderId = _currentUserService.UserId;

            var messageToAdd = new ChatMessage()
            {
                Content = message,
                SenderId = senderId,
                Timestamp = NewDate,
                ReceiverId = recieverId
            };
           
            await _context.AddAsync(messageToAdd);
            await _context.SaveChangesAsync();

            List<string> users = new List<string> { recieverId, senderId };
            await Clients.Users(users).SendAsync("ReceiveMessage", messageToAdd.Content, senderId, recieverId, date, time);
        }
    }
}
