namespace ourinv.WebAPI.Models
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
    }
}