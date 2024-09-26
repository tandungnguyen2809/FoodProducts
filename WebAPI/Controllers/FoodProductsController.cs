using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodProductsController : ControllerBase
    {
        private readonly FoodDBContext _context;

        public FoodProductsController(FoodDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodProduct>>> GetFoodProducts()
        {
            return await _context.FoodProducts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodProduct>> GetFoodProduct(int id)
        {
            var product = await _context.FoodProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<FoodProduct>> PostFoodProduct(FoodProduct product)
        {
            _context.FoodProducts.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFoodProduct", new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodProduct(int id, FoodProduct product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodProduct(int id)
        {
            var product = await _context.FoodProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.FoodProducts.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodProductExists(int id)
        {
            return _context.FoodProducts.Any(e => e.ProductId == id);
        }
    }
}
