using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.ScraperService
{
    public class Skytech
    {
         public static void SkytechAsync(int nuo, int iki)
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
                        gautiDuomenisIsSkytech(page);
                    }
                    else
                    {
                        for (int j = 1; j <= page.DocumentNode.SelectNodes("//td[@class='pagenav']//div[@class='page']").Count; j++)
                        {
                            uri = "http://www.skytech.lt/bevielio-rysio-antenos-priedai-antenos-c-" + i + ".html?pagesize=500&page=" + j + "&pav=0";
                            Console.WriteLine(uri);
                            page = web.Load(uri);
                            gautiDuomenisIsSkytech(page);
                        }
                    }
                }

            }

        }
        public static async void gautiDuomenisIsSkytech(HtmlDocument page)
        {

            var pricesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='name']//parent::tr//strong");
            var codesNodes = page.DocumentNode.SelectNodes("//tr[contains(@class,'productListing')]//td[@class='model ']//div");

            var codes = codesNodes.Select(node => node.FirstChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "")/*.Replace("MODELIS:", "")*/);
            var prices = pricesNodes.Select(node => node.InnerText.Replace("€", "").Replace(" ", ""));

            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();
            foreach (var set in sets)
            {

                {
                    var line = String.Format("{0,-40} {1}", set.Code, set.Price);

                    Console.WriteLine(line);
                }
            }
        }


    }
}