using Microsoft.EntityFrameworkCore;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Persistence
{
    public class PriceAdvisorDbContext : DbContext
    {
         public DbSet<Data> Datas { get; set; }
         public DbSet<EShop> Eshops {get ;set;}
         public DbSet<Administration> Administrations {get ;set;}
         public DbSet<Price> Prices {get ;set;}
         public DbSet<Product> Products {get ;set;}
        public PriceAdvisorDbContext(DbContextOptions<PriceAdvisorDbContext> options) 
          : base(options)
        {
        }
    }
}