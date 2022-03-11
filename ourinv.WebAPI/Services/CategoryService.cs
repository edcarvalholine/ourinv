using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseCategoryDTO GetCategory(string categoryName)
        {
            var category = GetCategoryByName(categoryName);

            return _mapper.Map<BaseCategoryDTO>(category);
        }

        public IEnumerable<BaseCategoryDTO> GetAllCategories()
        {
            var categories = _context.Categories;

            return _mapper.Map<IEnumerable<BaseCategoryDTO>>(categories);
        }

        public BaseCategoryDTO CreateCategory(CreateCategoryDTO categoryDTO)
        {
            var newCategory = _mapper.Map<Category>(categoryDTO);

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return _mapper.Map<BaseCategoryDTO>(newCategory);
        }

        public BaseCategoryDTO UpdateCategory(CreateCategoryDTO categoryDTO, UpdateCategoryDTO newCategoryDTO)
        {
            var categoryToBeUpdated = GetCategoryByName(categoryDTO.CategoryName);

            _mapper.Map(newCategoryDTO, categoryToBeUpdated);

            _context.SaveChanges();

            return _mapper.Map<BaseCategoryDTO>(categoryToBeUpdated);
        }

        public BaseCategoryDTO DeleteCategory(BaseCategoryDTO categoryDTO)
        {
            var categoryToDelete = GetCategoryByName(categoryDTO.Name);

            _context.Categories.Remove(categoryToDelete);
            _context.SaveChanges();

            return _mapper.Map<BaseCategoryDTO>(categoryToDelete);
        }

        public BaseCategoryDTO GetCategoryWithProducts(BaseCategoryDTO categoryDTO)
        {
            var category = GetCategoryWithProductsByName(categoryDTO.Name);

            return _mapper.Map<BaseCategoryDTO>(category);
        }

        public IEnumerable<BaseCategoryDTO> GetCategoriesWithProducts()
        {
            var categories = _context.Categories.Include(x => x.Products);

            return _mapper.Map<IEnumerable<BaseCategoryDTO>>(categories);
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