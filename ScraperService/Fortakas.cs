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
    public class Fortakas
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
        public async Task PrepareFortakas(List<string> category,int nuo, int iki)
        {  
            HtmlWeb web = new HtmlWeb();

            for (int i = nuo; i < iki; i++)
            {
               int pageNumber = 1;
               var url = category[i]+"?n=100&p="+pageNumber;
               var page = web.Load(url);
               Console.WriteLine(url);    
               await GetDataFromFortakas(page);          
                while (page.DocumentNode.SelectNodes("(//a[@class='product-next'])[1]") != null)
                {
                  
                    pageNumber++;
                    url = category[i]+"?n=100&p="+pageNumber;
                    page = web.Load(url);
                    Console.WriteLine(url);
                      if(page.DocumentNode.SelectNodes("//div[@class='no_prods_found clear']") == null)
                    {
                        await GetDataFromFortakas(page);
                    }
                   
                }
            }
        }

        public async Task GetDataFromFortakas(HtmlDocument page)
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

        public async Task GetLinksFromFortakas()
        {
            StreamWriter file = new StreamWriter(Environment.CurrentDirectory+@"\Links\FortakasLinks.txt");
            HtmlWeb web = new HtmlWeb();
            var url = "https://fortakas.lt/medis";
            var page = web.Load(url);

            
            var linkNodesInTheShop = page.DocumentNode.SelectNodes("//a[contains(@href,'https://fortakas.lt/')]");
            var links = linkNodesInTheShop.Select(node => node.Attributes["href"]);
            
            foreach (var link in links)
            {
                await file.WriteLineAsync(link.Value);
                Console.WriteLine(link.Value);
            }
            await file.FlushAsync();
        }
    }
}