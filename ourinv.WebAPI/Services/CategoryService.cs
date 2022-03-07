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

        public BaseCategoryDTO GetCategory(string categoryName)
        {
            var category = GetCategoryByName(categoryName);

            return new()
            {
                Name = category.Name,
            };
        }

        public IEnumerable<BaseCategoryDTO> GetAllCategories()
        {
            var categories = _context.Categories;
            List<BaseCategoryDTO> categoriesDTO = new List<BaseCategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDTO.Add(new()
                {
                    Name = category.Name
                });
            }

            return categoriesDTO;
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

        public BaseCategoryDTO GetCategoryWithProducts(BaseCategoryDTO categoryDTO)
        {
            var category = GetCategoryWithProductsByName(categoryDTO.Name);

            return new()
            {
                Name = category.Name,
                Products = ConvertProductToDto(category.Products)
            };
        }

        public IEnumerable<BaseCategoryDTO> GetCategoriesWithProducts()
        {
            var categories = _context.Categories;

            var categoryDTOs = new List<BaseCategoryDTO>();
            foreach (var category in categories)
            {
                var rawCategory = GetCategoryWithProductsByName(category.Name);
                BaseCategoryDTO categoryMapped = new()
                {
                    Name = rawCategory.Name
                };

                var productsMapped = rawCategory.Products != null ? ConvertProductToDto(category.Products) : null;
                categoryMapped.Products = productsMapped;

                categoryDTOs.Add(categoryMapped);
            }

            return categoryDTOs;
        }

        private static IEnumerable<BaseProductDTO> ConvertProductToDto(IEnumerable<Product> products)
        {
            var categoryProductsDTO = new List<BaseProductDTO>();

            foreach (var product in products)
            {
                categoryProductsDTO.Add(new()
                {
                    Name = product.Name,
                    Quantity = product.Quantity
                });
            }

            return categoryProductsDTO;
        }

        #region Helpers
        public Category GetCategoryByName(string categoryName)
        {
            categoryValidationException(categoryName);

            var existingCategory = _context.Categories.SingleOrDefault(x => x.Name == categoryName);
            if (existingCategory is null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategory;
        }

        public Category GetCategoryWithProductsByName(string categoryName)
        {
            categoryValidationException(categoryName);

            var existingCategoryWithProducts = _context.Categories.Include(x => x.Products).SingleOrDefault(x => x.Name == categoryName);
            if (existingCategoryWithProducts is null)
            {
                throw new Exception($"The category with name {categoryName} doesn't exist!");
            }

            return existingCategoryWithProducts;
        }

        public bool CheckCategoryExistence(string categoryName)
        {
            categoryValidationException(categoryName);

            return _context.Categories.Any(x => x.Name == categoryName);
        }

        private void categoryValidationException(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new Exception($"The category is empty!");
            }
        }
        #endregion
    }
}