using Microsoft.AspNetCore.Mvc;
using RazorWebsite.Models;
using RazorWebsite.Extensions;

public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context;

    public CheckoutController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Checkout
    public IActionResult Index()
    {
        // Get cart from session
        var cartItems = HttpContext.Session.Get<List<CartItem>>("Cart");

        if (cartItems == null || cartItems.Count == 0)
        {
            // If cart is empty, redirect back to store or cart page
            return RedirectToAction("Index", "Store");
        }

        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalPrice = cartItems.Sum(item => (item.Product?.Price ?? 0) * item.Quantity),
  
        };

        return View("Checkout", viewModel);
    }

    // POST: /Checkout
    [HttpPost]
    public IActionResult Checkout(CheckoutViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Handle the order logic here, e.g., saving order to database, payment processing

            // Clear the cart after order completion
            HttpContext.Session.Remove("Cart");

            // Redirect to a confirmation or success page
            return RedirectToAction("Success");
        }

        // If model is invalid, redisplay the checkout page with validation errors
model.CartItems = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();


if (model.CartItems != null)
{
    model.TotalPrice = model.CartItems.Sum(item => (item.Product?.Price ?? 0) * item.Quantity);
}
else
{
    model.TotalPrice = 0;
}

        return View("Checkout", model);
    }

    // GET: /Checkout/Success
    public IActionResult Success()
    {
        return View();
    }
}
