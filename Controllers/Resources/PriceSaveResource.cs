using System;

namespace PriceAdvisor.Controllers.Resources
{
    public class PriceSaveResource
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ProductId { get; set; }
        public int EshopId {get; set; }
    }
}