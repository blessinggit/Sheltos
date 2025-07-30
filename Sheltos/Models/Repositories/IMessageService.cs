using Sheltos.ViewModel.MessageViewModel;

namespace Sheltos.Models.Repositories
{
    public interface IMessageService
    {
        Task <IEnumerable<MessageUserListViewModel>> GetUsers();
        Task<ChatViewModel> GetMessages(string selectedUserId);
    }
}