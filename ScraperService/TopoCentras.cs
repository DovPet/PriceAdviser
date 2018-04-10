using System;
using System.Diagnostics;
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

        public async Task GetLinksFromFortakas(IWebDriver driver, HtmlDocument doc)
        { 
        }
    }
}