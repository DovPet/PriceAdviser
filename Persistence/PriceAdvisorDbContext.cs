using Microsoft.EntityFrameworkCore;
//using PriceAdvisor.Models;

namespace PriceAdvisor.Persistence
{
    public class PriceAdvisorDbContext : DbContext
    {
        public PriceAdvisorDbContext(DbContextOptions<PriceAdvisorDbContext> options) 
          : base(options)
        {
        }
       // public DbSet<Make> Makes { get; set; }
    }
}