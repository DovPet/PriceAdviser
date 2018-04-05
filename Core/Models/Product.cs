using System;

namespace PriceAdvisor.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}