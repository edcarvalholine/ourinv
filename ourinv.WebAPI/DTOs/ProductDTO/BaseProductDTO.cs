using ourinv.WebAPI.DTOs.CategoryDTO;

namespace ourinv.WebAPI.DTOs.ProductDTO
{
    public class BaseProductDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public BaseCategoryDTO Category { get; set; }
    }
}
