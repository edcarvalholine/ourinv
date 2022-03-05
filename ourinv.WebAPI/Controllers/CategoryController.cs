using Microsoft.AspNetCore.Mvc;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
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

            return CreatedAtAction(nameof(GetCategory), new { categoryName = createCategoryDTO.CategoryName }, convertToDTO(newCategory));
        }

        [HttpPut("{categoryName}")]
        public IActionResult UpdateCategory(string categoryName, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            var categoryToBeUpdated = getCategoryByName(categoryName);

            if (_context.Categories.Any(x => x.Name == updateCategoryDTO.NewCategoryName))
            {
                throw new Exception($"The category with name {updateCategoryDTO.NewCategoryName} already exists!");
            }

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
            return new()
            {
                Name = category.Name,
            };
        }

        private Category getCategoryByName(string categoryName)
        {
            var existingCategory = _context.Categories.Where(x => x.Name == categoryName).SingleOrDefault();
            if (existingCategory == null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategory;
        }
    }
}
