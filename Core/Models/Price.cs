using System;

namespace PriceAdvisor.Core.Models
{
    public class Price
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EshopId { get; set; }
        public EShop EShop {get;set;}
        public bool Edited {get;set;}
        public int ProductId { get; set; }
        public Product Product {get;set;}
    }
}