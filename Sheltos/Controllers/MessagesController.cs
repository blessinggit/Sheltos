using Microsoft.AspNetCore.Mvc;
using Sheltos.Models.Repositories;

namespace Sheltos.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _messageService.GetUsers();
            return View(users);
        }
        public async Task<IActionResult> Chat(string selectedUserId)
        {
            if (string.IsNullOrEmpty(selectedUserId))
            {
                return RedirectToAction("Index");
            }
            var chatViewModel = await _messageService.GetMessages(selectedUserId);
            if (chatViewModel == null)
            {
                return NotFound();
            }
            return View(chatViewModel);
        }
    }
}
