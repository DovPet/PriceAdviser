using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using PriceAdvisor.Core;
using PriceAdvisor.Persistence;
using System.Data;
using System.Text;
using PriceAdvisor.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
namespace PriceAdvisor.ScraperService
{
    public class Atea
    {
         private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "Atea";
        DateTime DateNow = DateTime.Now;
        private Stopwatch sw = new Stopwatch();
        StreamReader reader;
        public Atea(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
        }

        public async Task LoadProductsFromExcel()
        {
            
           var file = @"C:\Users\Dovydas.Petrutis\Documents\PriceAdvisor\Links\products(1).csv";
           foreach (string line in File.ReadLines(file))
            {
                    var data = line.Split(new[] { ';' });
                    var product = new Product() { Name = data[1], Code = data[0]};       
                    context.Products.Add(product);
                    Console.WriteLine(data[0]+"   "+ data[1]);       
            }
                await unitOfWork.CompleteAsync();
        }
        
        public async Task LoadPricesFromExcel()
        {
            var file = @"C:\Users\Dovydas.Petrutis\Documents\PriceAdvisor\Links\products(1).csv";
            var date = DateNow.AddTicks( - (DateNow.Ticks % TimeSpan.TicksPerSecond));
            foreach (string line in File.ReadLines(file))
            {
                    var data = line.Split(new[] { ';' });
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
                    var valueTo = data[3].Replace(",",".");
                    var price = new Price() { EshopId = 1, Value = data[3], UpdatedAt = date, ProductId = Int32.Parse(data[6]) };
       
                    context.Prices.Add(price);

                    Console.WriteLine("Data "+data[3]);
                    Console.WriteLine("Value "+price.Value);
                    Console.WriteLine("Value "+price.ProductId);
            }
            await unitOfWork.CompleteAsync();
        }
    }
}