using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class PriceAdvisorDbContext : DbContext
    {
        public PriceAdvisorDbContext(DbContextOptions<PriceAdvisorDbContext> options) 
          : base(options)
        {
        }
        public DbSet<Data> Datas { get; set; }
    }
}