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
using System.Diagnostics;
using OpenQA.Selenium.Remote;

namespace PriceAdvisor.Controllers
{
    
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;
        private string Skytech = "Skytech";
        private string Kilobaitas = "Kilobaitas";
        private string Fortakas = "Fortakas";
        private string TopoCentras = "TopoCentras";
        Skytech skytechInstance;
        Kilobaitas kilobaitasInstance;
        Fortakas fortakasInstance;
        TopoCentras topocentrasInstance;
        ChromeOptions option = new ChromeOptions();
        HtmlDocument doc = new HtmlDocument();
        
        public SampleDataController(IUnitOfWork unitOfWork,PriceAdvisorDbContext context)
        {
                this.context = context;
                this.unitOfWork = unitOfWork;
                skytechInstance = new Skytech(unitOfWork,context);
                kilobaitasInstance = new Kilobaitas(unitOfWork,context);
                fortakasInstance = new Fortakas(unitOfWork,context);
                topocentrasInstance = new TopoCentras(unitOfWork,context);
                
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
                            var sw = new Stopwatch();
                
           var NeedSkytech = context.Eshops.FirstOrDefault(shop=> shop.Name == Skytech);
           var NeedKilobaitas = context.Eshops.FirstOrDefault(shop=> shop.Name == Kilobaitas);
           var NeedFortakas = context.Eshops.FirstOrDefault(shop=> shop.Name == Fortakas);
           var NeedTopoCentras = context.Eshops.FirstOrDefault(shop=> shop.Name == TopoCentras);
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
           if(NeedKilobaitas.AdministrationId == 2)
            {
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Kilobaitas);
                }else{
                
                option.AddArgument("--headless");
                option.AddArgument("--start-maximized");
                    
                /*option.AddArgument("--disable-gpu"); 
                option.AddArgument("--hide-scrollbars"); 
                option.AddArgument("--no-sandbox");      
                 option.AddArgument("--no-startup-window"); 
                 option.AddArgument("--disable-extensions"); 
                 option.AddArgument("--disable-infobars");
                 option.AddArgument("--ignore-certificate-errors"); */
                 
                //IWebDriver driver = new ChromeDriver(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor",option);
                
                //00:24:47.9380459
                //sw.Start();
                List<List<int>> CategoryList = new List<List<int>>();

                /*CategoryList.Add( new List<int>{391, 392});
                CategoryList.Add( new List<int>{486, 485});
                CategoryList.Add( new List<int>{401, 626});
                CategoryList.Add( new List<int>{489, 402});
                CategoryList.Add( new List<int>{550, 420});
                CategoryList.Add( new List<int>{393, 394});
                CategoryList.Add( new List<int>{577, 663});
                CategoryList.Add( new List<int>{612, 484});
                CategoryList.Add( new List<int>{483, 600});
                CategoryList.Add( new List<int>{403, 660});
                CategoryList.Add( new List<int>{419, 739});
                CategoryList.Add( new List<int>{435, 407});
                CategoryList.Add( new List<int>{438, 408});
                CategoryList.Add( new List<int>{476, 570});
                CategoryList.Add( new List<int>{442, 409});
                CategoryList.Add( new List<int>{510, 576});
                CategoryList.Add( new List<int>{627, 737});
                CategoryList.Add( new List<int>{638, 487});
                CategoryList.Add( new List<int>{557, 770});
                CategoryList.Add( new List<int>{652, 488});*/

                /*CategoryList.Add( new List<int>{391, 392, 488});
                CategoryList.Add( new List<int>{486, 485, 652});
                CategoryList.Add( new List<int>{401, 626, 770});
                CategoryList.Add( new List<int>{489, 402, 557});
                CategoryList.Add( new List<int>{550, 420, 487});
                CategoryList.Add( new List<int>{393, 394, 638});
                CategoryList.Add( new List<int>{577, 663, 737});
                CategoryList.Add( new List<int>{612, 484, 627});
                CategoryList.Add( new List<int>{483, 600, 576});
                CategoryList.Add( new List<int>{403, 660, 510});
                CategoryList.Add( new List<int>{419, 739, 409});
                CategoryList.Add( new List<int>{435, 407, 442});
                CategoryList.Add( new List<int>{438, 408});
                CategoryList.Add( new List<int>{476, 570});*/

                CategoryList.Add( new List<int>{391, 392, 488, 468});
                CategoryList.Add( new List<int>{486, 485, 652, 463});
                CategoryList.Add( new List<int>{401, 626, 770, 465});
                CategoryList.Add( new List<int>{489, 402, 557, 534});
                CategoryList.Add( new List<int>{550, 420, 487, 535});
                CategoryList.Add( new List<int>{393, 394, 638, 536});
                CategoryList.Add( new List<int>{577, 663, 737, 537});
                CategoryList.Add( new List<int>{612, 484, 627, 461});
                CategoryList.Add( new List<int>{483, 600, 576, 543});
                CategoryList.Add( new List<int>{403, 660, 510, 458});
                CategoryList.Add( new List<int>{419, 739, 409, 459});
                CategoryList.Add( new List<int>{435, 407, 628, 769});
                CategoryList.Add( new List<int>{438, 408, 447, 470});
                CategoryList.Add( new List<int>{476, 570, 448});
                CategoryList.Add( new List<int>{449, 634, 729});
                CategoryList.Add( new List<int>{469, 617, 460});
                //15:38.0778014
                //List<int> kategorijuID = new List<int>() { 391,392 ,486 ,485 ,557,401,489,402,550,520,393,394,577,663,612,484,483,600,403,660,419,739,
               // 406,435,438,638,476,570,442,562,510 };
                Thread[] threads = new Thread[CategoryList.Count];

                 //IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());
                 sw.Start();
                Parallel.For(0, CategoryList.Count, i =>
            {
                //IWebDriver driver = new ChromeDriver(@"C:\selenium",option);
                IWebDriver driver = new ChromeDriver(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor",option);
                HtmlDocument doc = new HtmlDocument();
               
                //IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
                
               // driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                Thread pirma = new Thread(async() =>await kilobaitasInstance.PrepareKilobaitas(driver, doc,CategoryList[i]));
                threads[i] = pirma;
                threads[i].Start();
                pirma.Join();
                //Thread.Sleep(200);
            });
                
                //for (int i = 0; i < CategoryList.Count; i++)
               // {
                   // BackgroundJob.Enqueue(() => kilobaitasInstance.PrepareKilobaitas(driver, doc,CategoryList[0]));
                //     kilobaitasInstance.PrepareKilobaitas(driver, doc,CategoryList[i]);
              //  }
                //sw.Stop();
                //Console.WriteLine(sw.Elapsed);
             }
             if(NeedFortakas.AdministrationId == 2){
                Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Fortakas);
             }else{                 
                int[] nuoList = new int[10] { 0, 24, 48, 72, 96, 120, 144, 168, 192, 216};
                int[] ikiList = new int[10] { 24, 48, 72, 96, 120, 144, 168, 192, 216, 234};
                List<string> fortakasCategories = System.IO.File.ReadAllLines(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor\Links\FortakasLinks.txt").ToList();
                //fortakasInstance.PrepareFortakas(categories,0,categories.Count);
                for (int i = 0; i < nuoList.Count(); i++)
                {
                BackgroundJob.Enqueue(() => fortakasInstance.PrepareFortakas(fortakasCategories,nuoList[i], ikiList[i]));
                }
             }
                List<string> topoCentrasCategories = System.IO.File.ReadAllLines(@"F:\Duomenys\Bakalauro darbas\PriceAdvisor\Links\TopoCentrasLinks.txt").ToList();
                topocentrasInstance.PrepareTopoCentras(topoCentrasCategories,0,5);

             sw.Stop();
             Console.WriteLine("===============TIIIIIIIIME HERE=========="+sw.Elapsed);
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
