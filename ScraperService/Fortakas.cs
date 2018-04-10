using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using PriceAdvisor.Core;
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
        public Fortakas(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareFortakas(HtmlDocument doc)
        { 
        }

        public async Task GetDataFromFortakas(HtmlDocument doc)
        { 
        }
        public async Task GetLinksFromFortakas(IWebDriver driver,HtmlDocument doc)
        { 
        }
    }
}