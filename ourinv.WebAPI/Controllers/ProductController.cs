using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{productName}")]
        public IActionResult GetProduct(string productName)
        {
            var existingProduct = getProductByName(productName);

            return Ok(convertToDTO(existingProduct));
        }

        private BaseProductDTO convertToDTO(Product product)
        {
            return new()
            {
                Name = product.Name,
                Quantity = product.Quantity,
                Category = new()
                {
                    Name = product.Category.Name
                }
            };
        }

        private Product getProductByName(string productName)
        {
            var existingProduct = _context.Products.Where(x => x.Name == productName).Include(x => x.Category).SingleOrDefault();

            if (existingProduct == null)
            {
                throw new Exception($"The product with name {productName} doesn't exist!");
            }

            return existingProduct;
        }
    }
}
