namespace Sheltos.ViewModel.Admin
{
    public class AdminDashBoardViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

       
        public int TotalProperties { get; set; }
        public int TotalUsers { get; set; }
        public int TotalAgents { get; set; }
        public int TotalPayments { get; set; }
        public int PendingPayments { get; set; }
        public int PaidPayments { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
