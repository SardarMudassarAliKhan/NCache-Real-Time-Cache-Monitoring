using Microsoft.AspNetCore.Mvc;
using NCache_Real_Time_Cache_Monitoring.IRepository;
using NCache_Real_Time_Cache_Monitoring.Model;

namespace NCache_Real_Time_Cache_Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository<Product> _repository;

        public ProductsController(IProductRepository<Product> repository)
        {
            _repository = repository;
        }

        // POST: api/products (Add a new product)
        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("Invalid product data.");
            }

            _repository.Add(product); // This handles both DB insert and cache insertion
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // GET: api/products/{id} (Retrieve a product by ID)
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _repository.Get(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        // DELETE: api/products/{id} (Delete a product by ID)
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repository.Get(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _repository.Delete(id); // This will handle cache invalidation as well
            return NoContent(); // Return 204 No Content status
        }
    }
}
