using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace PriceAdvisor.ScraperService
{
    public class Skytech : IEshop
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "Skytech";
        DateTime DateNow = DateTime.Now;
        
        public Skytech(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
        }
         
        public async Task PrepareEshop(int from, int to)
        {
                      
            HtmlWeb web = new HtmlWeb();
            //1656
            for (int i = from; i < to; i++)
            {
                var uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i +
                                  ".html?pagesize=500&pav=0";
                Console.WriteLine(uri);
                var page = web.Load(uri);

                if (page.DocumentNode.SelectNodes("//td[contains(text(),'Šioje grupėje')]") != null)
                {
                }
                else
                {
                    if (page.DocumentNode.SelectNodes("//td[@class='pagenav']//div[@class='page']") == null)
                    {
                       await GetDataFromEshop(null,page);
                    }
                    else
                    {
                        var pages = page.DocumentNode.SelectSingleNode("(//td[@class='pagenav']//div[@class='page'])[last()]");
                        var pageCnt = pages.InnerText;
                        for (int j = 1; j <= Int32.Parse(pageCnt); j++)
                        {
                            uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i + ".html?pagesize=500&page=" + j + "&pav=0";
                            Console.WriteLine(uri);
                            page = web.Load(uri);
                            await GetDataFromEshop(null,page);
                        }
                    }
                }
            
           }
        }
        public async Task GetDataFromEshop(IWebDriver driver, HtmlDocument page)
        {
            var FindEShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            var pricesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='name']//parent::tr//td[5]");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='model ']//div");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "")/*.Replace("MODELIS:", "")*/);
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", "").Replace("\r", "").Replace("\n", ""));

            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();

            foreach (var set in sets)
            {
                    var FindProduct = await context.Products.FirstOrDefaultAsync(product=> product.Code == set.Code);
                    if(FindProduct==null)
                    {

                    }else{
                        var FindPriceExists = await context.Prices.FirstOrDefaultAsync(price=> price.ProductId == FindProduct.Id && price.EshopId == FindEShop.Id);
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

        public Task PrepareEshop(List<string> category, int from, int to)
        {
            throw new NotImplementedException();
        }


        public Task PrepareEshop(IWebDriver driver, HtmlDocument page, List<int> categoryID)
        {
            throw new NotImplementedException();
        }

    }
}