using System.ComponentModel.DataAnnotations;

namespace Sheltos.ViewModel.Agents
{
    public class PropertyRequestViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
       
        public string? Email { get; set; }
        
        public string? Subject { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
