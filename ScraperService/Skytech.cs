using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PriceAdvisor.ScraperService
{
    public class Skytech
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
         
        public async Task SkytechAsync(int nuo, int iki)
        {
           var FindShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
           if(FindShop.Administration.Id == 2)
           {
                Console.WriteLine("Nothing to do eshop {0} is not scrapable",EshopName);
           }else{
            HtmlWeb web = new HtmlWeb();
            //1656
            for (int i = nuo; i < iki; i++)
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
                       await gautiDuomenisIsSkytech(page);
                    }
                    else
                    {
                        for (int j = 1; j <= page.DocumentNode.SelectNodes("//td[@class='pagenav']//div[@class='page']").Count; j++)
                        {
                            uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i + ".html?pagesize=500&page=" + j + "&pav=0";
                            Console.WriteLine(uri);
                            page = web.Load(uri);
                            await gautiDuomenisIsSkytech(page);
                        }
                    }
                }
            }
           }
        }
        public async Task gautiDuomenisIsSkytech(HtmlDocument page)
        {
            var FindEShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            var pricesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='name']//parent::tr//strong");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='model ']//div");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "")/*.Replace("MODELIS:", "")*/);
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", ""));

            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();

            foreach (var set in sets)
            {

                {
                    var Code = new Product { Code = set.Code, Name = ""};
                    var FindProduct = context.Products.FirstOrDefault(product=> product.Code == set.Code);
                    var Price = new Price {Value = set.Price, UpdatedAt = DateNow, EshopId = FindEShop.Id, ProductId = FindProduct.Id};
                    //var productUpdate = await context.Datas.FirstOrDefaultAsync(s => s.Code == set.Code );
                   // productUpdate.Price = set.Price;
                    await unitOfWork.CompleteAsync();
                    var line = String.Format("{0,-40} {1}", set.Code, set.Price);

                    Console.WriteLine(line);
                }
            }
            await unitOfWork.CompleteAsync();
        }


    }
}