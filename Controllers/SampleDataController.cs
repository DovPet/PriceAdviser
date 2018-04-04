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
 
namespace PriceAdvisor.Controllers
{
    
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
           
            int[] nuoList = new int[9] { 1, 100, 200, 300, 400,500, 600, 700, 800 };
            int[] ikiList = new int[9] { 100, 200, 300, 400, 500, 600, 700, 800, 900};
            
            for (int i = 0; i < 9; i++)
            {
            BackgroundJob.Enqueue(() =>Testing(nuoList[i], ikiList[i]));
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
