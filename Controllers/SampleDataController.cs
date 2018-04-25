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
        Atea ateaInstance;
        IEshop kilobaitas, skytech, fortakas, topocentras; 
        ChromeOptions option = new ChromeOptions();
        HtmlDocument doc = new HtmlDocument();
        
        public SampleDataController(IUnitOfWork unitOfWork,PriceAdvisorDbContext context )
        {
                this.context = context;
                this.unitOfWork = unitOfWork;
                skytech = new Skytech(unitOfWork,context);
                kilobaitas = new Kilobaitas(unitOfWork,context);
                fortakas = new Fortakas(unitOfWork,context);
                topocentras = new TopoCentras(unitOfWork,context);
                ateaInstance = new Atea(unitOfWork,context);
                for (int i = 0; i < 1000; i++)
            {
//                BackgroundJob.Delete(i.ToString());
            }
        }
        
        [HttpGet("[action]")]
        [Route("all")]
        public async Task WeatherForecasts()
        {
           var NeedSkytech = context.Eshops.FirstOrDefault(shop=> shop.Name == Skytech);
           var NeedKilobaitas = context.Eshops.FirstOrDefault(shop=> shop.Name == Kilobaitas);
           var NeedFortakas = context.Eshops.FirstOrDefault(shop=> shop.Name == Fortakas);
           var NeedTopoCentras = context.Eshops.FirstOrDefault(shop=> shop.Name == TopoCentras);
           //need to set to 1 if you want to get data

           if(NeedSkytech.AdministrationId == 2)
            {
               
                int[] nuoList = new int[10] { 1, 170,340, 510, 680, 850,1020, 1190, 1360, 1530 };
                int[] ikiList = new int[10] { 170, 340,510, 680, 850, 1020, 1190, 1360, 1530, 1656};
             
                for (int i = 0; i < 1; i++)
                {
                     BackgroundJob.Enqueue(() => skytech.PrepareEshop(nuoList[i], ikiList[i]));
                }
                }else{
                    Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Skytech);
            }
           if(NeedKilobaitas.AdministrationId == 2)
            {
                option.AddArgument("--headless");
                option.AddArgument("--start-maximized");         
                option.AddArgument("--disable-gpu"); 
                option.AddArgument("--hide-scrollbars"); 
                option.AddArgument("--no-sandbox");      
                option.AddArgument("--no-startup-window"); 
                option.AddArgument("--disable-extensions"); 
                option.AddArgument("--disable-infobars");
                option.AddArgument("--ignore-certificate-errors");

                List<List<int>> CategoryList = new List<List<int>>();

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

                Parallel.For(0, 1, i =>
            {
                IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory,option);
                HtmlDocument doc = new HtmlDocument();
               
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                Thread pirma = new Thread(async()=>await kilobaitas.PrepareEshop(driver,doc,CategoryList[i]));
                threads[i] = pirma;
                threads[i].Start();
                pirma.Join();
            });
                
            }else{
                Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Kilobaitas);
             }
             if(NeedFortakas.AdministrationId == 2)
             {             
                int[] fromList = new int[10] { 0, 24, 48, 72, 96, 120, 144, 168, 192, 216};
                int[] toList = new int[10] { 1, 48, 72, 96, 120, 144, 168, 192, 216, 234};
                List<string> fortakasCategories = System.IO.File.ReadAllLines(Environment.CurrentDirectory+@"\Links\FortakasLinks.txt").ToList();
                for (int i = 0; i < 1; i++)
                {
                BackgroundJob.Enqueue(() => fortakas.PrepareEshop(fortakasCategories,fromList[i], toList[i]));
                }   
             }else{
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Fortakas);
             }
              
              if(NeedTopoCentras.AdministrationId == 2){
                int[] fromList = new int[] { 0, 4, 8, 12, 16, 20, 24, 28, 32, 36,40,44,48,52,56,60,64,68,72,76,80,84};
                int[] toList = new int[] { 4, 8, 12, 16, 20, 24, 28, 32, 36, 40,44,48,52,56,60,64,68,72,76,80,84,87};
               
                List<string> topoCentrasCategories = System.IO.File.ReadAllLines(Environment.CurrentDirectory+@"\Links\TopoCentrasLinks.txt").ToList();
                for (int i = 0; i < 1; i++)
                {
                BackgroundJob.Enqueue(() => topocentras.PrepareEshop(topoCentrasCategories,fromList[i], toList[i]));
                }
                
             
             }else{
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",TopoCentras);
                }
              //ateaInstance.LoadPricesFromExcel();
        }
        [HttpGet("[action]")]
        [Route("skytech")]
        public async Task GetDataFromSkytech()
        {
            
            var NeedSkytech = context.Eshops.FirstOrDefault(shop=> shop.Name == Skytech);
           if(NeedSkytech.AdministrationId == 2)
            {
                int[] nuoList = new int[10] { 1, 170,340, 510, 680, 850,1020, 1190, 1360, 1530 };
                int[] ikiList = new int[10] { 170, 340,510, 680, 850, 1020, 1190, 1360, 1530, 1656};
             
                for (int i = 0; i < 1; i++)
                {
                     BackgroundJob.Enqueue(() => skytech.PrepareEshop(nuoList[i], ikiList[i]));
                }
                }else{
                    Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Skytech);
                }
        }
        [HttpGet("[action]")]
        [Route("kilobaitas")]
        public async Task GetDataFromKilobaitas()
        {
            var NeedKilobaitas = context.Eshops.FirstOrDefault(shop=> shop.Name == Kilobaitas);
            if(NeedKilobaitas.AdministrationId == 2)
            {
                option.AddArgument("--headless");
                option.AddArgument("--start-maximized");         
                option.AddArgument("--disable-gpu"); 
                option.AddArgument("--hide-scrollbars"); 
                option.AddArgument("--no-sandbox");      
                option.AddArgument("--no-startup-window"); 
                option.AddArgument("--disable-extensions"); 
                option.AddArgument("--disable-infobars");
                option.AddArgument("--ignore-certificate-errors");

                List<List<int>> CategoryList = new List<List<int>>();

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

                Parallel.For(0, 1, i =>
            {
                IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory,option);
                HtmlDocument doc = new HtmlDocument();
               
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                Thread pirma = new Thread(async()=>await kilobaitas.PrepareEshop(driver,doc,CategoryList[i]));
                threads[i] = pirma;
                threads[i].Start();
                pirma.Join();
            });
                
            }else{
                Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Kilobaitas);
             }
           
        }
        [Route("fortakas")]
        [HttpGet("[action]")]
        
        public async Task GetDataFromFortakas()
        {
            var NeedFortakas = context.Eshops.FirstOrDefault(shop=> shop.Name == Fortakas);
            if(NeedFortakas.AdministrationId == 2)
             {        
                int[] fromList = new int[10] { 0, 24, 48, 72, 96, 120, 144, 168, 192, 216};
                int[] toList = new int[10] { 1, 48, 72, 96, 120, 144, 168, 192, 216, 234};
                List<string> fortakasCategories = System.IO.File.ReadAllLines(Environment.CurrentDirectory+@"\Links\FortakasLinks.txt").ToList();
                for (int i = 0; i < 1; i++)
                {
                BackgroundJob.Enqueue(() => fortakas.PrepareEshop(fortakasCategories,fromList[i], toList[i]));
                }   
             }else{
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",Fortakas);
             }           
        }
        [Route("topocentras")]
        [HttpGet("[action]")]
        
        public async Task GetDataFromTopoCentras()
        {
            var NeedTopoCentras = context.Eshops.FirstOrDefault(shop=> shop.Name == TopoCentras);
            if(NeedTopoCentras.AdministrationId == 2){
                int[] fromList = new int[] { 0, 4, 8, 12, 16, 20, 24, 28, 32, 36,40,44,48,52,56,60,64,68,72,76,80,84};
                int[] toList = new int[] { 4, 8, 12, 16, 20, 24, 28, 32, 36, 40,44,48,52,56,60,64,68,72,76,80,84,87};
               
                List<string> topoCentrasCategories = System.IO.File.ReadAllLines(Environment.CurrentDirectory+@"\Links\TopoCentrasLinks.txt").ToList();
                for (int i = 0; i < 1; i++)
                {
               BackgroundJob.Enqueue(() => topocentras.PrepareEshop(topoCentrasCategories,fromList[i], toList[i]));
                }
                
             
             }else{
                 Console.WriteLine("Nothing to do eshop '{0}' is not scrapable",TopoCentras);
                }
        }
    }
}
