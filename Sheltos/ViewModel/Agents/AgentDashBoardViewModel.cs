namespace Sheltos.ViewModel.Agents
{
    public class AgentDashBoardViewModel
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentEmail { get; set; }
        public string AgentPhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AgentImage { get; set; }
        public string AgentAddress { get; set; }
        public int PropertyCount { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }

    }
}
