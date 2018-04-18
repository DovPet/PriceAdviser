using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class EshopRepository : IEshopRepository
    {
    private readonly PriceAdvisorDbContext context;

    public EshopRepository(PriceAdvisorDbContext context)
    {
        this.context = context;
    }
        public void Add(EShop eshop)
        {
            context.Eshops.Add(eshop);
        }

        public async Task<EShop> GetEshop(int id)
        {
            return await context.Eshops.FindAsync(id);
        }

        public async Task<IEnumerable<EShop>> GetEshops()
        {
           return await context.Eshops.ToListAsync();
        }
    }
}