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
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{categoryName}")]
        public IActionResult GetCategory(string categoryName)
        {
            var existingCategory = getCategoryByName(categoryName);

            return Ok(convertToDTO(existingCategory));
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            if (_context.Categories.Any(x => x.Name == createCategoryDTO.CategoryName))
            {
                throw new Exception($"The category with name {createCategoryDTO.CategoryName} already exists!");
            }

            Category newCategory = new() { Id = _context.Categories.Max(x => x.Id) + 1, Name = createCategoryDTO.CategoryName };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { categoryName = createCategoryDTO.CategoryName }, newCategory);
        }

        [HttpPut("{categoryName}")]
        public IActionResult UpdateCategory(string categoryName, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            if (_context.Categories.Any(x => x.Name == updateCategoryDTO.NewCategoryName))
            {
                throw new Exception($"The category with name {updateCategoryDTO.NewCategoryName} already exists!");
            }

            var categoryToBeUpdated = getCategoryByName(categoryName);
            categoryToBeUpdated.Name = updateCategoryDTO.NewCategoryName;
            _context.SaveChanges();

            return Ok(convertToDTO(categoryToBeUpdated));
        }

        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory(string categoryName)
        {
            var categoryToBeDeleted = getCategoryByName(categoryName);

            _context.Categories.Remove(categoryToBeDeleted);
            _context.SaveChanges();

            return Ok(convertToDTO(categoryToBeDeleted));
        }

        private BaseCategoryDTO convertToDTO(Category category)
        {
            BaseCategoryDTO categoryDTO = new();
            categoryDTO.Name = category.Name;

            if (category.Products.Any())
            {
                List<BaseProductDTO> formattedProducts = new();
                foreach (var product in category.Products)
                {
                    formattedProducts.Add(new() { Name = product.Name, Quantity = product.Quantity });
                }

                categoryDTO.Products = formattedProducts;
            }

            return categoryDTO;
        }

        private Category getCategoryByName(string categoryName)
        {
            var existingCategory = _context.Categories.Include(x => x.Products).Where(x => x.Name == categoryName).SingleOrDefault();

            if (existingCategory == null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategory;
        }
    }
}
