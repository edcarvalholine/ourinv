using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories;
        }

        public BaseCategoryDTO CreateCategory(CreateCategoryDTO categoryDTO)
        {
            Category newCategory = new()
            {
                Name = categoryDTO.CategoryName,
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return new()
            {
                Name = newCategory.Name,
            };
        }

        public BaseCategoryDTO UpdateCategory(CreateCategoryDTO categoryDTO, UpdateCategoryDTO newCategoryDTO)
        {
            var categoryToBeUpdated = GetCategoryByName(categoryDTO.CategoryName);
            categoryToBeUpdated.Name = newCategoryDTO.NewCategoryName;

            _context.SaveChanges();

            return new()
            {
                Name = categoryToBeUpdated.Name
            };
        }

        public BaseCategoryDTO DeleteCategory(BaseCategoryDTO categoryDTO)
        {
            var categoryToDelete = GetCategoryByName(categoryDTO.Name);

            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();

            return new()
            {
                Name = categoryToDelete.Name,
            };
        }

        public BaseCategoryDTO GetCategoryWithProduct(BaseCategoryDTO categoryDTO)
        {
            var categoryWithProducts = GetCategoryWithProductsByName(categoryDTO.Name);
            BaseCategoryDTO categoryMapper = new();
            var productsMapped = new List<BaseProductDTO>();

            foreach (var product in categoryWithProducts.Products)
            {
                productsMapped.Add(new()
                {
                    Name = product.Name,
                    Quantity = product.Quantity
                });
            }

            categoryMapper.Products = productsMapped;

            return categoryMapper;
        }

        #region Helpers
        public Category GetCategoryByName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new Exception($"The category is empty!");
            }

            var existingCategory = _context.Categories.SingleOrDefault(x => x.Name == categoryName);
            if (existingCategory is null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategory;
        }

        public Category GetCategoryWithProductsByName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new Exception($"The category is empty!");
            }

            var existingCategoryWithProducts = _context.Categories.Include(x => x.Products).SingleOrDefault(x => x.Name == categoryName);
            if (existingCategoryWithProducts is null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategoryWithProducts;
        }

        public bool CheckCategoryExistence(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new Exception($"The category is empty!");
            }

            return _context.Categories.Any(x => x.Name == categoryName);
        }
        #endregion
    }
}
