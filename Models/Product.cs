namespace RazorWebsite.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } // URL for the product image
        public int Stock { get; set; } // Available stock for the product
    }
}
