﻿namespace ourinv.WebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }
    }
}
