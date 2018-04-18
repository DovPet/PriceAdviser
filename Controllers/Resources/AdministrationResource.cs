using System.Collections.Generic;
using System.Collections.ObjectModel;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Controllers.Resources
{
    public class AdministrationResource
    {
        public int Id { get; set; }
        public bool Scrapable { get; set; }
        
    }
}