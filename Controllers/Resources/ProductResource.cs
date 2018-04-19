using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PriceAdvisor.Controllers.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Edited {get;set;}
        public ICollection<PriceResource> Prices { get; set; }

        public ProductResource()
        {
            Prices = new Collection<PriceResource>();
        }
    }
}