using System.ComponentModel.DataAnnotations.Schema;

namespace Sheltos.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public ApplicationUser Receiver { get; set; }
        public string Content { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
