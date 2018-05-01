using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceAdvisor.Controllers.Resources;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.Controllers
{
     [Route("/api/eshop")]
    public class EshopController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private readonly IMapper mapper;        
        private readonly IEshopRepository repository;
        public EshopController(IUnitOfWork unitOfWork,IMapper mapper,IEshopRepository repository,PriceAdvisorDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
        }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> CreateEshop(int id, [FromBody] EShopResource EshopResource)
    {
     var eshop = await repository.GetEshop(id);

      if (eshop == null)
        return NotFound();

      mapper.Map<EShopResource, EShop>(EshopResource, eshop);
      await unitOfWork.CompleteAsync();

      eshop = await repository.GetEshop(eshop.Id);
      var result = mapper.Map<EShop, EShopResource>(eshop);

      return Ok(result);
    }
    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<EShopResource>> GetEshops()
    {
        var eshops = await repository.GetEshops();

        return mapper.Map<IEnumerable<EShop>,IEnumerable<EShopResource>>(eshops);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetEshop(int id)
    {
      var eshop = await repository.GetEshop(id);

      if (eshop == null)
        return NotFound();

      var eshopResource = mapper.Map<EShop, EShopResource>(eshop);

      return Ok(eshopResource);
    }
    }
}