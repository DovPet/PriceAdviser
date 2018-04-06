using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading; 
using System.Threading.Tasks;
using Hangfire;
using HtmlAgilityPack;
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
        private static readonly IUnitOfWork unitOfWork;
        private static readonly PriceAdvisorDbContext context;

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        int[] nuoList = new int[9] { 1, 340, 510, 680, 850,1020, 1190, 1360, 1530 };
        int[] ikiList = new int[9] { 170, 510, 680, 850, 1020, 1190, 1360, 1530, 1656};
        Skytech inst = new Skytech(unitOfWork,context);
        [HttpPost("[action]")]
        public void WriteData()
        {
             //for (int i = 0; i < 9; i++)
            //{
           
           // }
        }
        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
           
             int[] nuoList = new int[9] { 1, 340, 510, 680, 850,1020, 1190, 1360, 1530 };
             int[] ikiList = new int[9] { 170, 510, 680, 850, 1020, 1190, 1360, 1530, 1656};
            
            for (int i = 0; i < 9; i++)
            {
            //BackgroundJob.Enqueue(() =>Testing(nuoList[i], ikiList[i]));
            BackgroundJob.Enqueue(() => inst.SkytechAsync(nuoList[i], ikiList[i]));
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
        public static void Testing(int nuo, int iki)
        {
             for (int i = nuo; i < iki; i++){                       
                    Console.WriteLine(i);
             }
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
