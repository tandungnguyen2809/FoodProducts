using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public partial class FoodProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
