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
    [Route("/api/products")]
    public class ProductController :Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private readonly IMapper mapper;        
        private readonly IProductRepository repository;
        public ProductController(IUnitOfWork unitOfWork,IMapper mapper,IProductRepository repository,PriceAdvisorDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
        }

     [HttpGet]
    public async Task<IEnumerable<ProductResource>> GetProducts()
    {
        var products = await repository.GetProducts();

        return mapper.Map<IEnumerable<Product>,IEnumerable<ProductResource>>(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
      var product = await repository.GetProduct(id);

      if (product == null)
        return NotFound();

      var productResource = mapper.Map<Product,ProductResource>(product);

      return Ok(productResource);
    }
   
    [HttpGet]
    [Route("prices")]
    public async Task<IEnumerable<PriceResource>> GetPrices()
    {
        var prices = await repository.GetPrices();

        return mapper.Map<IEnumerable<Price>,IEnumerable<PriceResource>>(prices);
    }

    }
}