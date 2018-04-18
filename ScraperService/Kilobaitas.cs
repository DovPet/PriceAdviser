using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class Kilobaitas
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string EshopName = "Kilobaitas";
        DateTime DateNow = DateTime.Now;
        private Stopwatch sw = new Stopwatch();
        
        public Kilobaitas(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareKilobaitas(IWebDriver driver, HtmlDocument doc,List<int> kategorijuID)
        {
            //390-781
            //await Task.Delay(500);
            for (int kategorijosNr = 0; kategorijosNr < kategorijuID.Count; kategorijosNr++)
           {
                driver.Navigate()
                    .GoToUrl(
                        "http://www.kilobaitas.lt/Kompiuteriai/Plansetiniai_(Tablet)/CatalogStore.aspx?CatID=PL_" +
                        kategorijuID[kategorijosNr] + "");
                        Console.WriteLine("http://www.kilobaitas.lt/Kompiuteriai/Plansetiniai_(Tablet)/CatalogStore.aspx?CatID=PL_" +
                        kategorijuID[kategorijosNr]);
                if (driver.FindElements(By.XPath("//img[@src='/Images/design/notify_information.gif']")).Count > 0 ||
                        driver.FindElements(By.XPath("//td[@class='hdMain']//a[contains(text(),'Buitinė technika')]")).Count > 0)
                {
                    //do nothing () got to another category
                }
                else
                {
                   
                    driver.FindElement(By.XPath("//select[@class='simpleSelect']//option[@value='90']")).Click();

                    while (driver.FindElements(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]"))
                               .Count > 0)
                    {

                        await gautiDuomenisIsKilobaito(driver, doc);
                    }
                    await gautiDuomenisIsKilobaito(driver, doc);
                }
                Console.WriteLine("=========Me Doneeeeeeee=========="+kategorijuID[kategorijosNr]+"===========================");
            }
            Console.WriteLine(sw.Elapsed);
            driver.Close();
        }

        public async Task gautiDuomenisIsKilobaito(IWebDriver driver, HtmlDocument doc)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PriceAdvisorDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PriceAdvisor;Trusted_Connection=True;MultipleActiveResultSets=True;");
            
using(var db = new PriceAdvisorDbContext(optionsBuilder.Options))
{
            var FindEShop = db.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            int psl = 1;
            IWebElement ie;
            IWebElement next;
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            ie = driver.FindElement(By.XPath("//html"));
            string innerHtml = ie.GetAttribute("innerHTML");
            doc.LoadHtml(innerHtml);
            
            var codesNodes =
                doc.DocumentNode.SelectNodes(
                    "//td[@class='mainContent']//div[@class='itemNormal']//div[@class='itemCode']");
            var pricesNodes = doc.DocumentNode.SelectNodes(
                "//td[@class='mainContent']//div[@class='itemNormal']//div[@class='itemCode']//parent::div//div[@class='itemBoxPrice']//div[2]");
            var datosNodes = doc.DocumentNode.SelectNodes("//span[@class='DeliveryDate']");
            if (datosNodes != null)
            {
                foreach (HtmlNode node in datosNodes)
                {
                    node.Remove();
                }
            }
            
            var codes = codesNodes.Select(node =>
                node.InnerText.Replace("kodas", "").Replace(" ", "").Replace(":&nbsp;", "").Replace("\n", "")
                    .Replace("\t", "").Replace("\r", ""));
            var prices = pricesNodes.Select(node =>
                node.InnerText.Replace(" ", "").Replace("&nbsp;€", "").Replace("\n", "").Replace("\t", "")
                    .Replace("\r", "").Replace(",", "."));

            List<Data> sets =
                codes.Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();

            foreach (var set in sets)
            {
            var FindProduct = await db.Products.FirstOrDefaultAsync(product=> product.Code == set.Code);
                    if(FindProduct==null)
                    {

                    }else{
                        var FindPriceExists = await db.Prices.FirstOrDefaultAsync(price=> price.ProductId == FindProduct.Id && price.EshopId == FindEShop.Id);
                        if(FindPriceExists != null && FindPriceExists.EshopId==FindEShop.Id)
                        {
                            FindPriceExists.Value = set.Price;
                            FindPriceExists.UpdatedAt = DateNow.AddTicks( - (DateNow.Ticks % TimeSpan.TicksPerSecond));
                        }else{
                            var Price = new Price {Value = set.Price, UpdatedAt = DateNow, EshopId = FindEShop.Id, ProductId = FindProduct.Id};
                            db.Prices.Add(Price);
                        }                  
                     
            }
            var line = String.Format("{0,-40} {1}", set.Code, set.Price);
            Console.WriteLine(line);
        }
         if (driver.FindElements(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]")).Count != 0)
            {
                next = driver.FindElement(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]"));
                js.ExecuteScript("arguments[0].click();", next);
                ie = driver.FindElement(By.XPath("//html"));
                psl++;
                 Console.WriteLine("========================="+psl+"===========================");
            }
            await db.SaveChangesAsync();
        
      }
    }
    }
}