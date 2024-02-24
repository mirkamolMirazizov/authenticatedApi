using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MyLibraryProject; // Replace with your actual namespace

namespace MyApiProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCart
        [HttpGet]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userEmail = User.Identity.Name; // Assuming email is used as username
            var cart = await _context.ShoppingCarts
                            .Include(c => c.Products)
                            .Where(c => c.User == userEmail)
                            .FirstOrDefaultAsync();

            if (cart == null) return NotFound("Shopping cart not found.");

            return Ok(cart);
        }

        // POST: api/ShoppingCart/AddProduct/5
        [HttpPost("AddProduct/{id}")]
        public async Task<IActionResult> AddProduct(int id)
        {
            var userEmail = User.Identity.Name;
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound("Product not found.");

            var cart = await _context.ShoppingCarts
                            .Include(c => c.Products)
                            .Where(c => c.User == userEmail)
                            .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    User = userEmail,
                    Products = new List<Product> { product }
                };
                _context.ShoppingCarts.Add(cart);
            }
            else
            {
                cart.Products.Add(product);
            }

            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        // POST: api/ShoppingCart/RemoveProduct/5
        [HttpPost("RemoveProduct/{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var userEmail = User.Identity.Name;
            var cart = await _context.ShoppingCarts
                            .Include(c => c.Products)
                            .Where(c => c.User == userEmail)
                            .FirstOrDefaultAsync();

            if (cart == null) return NotFound("Shopping cart not found.");

            var product = cart.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound("Product not in the cart.");

            cart.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(cart);
        }
    }
}
