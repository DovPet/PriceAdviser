using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.ScraperService
{
    public class TopoCentras
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "TopoCentras";
        DateTime DateNow = DateTime.Now;
        private Stopwatch sw = new Stopwatch();
        StreamReader reader;
        public TopoCentras(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareTopoCentras(List<string> category,int nuo, int iki)
        { 
            HtmlWeb web = new HtmlWeb();
            

            for (int i = nuo; i < iki; i++)
            {
               int pageNumber = 1;
               var url = category[i]+"?n=80&p="+pageNumber;
               var page = web.Load(url);
               Console.WriteLine(url);    
               await GetDataFromTopoCentras(page);          
                while (page.DocumentNode.SelectNodes("//a[@class='next']") != null)
                {
                    pageNumber++;
                    url = category[i]+"?limit=80&p="+pageNumber;
                    page = web.Load(url);
                    Console.WriteLine(url);

                    await GetDataFromTopoCentras(page);                   
                }
            }
        }

         public async Task GetDataFromTopoCentras(HtmlDocument page)
        { 
            var pricesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'ajax_block_product')]//td[@class='kodas']");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", "").Replace(",", "."));

            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();
            foreach(var set in sets)
            {
            var line = String.Format("{0,-40} {1}", set.Code, set.Price);
            Console.WriteLine(line);
            }
        }

        public async Task GetLinksFromTopoCentras()
        { 
            StreamWriter file = new StreamWriter(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor\Links\TopoCentrasLinks.txt");
            HtmlWeb web = new HtmlWeb();
            var url = "https://www.topocentras.lt";
            var page = web.Load(url);

            var linkNodesInTheShop = page.DocumentNode.SelectNodes("//a[contains(@href,'https://www.topocentras.lt')]");
            var links = linkNodesInTheShop.Select(node => node.Attributes["href"]);
            
            foreach (var link in links)
            {
               if(link.Value.Contains("sveikata")  ||  link.Value.Contains("buitine"))
               {}else{
                await file.WriteLineAsync(link.Value);
                Console.WriteLine(link.Value);
               }
            }
            await file.FlushAsync();
        
        }
    }
}