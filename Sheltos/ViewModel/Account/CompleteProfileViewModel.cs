namespace Sheltos.ViewModel
{
    public class CompleteProfileViewModel
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}
