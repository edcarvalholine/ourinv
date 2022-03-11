using Microsoft.EntityFrameworkCore;
using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Database.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Product GetProductByName(string productName)
        {
            var existingProduct = _context.Products.SingleOrDefault(x => x.Name == productName);
            checkExistenceProductException(existingProduct, productName);

            return existingProduct;
        }

        public Product GetProductWithCategoryByName(string productName)
        {
            var existingProduct = _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Name == productName);
            checkExistenceProductException(existingProduct, productName);

            return existingProduct;
        }

        public Product CheckProductExistence(string productName)
        {
            var existingProduct = _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Name == productName);
            if (existingProduct != null)
            {
                throw new Exception($"The product with name {productName} already exists (Inserted in Category: {existingProduct.Category.Name}).");
            }

            return existingProduct;
        }

        public Product CheckProductExistenceInCategory(string categoryName, string productName)
        {
            CheckProductExistence(productName);

            var existingProductInCategory = _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Category.Name == categoryName && x.Name == productName);
            if (existingProductInCategory != null)
            {
                throw new Exception($"The product with name {existingProductInCategory.Name} already exists in {existingProductInCategory.Category.Name}!");
            }

            return existingProductInCategory;
        }

        private void checkExistenceProductException(Product existingProduct, string productName)
        {
            if (existingProduct == null)
            {
                throw new Exception($"The product with name {productName} doesn't exist");
            }
        }
    }
}
