using System.Threading.Tasks;
using PriceAdvisor.Core;

namespace PriceAdvisor.Persistence
{
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PriceAdvisorDbContext context;

        public UnitOfWork(PriceAdvisorDbContext context)
        {
        this.context = context;
        }

        public async Task CompleteAsync()
        {
         await context.SaveChangesAsync();
        }
  
    }
}