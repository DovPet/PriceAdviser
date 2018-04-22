using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.ScraperService
{
    public class Fortakas : IEshop
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "Fortakas";
        DateTime DateNow = DateTime.Now;
        private Stopwatch sw = new Stopwatch();
        StreamReader reader;
        public Fortakas(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
        }
        public async Task PrepareEshop(List<string> category, int from, int to)
        {  
            HtmlWeb web = new HtmlWeb();

            for (int i = from; i < to; i++)
            {
               int pageNumber = 1;
               var url = category[i]+"?n=100&p="+pageNumber;
               var page = web.Load(url);
               Console.WriteLine(url);    
               await GetDataFromEshop(null,page);          
                while (page.DocumentNode.SelectNodes("(//a[@class='product-next'])[1]") != null)
                {
                  
                    pageNumber++;
                    url = category[i]+"?n=100&p="+pageNumber;
                    page = web.Load(url);
                    Console.WriteLine(url);
                      if(page.DocumentNode.SelectNodes("//div[@class='no_prods_found clear']") == null)
                    {
                        await GetDataFromEshop(null,page);
                    }
                   
                }
            }
        }

        public async Task GetDataFromEshop(IWebDriver driver, HtmlDocument page)
        { 
            var FindEShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            var pricesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'ajax_block_product')]//td[@class='ekaina']//strong");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'ajax_block_product')]//td[@class='kodas']");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", "").Replace(",", "."));

            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();
            foreach(var set in sets)
            {
                    var FindProduct = await context.Products.FirstOrDefaultAsync(product=> product.Code == set.Code);
                    if(FindProduct==null)
                    {

                    }else{
                        var FindPriceExists = await context.Prices.FirstOrDefaultAsync(price=> price.ProductId == FindProduct.Id && price.EshopId == FindEShop.Id );
                        if(FindPriceExists != null && FindPriceExists.EshopId==FindEShop.Id)
                        {
                            FindPriceExists.Value = set.Price;
                            FindPriceExists.UpdatedAt = DateNow.AddTicks( - (DateNow.Ticks % TimeSpan.TicksPerSecond));
                        }else{
                            var Price = new Price {Value = set.Price, UpdatedAt = DateNow.AddTicks( - (DateNow.Ticks % TimeSpan.TicksPerSecond)), EshopId = FindEShop.Id, ProductId = FindProduct.Id};

                            context.Prices.Add(Price);
                        }
                    var line = String.Format("{0,-40} {1}", set.Code, set.Price);
                    Console.WriteLine(line); 
            }
            await unitOfWork.CompleteAsync();
            }
        }

        public Task PrepareEshop(int from, int to)
        {
            throw new NotImplementedException();
        }

        public Task PrepareEshop(IWebDriver driver, HtmlDocument page, List<int> categoryID)
        {
            throw new NotImplementedException();
        }
    }
}