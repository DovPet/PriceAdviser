using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Controllers.Resources
{
    public class PriceResource
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EshopId { get; set; }
        public int Edited { get; set; }
        public int Percents { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }

    }
}