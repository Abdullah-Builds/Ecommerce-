using Microsoft.AspNetCore.Mvc;
using MyApp.Data;
using MyApp.Models;
using Newtonsoft.Json;

namespace MyApp.Controllers
{
    public class CartController : Controller
    {
        private const string SessionKey = "CartSession";

        private readonly CartDbContext _context;

        public CartController(CartDbContext context)
        {
            _context = context;
        }


        public IActionResult Cart()
        {
            var productsInCart = GetCartFromSession();

            var cartModel = new CartModel
            {
                ItemsSelected = new List<UserModel>
                {
                  new UserModel
                  {
                    Products = productsInCart
                  }

                }

            };

            return View(cartModel);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] ProductModel product)
        {
            if (product == null)
                return BadRequest("Product cannot be null.");


            var cart = GetCartFromSession();
            cart.Add(product);
            SaveCartToSession(cart);

            return Ok(new { message = "Item added to cart" });
        }

        private List<ProductModel>? GetCartFromSession()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            return string.IsNullOrEmpty(sessionData)
                ? new List<ProductModel>()
                : JsonConvert.DeserializeObject<List<ProductModel>>(sessionData);
        }

        private void SaveCartToSession(List<ProductModel> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SessionKey, json);
        }
        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] int productId)
        {
            var cartJson = HttpContext.Session.GetString("CartSession");

            var cart = string.IsNullOrEmpty(cartJson)
                ? new List<ProductModel>()
                : JsonConvert.DeserializeObject<List<ProductModel>>(cartJson);

            // Remove by ID
            cart = cart.Where(p => p.Id != productId).ToList(); // No type mismatch now

            // Save updated cart
            HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cart));

            return Json(new { success = true });
        }


        //===========
        private CartModel GetPreparedCartModel(CartModel UserPurchased)
        {
            const string SessionKey = "CartSession";
            var sessionData = HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(sessionData))
                return new CartModel();

            var products = JsonConvert.DeserializeObject<List<ProductModel>>(sessionData);

            if (products == null || !products.Any())
                return new CartModel();

            

            var userItems = UserPurchased.Mapping;
            var userModels = new List<UserModel>();

            if(userItems == null || !userItems.Any())
                return new CartModel();

            foreach (var user in userItems)
            {
                var matchedProducts = new List<ProductModel>();

                    var matchedProduct = products.FirstOrDefault(p => p.Id == user.ProductId);
                    if (matchedProduct != null)
                    {

                        userModels.Add(new UserModel
                        {
                            Quantity = user.Quantity,
                            Products = new List<ProductModel> { matchedProduct },
                        });
                    }
                

            }

            var result =  new CartModel
            {
                ItemCount = UserPurchased.ItemCount,
                TotalPrice = UserPurchased.TotalPrice,
                Tax = UserPurchased.Tax,
                Discount = UserPurchased.Discount,
                DeliveryCharges = UserPurchased.DeliveryCharges,
                ItemsSelectedJson = JsonConvert.SerializeObject(userModels)
            };


            return result;  
        }


        [HttpPost]
        public async Task<IActionResult> AddToCartDB([FromBody] CartModel UserPurchased)
        {
            var cartModel = GetPreparedCartModel(UserPurchased);

            if (cartModel == null  )
                return BadRequest("Cart is empty or invalid.");

            var existingCart = await _context.Carts.FindAsync(cartModel.Id);

            if (existingCart == null)
            {
                _context.Carts.Add(cartModel);
            }
            else
            {
                existingCart.ItemsSelected = cartModel.ItemsSelected;
                existingCart.ItemCount = cartModel.ItemCount;
                existingCart.TotalPrice = cartModel.TotalPrice;
                existingCart.Tax = cartModel.Tax;
                existingCart.Discount = cartModel.Discount;
                existingCart.DeliveryCharges = cartModel.DeliveryCharges;

                _context.Carts.Update(existingCart);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cart saved to database successfully." });
        }
    }
}
