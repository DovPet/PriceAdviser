using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using PriceAdvisor.Core;
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
        public TopoCentras(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareTopoCentras(HtmlDocument doc)
        { 

        }

         public async Task GetDataFromTopoCentras(HtmlDocument doc)
        { 

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