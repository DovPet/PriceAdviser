using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class ExportRepository : IExportRepository
    {
    private readonly PriceAdvisorDbContext context;

    public ExportRepository(PriceAdvisorDbContext context)
    {
        this.context = context;
    }
        public async Task<IEnumerable<Price>> GetPricesAsync()
        {
           return await context.Prices.Where(p=>p.Edited==true).Include(pr=> pr.Product).ToListAsync();
        }

        
    }
}