using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class PriceAdvisorDbContext : DbContext
    {
         public DbSet<Data> Datas { get; set; }
        public PriceAdvisorDbContext(DbContextOptions<PriceAdvisorDbContext> options) 
          : base(options)
        {
        }
       
    }
}