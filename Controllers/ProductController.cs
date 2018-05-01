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
   [Route("prices")]
   [HttpGet] 
    public async Task<IEnumerable<PriceResource>> GetPrices()
    {
        var prices = await repository.GetPrices();

        return mapper.Map<IEnumerable<Price>,IEnumerable<PriceResource>>(prices);
    }
     
    [HttpGet("prices/{id}")]
    public async Task<IActionResult> GetPrice(int id)
    {
      var price = await repository.GetPrice(id);

      if (price == null)
        return NotFound();

      var priceSaveResource = mapper.Map<Price,PriceSaveResource>(price);

      return Ok(priceSaveResource);
    }

    [HttpPut("prices/{id}")]
    public async Task<IActionResult> UpdatePrices(int id, [FromBody] PriceSaveResource saveResource)
    {
     var price = await repository.GetPrice(id);

      if (price == null)
        return NotFound();

      mapper.Map<PriceSaveResource, Price>(saveResource, price);
      await unitOfWork.CompleteAsync();

      price = await repository.GetPrice(price.Id);
      var result = mapper.Map<Price, PriceSaveResource>(price);

      return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductResource productResource)
    {
      var product = mapper.Map<ProductResource, Product>(productResource);

      repository.AddProduct(product);
      await unitOfWork.CompleteAsync();

      product = await repository.GetProduct(product.Id);

      var result = mapper.Map<Product,ProductResource>(product);

      return Ok(result);
    }
    [HttpPost("prices")]
    public async Task<IActionResult> CreatePrice([FromBody] PriceSaveResource priceResource)
    {
      var price = mapper.Map<PriceSaveResource, Price>(priceResource);

      repository.AddPrice(price);
      await unitOfWork.CompleteAsync();

      price = await repository.GetPrice(price.Id);

      var result = mapper.Map<Price,PriceSaveResource>(price);

      return Ok(result);
    }
    }
}