using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PriceAdvisor.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [DefaultValue(false)]
        public bool Edited {get;set;}
        public ICollection<Price> Prices { get; set; }
    }
}