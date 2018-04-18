using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PriceAdvisor.Controllers.Resources;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.Controllers
{
        [Route("/api/administr")]
    public class AdministrationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private readonly IMapper mapper;        
        private readonly IAdministrationRepository repository;
        public AdministrationController(IUnitOfWork unitOfWork,IMapper mapper,IAdministrationRepository repository,PriceAdvisorDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateScrapable(int id, [FromBody] AdministrationResource AdminResource)
    {
     var scapable = await repository.GetScrapable(id);

      if (scapable == null)
        return NotFound();

      mapper.Map<AdministrationResource, Administration>(AdminResource, scapable);
      await unitOfWork.CompleteAsync();

      scapable = await repository.GetScrapable(scapable.Id);
      var result = mapper.Map<Administration, AdministrationResource>(scapable);

      return Ok(result);
    }
    [HttpGet]
    public async Task<IEnumerable<AdministrationResource>> GetScrapables()
    {
        var scrapables = await repository.GetScrapables();

        return mapper.Map<IEnumerable<Administration>,IEnumerable<AdministrationResource>>(scrapables);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetScrapable(int id)
    {
      var scrapable = await repository.GetScrapable(id);

      if (scrapable == null)
        return NotFound();

      var adminResource = mapper.Map<Administration, AdministrationResource>(scrapable);

      return Ok(adminResource);
    }
    }
}