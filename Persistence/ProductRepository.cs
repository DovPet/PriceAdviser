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
            return await context.Products.Include(v => v.Prices).ThenInclude(e=>e.EShop).Skip(84243).Take(100).ToListAsync();
        }

        public async Task<IEnumerable<Price>> GetPrices()
        {
            return await context.Prices.ToListAsync();
        }

         public async Task<Price> GetPrice(int id)
        {
           return await context.Prices.FindAsync(id);
        }
    }
}