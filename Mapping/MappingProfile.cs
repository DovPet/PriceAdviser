using AutoMapper;
using PriceAdvisor.Controllers.Resources;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Administration, AdministrationResource>();
            CreateMap<AdministrationResource, Administration>();

            CreateMap<EShop, EShopResource>();
            CreateMap<EShopResource, EShop>();
        }
        
    }
}