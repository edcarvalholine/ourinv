using ourinv.WebAPI.DTOs.CategoryDTO;

namespace ourinv.WebAPI.DTOs.ProductDTO
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public CreateCategoryDTO Category { get; set; }
    }
}
