using System;
using System.Collections.Generic;

namespace PriceAdvisor.Core.Models
{
    public class EShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percents { get; set; }
        public ICollection<Price> Prices { get; set; }
        public int AdministrationId { get; set; }
        public Administration Administration {get; set;}
    }
}