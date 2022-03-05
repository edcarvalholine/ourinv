using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
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

        [HttpGet("Category/{categoryName}")]
        public IActionResult GetProductsFromCategory(string categoryName)
        {
            var existingProductWithCategories = getProductsFromCategoryName(categoryName);

            return Ok(existingProductWithCategories);
        }

        [HttpPost]
        public IActionResult CreateProdct(string? categoryName, string productName, int productQuantity)
        {
            if (categoryName == null)
            {
                categoryName = "Unknown";
            }

            var productCategory = _context.Categories.SingleOrDefault(x => x.Name == categoryName);
            var existingProduct = _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Name == productName);
            if (existingProduct != null)
            {
                throw new Exception($"The product with name {productName} already exists in category {categoryName}!");
            }

            Product newProduct = new()
            {
                Name = productName,
                Quantity = productQuantity,
                Category = productCategory
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduct), new { productName = newProduct.Name }, newProduct);
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

        private BaseCategoryDTO getProductsFromCategoryName(string categoryName)
        {
            var existingCategory = _context.Categories.Include(x => x.Products).Where(x => x.Name == categoryName).SingleOrDefault();
            if (existingCategory == null)
            {
                throw new Exception($"The categoryName with name {categoryName} doesn't exist!");
            }


            List<BaseProductDTO> formattedProducts = new();
            foreach (var product in existingCategory.Products)
            {
                formattedProducts.Add(new() {  Name = product.Name, Quantity = product.Quantity });
            }

            return new()
            {
                Name = existingCategory.Name,
                Products = formattedProducts 
            };
        }
    }
}
