using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.DTOs.CategoryDTO;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;
using ourinv.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xUnitTest.ServiceTest
{
    public class CategoryService_Test
    {
        private static IMapper _mapper;
        private CategoryService _categoryService;
        public CategoryService_Test()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
            });
            _mapper = config.CreateMapper();

            
            _categoryService = new(configContext(), _mapper);
        }

        [Fact]
        public void GetCategory()
        {
            // Arrange
            Category newCategory = new ()
            {
                Name = "Category 1"
            };

            using var context = configContext();
            context.Categories.Add(newCategory);
            context.SaveChanges();
            _categoryService = new(context, _mapper);

            // Assert
            BaseCategoryDTO result = _categoryService.GetCategory(newCategory.Name);
            result.Name.Should().Be(newCategory.Name);
            result.Products.Should().BeNull();
        }

        [Fact]
        public void GetAllCategories()
        {
            // Arrange
            var cateogryEntities = new List<Category>()
            {
                new() { Id = 1, Name = "Category 1" },
                new() { Id = 2, Name = "Category 2" },
            };
            
            using var context = configContext();
            context.Categories.AddRange(cateogryEntities);
            context.SaveChanges();
            _categoryService = new(context, _mapper);

            // Assert
            Assert.Equal(cateogryEntities.Count, _categoryService.GetAllCategories().ToList().Count);
        }

        [Fact]
        public BaseCategoryDTO CreateCategory()
        {
            _categoryService = new(configContext(), _mapper);

            // Arrange
            CreateCategoryDTO newCategory = new()
            {
                CategoryName = "Teste"
            };

            // Act
            var result = _categoryService.CreateCategory(newCategory);

            // Assert
            Assert.Equal(newCategory.CategoryName, result.Name);

            return result;
        }

        [Fact]
        public BaseCategoryDTO DeleteCategory()
        {
            // Arrange
            Category newCategory = new() { Name = "Teste" };
            var context = configContext();
            context.Categories.Add(newCategory);
            context.SaveChanges();

            _categoryService = new(context, _mapper);
            // Act
            var result = _categoryService.DeleteCategory(new() { Name = newCategory.Name });
            var categoryExists = _categoryService.CheckCategoryExistence(newCategory.Name);

            // Assert
            Assert.Equal(newCategory.Name, result.Name);
            Assert.False(categoryExists);

            return result;
        }

        [Fact]
        public void UpdateCategory()
        {
            // Arrange
            Category newCategory = new() { Name = "Teste" };
            var context = configContext();
            context.Categories.Add(newCategory);
            context.SaveChanges();
            _categoryService = new(context, _mapper);

            var categoryUpdated = new UpdateCategoryDTO() { NewCategoryName = "Teste 2" };

            // Act
            var result = _categoryService.UpdateCategory(new() { CategoryName = newCategory.Name }, categoryUpdated);
            
            // Assert
            Assert.Equal(categoryUpdated.NewCategoryName, result.Name);
        }

        [Fact]
        public void GetCategoryByName_Throws()
        {
            // Arrange
            _categoryService = new(configContext(), _mapper);

            // Act
            Action act = () => _categoryService.GetCategoryByName("Teste");

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage($"The category with name Teste doesn't exist!");
        }

        [Fact]
        public void GetCategoryWithProduct()
        {
            // Arrange
            var context = configContext();
            Category newCategory = new() { Name = "Teste" };
           
            context.Categories.Add(newCategory);
            context.SaveChanges();

            List<Product> newProducts = new List<Product>()
            {
                new Product() { Category = context.Categories.Find(1), Name = "Content 1" },
                new Product() { Category = context.Categories.Find(1), Name = "Content 2" }
            };
            context.Products.AddRange(newProducts);
            context.SaveChanges();

            _categoryService = new(context, _mapper);

            var expectedResult = new BaseCategoryDTO()
            {
                Name = newCategory.Name,
                Products = new List<BaseProductDTO>
                {
                    new() { Name = newProducts[0].Name },
                    new() { Name = newProducts[1].Name }
                }
            };

            // Act
            var result = _categoryService.GetCategoryWithProducts(new() { Name = newCategory.Name });

            // Assert
            result.Name.Should().Be(newCategory.Name);
            result.Products.Should().HaveCount(2);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetCategoriesWithProducts()
        {
            // Arrange
            var context = configContext();
            List<Category> newCategories = new() 
            { 
                new() { Name = "Teste 1" },
                new() { Name = "Teste 2" }
            };
            context.Categories.AddRange(newCategories);
            context.SaveChanges();

            List<Product> newProducts = new List<Product>()
            {
                new Product() { Category = context.Categories.Find(1), Name = "Content 1" },
                new Product() { Category = context.Categories.Find(2), Name = "Content 2" }
            };
            context.Products.AddRange(newProducts);
            context.SaveChanges();

            _categoryService = new(context, _mapper);

            List<BaseCategoryDTO> expectedCategories = new()
            {
                new()
                {
                    Name = newCategories[0].Name,
                    Products = new List<BaseProductDTO>()
                    {
                        new() { Name = newProducts[0].Name },
                    }
                },
                new()
                {
                    Name = newCategories[1].Name,
                    Products = new List<BaseProductDTO>()
                    {
                        new() { Name = newProducts[1].Name },
                    }
                },
            };

            // Act
            var result = _categoryService.GetCategoriesWithProducts().ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Products).Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedCategories);
            result.Should().AllSatisfy(x =>
            {
                x.Name.Should().NotBeNull();
                x.Products.Should().HaveCount(1);
            });
        }

        [Fact]
        public void GetCategoryByName()
        {
            // Arrange
            var createdCategory = CreateCategory();

            // Act
            var newCategory = _categoryService.GetCategoryByName(createdCategory.Name);

            // Assert
            Assert.Equal(createdCategory.Name, newCategory.Name);
        }

        [Fact]
        public void CheckCategoryExistence_True()
        {
            // Arrange
            var createdCategory = CreateCategory();

            // Act
            var categoryExists = _categoryService.CheckCategoryExistence(createdCategory.Name);

            // Assert
            Assert.True(categoryExists);
        }

        [Fact]
        public void CheckCategoryExistence_False()
        {
            _categoryService = new(configContext(), _mapper);

            // Act
            var categoryExists = _categoryService.CheckCategoryExistence("don't exist");

            // Assert
            Assert.False(categoryExists);
        }

        private AppDbContext configContext()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: "CategoryService_Test_" + Guid.NewGuid()).Options;

            return new AppDbContext(options);
        }
    }
}
