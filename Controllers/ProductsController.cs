using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.Where(p => p.IsActive).Select(p =>ProductToDto(p)).ToListAsync();
            if(products == null)
            {
                return NotFound(); // 404 status code
            }
            return Ok(products);// 200 status code
        }
        [HttpGet("{id}")] //api/controller/id
        [Authorize]
        public async Task<IActionResult> GetProduct(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Where(x => x.ProductId == id).Select(p =>ProductToDto(p)).FirstOrDefaultAsync();
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new {id = product.ProductId}, product); // 201 status code 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if(id != product.ProductId)
            {
                return BadRequest();
            }
            var productToUpdate = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if(productToUpdate == null)
            {
                return NotFound();
            }
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.Price = product.Price;
            productToUpdate.IsActive = product.IsActive;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return NotFound();
            }
            return NoContent();//204 status code
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        private static ProductDto ProductToDto(Product p)
        {
            var entity = new ProductDto();
            if(p != null)
            {
                entity.ProductId = p.ProductId;
                entity.ProductName = p.ProductName;
                entity.Price = p.Price;
            }
            return entity;
        }
    }
}
