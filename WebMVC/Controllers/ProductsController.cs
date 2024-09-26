using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

public class ProductsController : Controller
{
    private readonly FoodDBContext _context;
    public ProductsController(FoodDBContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.FoodProducts.ToList();
        return View(products);
    }

    // For AJAX search
    public IActionResult Search(string search)
    {
        var products = string.IsNullOrEmpty(search)
            ? _context.FoodProducts.ToList()
            : _context.FoodProducts.Where(p => p.ProductName.Contains(search) || p.Category.Contains(search)).ToList();

        return PartialView("_ProductList", products); 
    }

    public IActionResult Details(int id)
    {
        var product = _context.FoodProducts.Find(id);
        if (product == null)
        {
            return NotFound();
        }
        return Json(product); // Return product as JSON
    }

    [HttpPost]
    public IActionResult Create([FromBody] FoodProduct product)
    {
        if (ModelState.IsValid)
        {
            _context.FoodProducts.Add(product);
            _context.SaveChanges();
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    public IActionResult Edit(int id, [FromBody] FoodProduct product)
    {
        var existingProduct = _context.FoodProducts.Find(id);
        if (existingProduct == null) return NotFound();

        if (ModelState.IsValid)
        {
            existingProduct.ProductName = product.ProductName;
            existingProduct.Category = product.Category;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            _context.SaveChanges();
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var product = _context.FoodProducts.Find(id);
        if (product != null)
        {
            _context.FoodProducts.Remove(product);
            _context.SaveChanges();
        }
        return Ok();
    }
}
