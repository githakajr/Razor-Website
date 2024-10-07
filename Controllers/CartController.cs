using Microsoft.AspNetCore.Mvc;
using RazorWebsite.Extensions;
using RazorWebsite.Models; 


namespace RazorWebsite.Controllers
{
    public class CartController : Controller
    {
        // Action to view the cart
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cartItems);
        }

        // Action to add an item to the cart
        [HttpPost]
        public IActionResult AddToCart(Product product)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingCartItem = cartItems.FirstOrDefault(c => c.ProductId == product.Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Product = product // Assuming the Product object is also being passed
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cartItems);
            return RedirectToAction("Index");
        }

        // Action to remove an item from the cart
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cartItems);
            return RedirectToAction("Index");
        }

        // Action to clear the cart
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
