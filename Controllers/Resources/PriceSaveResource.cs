using System;

namespace PriceAdvisor.Controllers.Resources
{
    public class PriceSaveResource
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public bool Edited {get;set;}

    }
}