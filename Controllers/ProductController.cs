using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotShopApi.Data;
using DotShopApi.Models;
using DotShopApi.DTO;

namespace DotShopApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Fetch all products. Returns an empty list or a list of product objects
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            // Fetch product by the given ID, returns null or a product object
            var product = await GetActiveProduct(id);

            // If the product is not found, return a 404
            if (product == null)
            {
                return NotFound();
            }

            // If found, then retun the product object
            return product;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, ProductDTOCreateUpdate productDTOCreateUpdate)
        {

            // Fetch product by the given ID, returns null or a product object
            var product = await GetActiveProduct(id);

            // If the product is not found, return a 404
            if (product == null)
            {
                return NotFound();
            }

            // Update product values
            product.Name = productDTOCreateUpdate.Name;
            product.Price = productDTOCreateUpdate.Price;
            product.Quantity = productDTOCreateUpdate.Quantity;
            product.DateModified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTOCreateUpdate productDTOCreateUpdate)
        {
            // Create a new product based on the given product DTO
            var product = new Product
            {
                Id = DotShopApi.Utils.GenerateRandomId(),
                Name = productDTOCreateUpdate.Name,
                Quantity = productDTOCreateUpdate.Quantity,
                Price = productDTOCreateUpdate.Price,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Deleted = false
            };

            // Persit and save in the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Return created product
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            // Fetch product by the given ID, returns null or a product object
            var product = await GetActiveProduct(id);

            // If the product is not found, return a 404
            if (product == null)
            {
                return NotFound();
            }

            // Set product as deleted
            product.Deleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Fetch product if by id and not deleted
        private async Task<Product> GetActiveProduct(long id)
        {
            return await _context.Products.Where(pr => !pr.Deleted && pr.Id == id).FirstOrDefaultAsync<Product>();
        }
    }
}
