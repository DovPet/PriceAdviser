using System;
using System.Collections.Generic;

namespace PriceAdvisor.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Price> Prices { get; set; }
    }
}