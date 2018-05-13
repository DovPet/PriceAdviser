using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class ProductRepository : IProductRepository
    {
    private readonly PriceAdvisorDbContext context;

    public ProductRepository(PriceAdvisorDbContext context)
    {
        this.context = context;
    }
        public async Task<Product> GetProduct(int id)
        {
           return await context.Products.Include(v => v.Prices).ThenInclude(e=>e.EShop).SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            context.Prices.OrderBy(pr=>pr.EshopId);
            var list = await context.Products.Include(pr=>pr.Prices).ThenInclude(e=>e.EShop).OrderByDescending(p=>p.Id).ToListAsync();
            list.ForEach(m=>m.Prices = m.Prices.OrderBy(o=>o.EshopId).ToList());
            return list;
        }

        public async Task<IEnumerable<Price>> GetPrices()
        {
            return await context.Prices.Include(v => v.Product).ToListAsync();
        }

         public async Task<Price> GetPrice(int id)
        {
           return await context.Prices.Include(pr=> pr.Product).SingleOrDefaultAsync(v => v.Id == id);
        }
         public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }
         public void AddPrice(Price prices)
        {
            var price = prices;
            
            context.Prices.Add(prices);
        }
    }
}