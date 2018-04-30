using System.Linq;
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

            CreateMap<Product, ProductResource>()
            .ForMember(vr => vr.Prices, opt => opt.MapFrom(v => v.Prices
            .Select(vf => new PriceResource { Id = vf.Id, Value = vf.Value, UpdatedAt = vf.UpdatedAt,
             EshopId = vf.EshopId, ProductId = vf.ProductId, Percents = vf.EShop.Percents})));

            CreateMap<ProductResource, Product>()
            .ForMember(v => v.Prices, opt => opt.Ignore());

            CreateMap<Price, PriceResource>().ForMember(vr => vr.ProductId, opt => opt.MapFrom(v => v.ProductId))
            .ForMember(vr => vr.Code, opt => opt.MapFrom(v => v.Product.Code)).ForMember(vr => vr.Percents, opt => opt.MapFrom(v => v.EShop.Percents));;
            CreateMap<Price, PriceSaveResource>().ForMember(vr => vr.ProductId, opt => opt.MapFrom(v => v.ProductId))
            .ForMember(vr => vr.Code, opt => opt.MapFrom(v => v.Product.Code));;
            CreateMap<PriceResource, Price>();
            CreateMap<PriceSaveResource, Price>();
        }
        
    }
}