namespace Sheltos.ViewModel.User
{
    public class EditProfileViewModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
