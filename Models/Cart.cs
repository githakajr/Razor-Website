namespace RazorWebsite.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product? Product { get; set; } // Navigation property

        public decimal TotalPrice => Product?.Price * Quantity ?? 0;
    }
}
