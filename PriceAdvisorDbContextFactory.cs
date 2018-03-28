using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PriceAdvisor.Persistence;
public class PriceAdvisorContextFactory : IDesignTimeDbContextFactory<PriceAdvisorDbContext>
{
  //////// 
     public PriceAdvisorDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<PriceAdvisorDbContext>();
        var connectionString = configuration.GetConnectionString("Default");
        builder.UseSqlServer(connectionString);
        return new PriceAdvisorDbContext(builder.Options);
    }

}