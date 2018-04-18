using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class AdministrationRepository : IAdministrationRepository
    {
   
    private readonly PriceAdvisorDbContext context;

    public AdministrationRepository(PriceAdvisorDbContext context)
    {
        this.context = context;
    }

        public void Add(Administration administration)
        {
            context.Administrations.Add(administration);
        }

        public async Task<Administration> GetScrapable(int id)
        {
          return await context.Administrations.FindAsync(id);
        }

        public async Task<IEnumerable<Administration>> GetScrapables()
        {
            return await context.Administrations.ToListAsync();
        }

    }
}