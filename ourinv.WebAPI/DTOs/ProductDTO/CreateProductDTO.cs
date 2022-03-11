namespace ourinv.WebAPI.DTOs.ProductDTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public CategoryDTO? Category { get; set; }
    }

    public class CategoryDTO
    {
        public string? Name { get; set; }
    }
}
