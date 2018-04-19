using System;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Controllers.Resources
{
    public class PriceResource
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int EshopId { get; set; }
    }
}