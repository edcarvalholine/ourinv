using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Database;
using ourinv.WebAPI.Database.Repository;
using ourinv.WebAPI.DTOs.ProductDTO;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Services
{
    public class ProductService
    {
        private readonly CategoryService _categoryService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        public ProductService(CategoryService categoryService, AppDbContext context, IMapper mapper, ProductRepository productRepository)
        {
            _categoryService = categoryService;
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public BaseProductDTO CreateProduct(CreateProductDTO productDTO)
        {
            var newProduct = _mapper.Map<Product>(productDTO);

            newProduct.Category = _categoryService.GetCategoryByName(newProduct.Category?.Name == null ? "Unknown" : newProduct.Category.Name);

            _productRepository.CheckProductExistenceInCategory(newProduct.Category.Name, productDTO.Name);

            _productRepository.Create(newProduct);
            _context.SaveChanges();

            return _mapper.Map<BaseProductDTO>(newProduct);
        }

        public BaseProductDTO GetProduct(string productName)
        {
            return _mapper.Map<BaseProductDTO>(_productRepository.GetProductWithCategoryByName(productName));
        }

        public BaseProductDTO DeleteProduct(string productName)
        {
            var productToBeDeleted = _productRepository.GetProductByName(productName);
            _context.Products.Remove(productToBeDeleted);
            _context.SaveChanges();

            return _mapper.Map<BaseProductDTO>(productToBeDeleted);
        }

        public BaseProductDTO UpdateProduct(string productName, UpdateProductDTO productDTO)
        {
            var productToBeUpdated = _productRepository.GetProductWithCategoryByName(productName);
            if (productToBeUpdated.Category.Name != productDTO.Category.CategoryName)
            {
                productToBeUpdated.Category = _categoryService.GetCategoryByName(productDTO.Category.CategoryName);
            }

            // TODO
            _productRepository.CheckProductExistenceInCategory(productToBeUpdated.Category.Name, productDTO.Name);

            _mapper.Map(productDTO, productToBeUpdated);

            _productRepository.Update(productToBeUpdated);
            _context.SaveChanges();

            return _mapper.Map<BaseProductDTO>(productToBeUpdated);
        }
    }
}
