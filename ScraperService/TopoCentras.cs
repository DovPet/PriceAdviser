using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.ScraperService
{
    public class TopoCentras : IEshop
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "TopoCentras";
        DateTime DateNow = DateTime.Now;
        private Stopwatch sw = new Stopwatch();
        List<string> inTheList = new List<string>();
        List<string> exclude = System.IO.File.ReadAllLines(Environment.CurrentDirectory+@"\Links\TopoCentrasExclude.txt").ToList();
        public TopoCentras(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareEshop(List<string> category,int nuo, int iki)
        { 
            HtmlWeb web = new HtmlWeb();
            inTheList.Clear();
            for (int i = nuo; i < iki; i++)
            {
               int pageNumber = 1;
               var url = category[i]+"?limit=80&p="+pageNumber;
               var page = web.Load(url);
               Console.WriteLine(url);   
               await GetDataFromEshop(null,page);  

               pageNumber++;
               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               var containedItemNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
               var containedItem = containedItemNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;","")).ToList()[0].ToString();       
               
                while (!inTheList.Contains(containedItem))
                {

               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               Console.WriteLine(url);

                    await GetDataFromEshop(null,page);
                    pageNumber++;
               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               containedItemNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
               containedItem = containedItemNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;","")).ToList()[0].ToString();       
                                  
                }
                 inTheList.Clear();
            }  
        }

         public async Task GetDataFromEshop(IWebDriver driver, HtmlDocument page)
        { 
            var FindEShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            var pricesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[contains(@class,'price-wrapper')]");
            var pricesNodes2 = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[contains(@class,'price-wrapper')]");
            var codesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
            
            List<string> prices = new List<string>();
            var codes = codesNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;",""));
            var pricesHtml = pricesNodes.Select(node => node.InnerHtml);
            
            if(page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper leasing final-price']") != null){
                pricesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper leasing final-price']//span[@class='final-price']");
            }
            if(page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper']")!=null){
                pricesNodes2 = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper']//span[@class='price']");  
            }
            int priceID = 0;
            int priceID2 = 0;
            foreach (var item in pricesHtml)
            {
                if (item.Contains("/m"))
                {
                    
                   prices.Add(pricesNodes.Select(node => node.InnerText.Replace(" ", "").Replace("\n", "").Replace("Kaina:", "").Replace("€","").Replace(".", "").Replace(",",".")).ToList()[priceID++].ToString());
                }else{
                    
                    prices.Add(pricesNodes2.Select(node => node.InnerText.Replace(" ", "").Replace("\n", "").Replace("€","").Replace(".", "").Replace(",",".")).ToList()[priceID2++].ToString());
                }
            }
            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();
            foreach(var set in sets)
            {
            inTheList.Add(set.Code);
            var Coding = ProductRecognitionTopoCentras(set.Code);
            
            var FindProduct = await context.Products.FirstOrDefaultAsync(product=> product.Code == Coding);
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
                    var line = String.Format("{0,-40} {1}", Coding, set.Price);
                    Console.WriteLine(line); 
            }
            await unitOfWork.CompleteAsync();
            }

        }
        public string ProductRecognitionTopoCentras(string product)
        { 
           int index = 0;
           foreach (Char item in product)
           {
               if(Char.IsUpper(item) && index!=0) break;
               index++;
           }
            product = product.Remove(0,index);
            var parts = product.Split(new[] { ' ','-','/','+' });
            List<Product> searchingProd = new List<Product>();
            var queryName = $"SELECT * FROM [PriceAdvisor].[dbo].[Products] WHERE [Name] LIKE '%"+parts[0]+"%' AND [Name] LIKE '%"+parts[parts.Count()-1].Replace("Dos","")+"%' ";
            
            for (int i = parts.Count()-2; i > 0; i--)
            {
                
                if(!exclude.Contains(parts[i].ToLower()))
                {
                queryName = queryName+"AND [Name] LIKE '%"+parts[i].Replace("GB","").Replace("Ti","")
                                        .Replace("TB","").Replace("GTX","").Replace("+","").Replace("SSD","")+"%' ";
                
                }
            }
            if(parts.Count()<4){
            for (int i = 1; i < parts.Count(); i++)
            {
               var queryCode = $"SELECT * FROM [PriceAdvisor].[dbo].[Products] WHERE [Code] LIKE '%"+parts[i]+"%'";
               searchingProd = context.Products.FromSql(queryCode).ToList(); 
               if(searchingProd.Count()==1) 
                    break;
            }
            }
            if(searchingProd.Count()==1)
            {}else{
            searchingProd = context.Products.FromSql(queryName).ToList();
                }//if(searchingProd.Count()==1)
                //break;
            if(searchingProd.Count()>0)
            {
            Console.WriteLine(searchingProd[0].Code);
            return searchingProd[0].Code;        
            }
         return null;            
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