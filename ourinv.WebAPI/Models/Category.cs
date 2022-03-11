namespace ourinv.WebAPI.Models
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}