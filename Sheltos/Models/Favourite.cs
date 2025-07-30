namespace Sheltos.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
