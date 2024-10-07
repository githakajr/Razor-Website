using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorWebsite.Models;
using RazorWebsite.Extensions; 
namespace RazorWebsite.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // GET: Store/AddToCart/{id}
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.Stock <= 0)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.Find(item => item.ProductId == id);
            if (cartItem == null)
            {
                cart.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            HttpContext.Session.Set("Cart", cart); // Use the extension method to set the cart
            return RedirectToAction("Index");
        }

        // GET: Store/Cart
        public IActionResult Cart()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // POST: Store/Checkout
        [HttpPost]
        public IActionResult Checkout()
        {
            // Checkout logic (e.g., process payment, save order, etc.)
            HttpContext.Session.Remove("Cart"); // Clear the cart after checkout
            return RedirectToAction("Index");
        }
    }
}
