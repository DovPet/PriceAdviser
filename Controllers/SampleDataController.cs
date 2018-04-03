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
        Thread[] threads = new Thread[10];
        public void RunInBackground(){
            Console.WriteLine($"Runing in background:{Thread.CurrentThread.Name}");
        }
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            /* Parallel.For(0, 9, i =>
            {
                
                Thread pirma = new Thread(() => RunInBackground());
                threads[i] = pirma;
                
                //pirma.Join();
                 BackgroundJob.Enqueue(() =>threads[i].Start() );
                 pirma.Join();
                Thread.Sleep(1000);
            });*/
        StreamWriter skytech = new StreamWriter(@"C:\Users\Dovydas.Petrutis\source\repos\ConsoleApp1\ConsoleApp1\SkytechWEB.txt");
            int[] nuoList = new int[9] { 1, 340, 510, 680, 850,1020, 1190, 1360, 1530 };
            int[] ikiList = new int[9] { 170, 510, 680, 850, 1020, 1190, 1360, 1530, 1656};
            
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
            for (int j = 1; j <= 2000; j++)
                        {
                           
                            Console.WriteLine(j+" "+ i);
                            //file.WriteLine(j+" "+ i);
                        }
        }
        }
        public static void SkytechAsync(StreamWriter file, int nuo, int iki)
        {
            HtmlWeb web = new HtmlWeb();
            //1656
            for (int i = nuo; i < iki; i++)
            {
                var uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i +
                                  ".html?pagesize=500&pav=0";
                Console.WriteLine(uri);
                var page = web.Load(uri);

                if (page.DocumentNode.SelectNodes("//td[contains(text(),'Šioje grupėje')]") != null)
                {
                }
                else
                {
                    if (page.DocumentNode.SelectNodes("//td[@class='pagenav']//div[@class='page']") == null)
                    {
                        gautiDuomenisIsSkytech(page, file);
                    }
                    else
                    {
                        for (int j = 1; j <= page.DocumentNode.SelectNodes("//td[@class='pagenav']//div[@class='page']").Count; j++)
                        {
                            uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i + ".html?pagesize=500&page=" + j + "&pav=0";
                            Console.WriteLine(uri);
                            page = web.Load(uri);
                            gautiDuomenisIsSkytech(page, file);
                        }
                    }
                }

            }

        }
        public static async void gautiDuomenisIsSkytech(HtmlDocument page, StreamWriter file)
        {

            var pricesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='name']//parent::tr//strong");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='model ']//div");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "")/*.Replace("MODELIS:", "")*/);
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", ""));

            List<CodeAndPrice> sets = codes
                .Zip(prices, (code, price) => new CodeAndPrice() { Code = code, Price = price }).ToList();
            foreach (var set in sets)
            {

                {
                    var line = String.Format("{0,-40} {1}", set.Code, set.Price);

                    await file.WriteLineAsync(line);
                    Console.WriteLine(line);
                }
            }
            await file.FlushAsync();
        }


        class CodeAndPrice
        {
            public string Code { get; set; }
            public string Price { get; set; }
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
