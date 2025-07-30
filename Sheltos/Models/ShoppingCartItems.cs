namespace Sheltos.Models
{
    public class ShoppingCartItems
    {
        public int Id { get; set; }
        public string? ShoppingCartId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; } = default!;
        public int Quantity { get; set; }
        public string? UserId { get; set; } 


    }
}
