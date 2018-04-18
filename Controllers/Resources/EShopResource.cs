using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Controllers.Resources
{
    public class EShopResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percents { get; set; }
        public int AdministrationId {get; set;}
    }
}