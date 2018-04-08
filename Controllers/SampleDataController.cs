using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading; 
using System.Threading.Tasks;
using Hangfire;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PriceAdvisor.ScraperService;
using PriceAdvisor.Core;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.Controllers
{
    
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string Skytech = "Skytech";
        private string Kilobaitas = "Kilobaitas";
        Skytech skytechInstance;
        Kilobaitas kilobaitasInstance;
        static ChromeOptions option = new ChromeOptions();
        static HtmlDocument doc = new HtmlDocument();
        public SampleDataController(IUnitOfWork unitOfWork,PriceAdvisorDbContext context)
        {
                this.context = context;
                this.unitOfWork = unitOfWork;
                skytechInstance = new Skytech(unitOfWork,context);
                kilobaitasInstance = new Kilobaitas(unitOfWork,context);
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
           var NeedSkytech = context.Eshops.FirstOrDefault(shop=> shop.Name == Skytech);
           var NeedKilobaitas = context.Eshops.FirstOrDefault(shop=> shop.Name == Kilobaitas);
           //need to set to 1 if you want to get data
           if(NeedSkytech.AdministrationId == 2)
           {
                Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Skytech);
           }else{
                int[] nuoList = new int[9] { 1, 340, 510, 680, 850,1020, 1190, 1360, 1530 };
                int[] ikiList = new int[9] { 170, 510, 680, 850, 1020, 1190, 1360, 1530, 1656};
             
                for (int i = 0; i < 9; i++)
                {
                    BackgroundJob.Enqueue(() => skytechInstance.PrepareSkytech(nuoList[i], ikiList[i]));
                }
             }
             if(NeedKilobaitas.AdministrationId == 1)
             {
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Kilobaitas);
             }else{
                option.AddArgument("--headless");
                 
                IWebDriver driver = new ChromeDriver(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor",option);
                //IWebDriver driver = new ChromeDriver(@"C:\selenium");
                driver.Manage().Window.Maximize();
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //00:24:47.9380459
                //sw.Start();
                List<List<int>> CategoryList = new List<List<int>>();
                CategoryList.Add( new List<int>{390, 406, 435});
                CategoryList.Add( new List<int>{438, 638, 442});
                CategoryList.Add( new List<int>{557, 476, 482});
                CategoryList.Add( new List<int>{489, 652, 510});
                CategoryList.Add( new List<int>{550, 570, 583});
                //new List<int>() { 390, 406, 435, 438, 638, 442, 557, 476, 482, 489, 652, 510, 550, 570, 583 };
                for (int i = 0; i < CategoryList.Count; i++)
                {
                    //BackgroundJob.Enqueue(() => kilobaitasInstance.PrepareKilobaitas(driver, doc,CategoryList[0]));
                     kilobaitasInstance.PrepareKilobaitas(driver, doc,CategoryList[0]);
                }
                //sw.Stop();
                //Console.WriteLine(sw.Elapsed);
             }
            var rng = new Random();
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
          
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
           
           return new string[] {"value1", "value2"};
        }
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
