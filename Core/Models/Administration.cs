using System;
using System.Collections.Generic;

namespace PriceAdvisor.Core.Models
{
    public class Administration
    {
        public int Id { get; set; }
        public bool Scrapable { get; set; }
        public ICollection<EShop> EShops { get; set; }
    }
}