namespace Sheltos.ViewModel.MessageViewModel
{
    public class ChatViewModel
    {
        public ChatViewModel() 
        { 
            Messages = new List<UserMessageViewModel>();
        }
        public string CurrentUserId { get; set; }
        public string ReceiverId { get; set; }  
        public string ReceiverName { get; set; }

        public IEnumerable<UserMessageViewModel> Messages { get; set; }

    }
}
