using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 3000,
                Count = 23,
            },
            new Product
            {
                Id = 2,
                Name = "Product2",
                Price = 241324,
                Count = 456,
            },
        };

        private const string API_KEY = "X-API-KEY";

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int Id)
        {
            var product = _products.FirstOrDefault(p => p.Id == Id);
            return product is not null ? Ok(product) : NotFound();
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] Product newProduct,
            [FromHeader(Name = "X-API-KEY")] string apiKey
        )
        {
            if (apiKey != API_KEY)
                return Unauthorized();
            newProduct.Id = _products.Max(p => p.Id) + 1;
            _products.Add(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] Product updatedProduct,
            [FromHeader(Name = "X-API-KEY")] string apiKey
        )
        {
            if (apiKey != API_KEY)
                return Unauthorized();
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return NotFound();
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Count = updatedProduct.Count;
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader(Name = "X-API-KEY")] string apiKey)
        {
            if (apiKey != API_KEY)
                return Unauthorized();
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return NotFound();
            _products.Remove(product);
            return NoContent();
        }
    }
}
