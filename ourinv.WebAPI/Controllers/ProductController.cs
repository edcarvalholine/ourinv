using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;
using ourinv.WebAPI.Services;

namespace ourinv.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productName}")]
        public IActionResult GetProduct(string productName)
        {
            return Ok(_productService.GetProduct(productName));
        }

        [HttpDelete("{productName}")]
        public IActionResult DeleteProduct(string productName)
        {
            return Ok(_productService.DeleteProduct(productName));
        }

        [HttpPut("{productName}")]
        public IActionResult UpdateProduct(string productName, UpdateProductDTO product)
        {
            return Ok(_productService.UpdateProduct(productName, product));
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductDTO product)
        {
            var newProduct = _productService.CreateProduct(product);

            return CreatedAtAction(nameof(GetProduct), new { productName = newProduct.Name }, newProduct);
        }
    }
}
