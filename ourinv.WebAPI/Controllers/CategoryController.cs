using Microsoft.AspNetCore.Mvc;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.Services;

namespace ourinv.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CategoryService _categoryService;
        public CategoryController(AppDbContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        [HttpGet("{categoryName}")]
        public IActionResult GetCategory(string categoryName)
        {
            var existingCategory = _categoryService.GetCategory(categoryName);

            return Ok(existingCategory);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var existingCategories = _categoryService.GetAllCategories();

            return Ok(existingCategories);
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            var newCategory = _categoryService.CreateCategory(createCategoryDTO);

            return CreatedAtAction(nameof(GetCategory), new { categoryName = createCategoryDTO.CategoryName }, newCategory);
        }

        [HttpPut("{categoryName}")]
        public IActionResult UpdateCategory(string categoryName, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            var categoryUpdated = _categoryService.UpdateCategory(new() { CategoryName = categoryName }, updateCategoryDTO);

            return Ok(categoryUpdated);
        }

        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory(string categoryName)
        {
            var categoryToBeDeleted = _categoryService.DeleteCategory(new()
            {
                Name = categoryName
            });

            return Ok(categoryToBeDeleted);
        }

        [HttpGet("{categoryName}/product")]
        public IActionResult GetCategoryWithProduct(string categoryName)
        {
            var existingCategory = _categoryService.GetCategoryWithProducts(new()
            {
                Name = categoryName
            });

            return Ok(existingCategory);
        }

        [HttpGet("product")]
        public IActionResult GetCategoriesWithProducts()
        {
            var existingCategories = _categoryService.GetCategoriesWithProducts();

            return Ok(existingCategories);
        }
    }
}
