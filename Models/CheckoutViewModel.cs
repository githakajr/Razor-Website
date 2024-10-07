using RazorWebsite.Models;

public class CheckoutViewModel
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public decimal TotalPrice { get; set; }
    
    // For user details
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? PaymentMethod { get; set; }
}
