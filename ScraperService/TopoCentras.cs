using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using PriceAdvisor.Core;
using PriceAdvisor.Core.Models;
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
        List<string> inTheList = new List<string>();
        StreamReader reader;
        public TopoCentras(IUnitOfWork unitOfWork, PriceAdvisorDbContext context)
        {          
            this.unitOfWork = unitOfWork;
            this.context = context;
            sw.Start();
        }
        public async Task PrepareTopoCentras(List<string> category,int nuo, int iki)
        { 
            HtmlWeb web = new HtmlWeb();
            inTheList.Clear();
            //StreamWriter file = new StreamWriter(Environment.CurrentDirectory+@"\Links\html.txt");
            for (int i = nuo; i < iki; i++)
            {
               int pageNumber = 1;
               var url = category[i]+"?limit=80&p="+pageNumber;
               var page = web.Load(url);
               Console.WriteLine(url);   
                //await file.WriteLineAsync(page.DocumentNode.OuterHtml); 
                //await file.FlushAsync();
               await GetDataFromTopoCentras(page);  

               pageNumber++;
               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               var containedItemNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
               var containedItem = containedItemNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;","")).ToList()[0].ToString();       
               
                while (!inTheList.Contains(containedItem))
                {

               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               Console.WriteLine(url);

                    await GetDataFromTopoCentras(page);
                    pageNumber++;
               url = category[i]+"?limit=80&p="+pageNumber;
               page = web.Load(url); 
               containedItemNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
               containedItem = containedItemNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;","")).ToList()[0].ToString();       
                                  
                }
                 inTheList.Clear();
            }  
        }

         public async Task GetDataFromTopoCentras(HtmlDocument page)
        { 
            //StreamWriter file = new StreamWriter(Environment.CurrentDirectory+@"\Links\html.txt");
            var FindEShop = context.Eshops.FirstOrDefault(shop=> shop.Name == EshopName);
            var pricesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[contains(@class,'price-wrapper')]");
            var pricesNodes2 = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[contains(@class,'price-wrapper')]");
            var codesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//a[@class='title']");
            
            List<string> prices = new List<string>();
            var codes = codesNodes.Select(node => node.InnerText.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ","").Replace("&quot;",""));
            var pricesHtml = pricesNodes.Select(node => node.InnerHtml);
            
            if(page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper leasing final-price']") != null){
                pricesNodes = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper leasing final-price']//span[@class='final-price']");
            }
            if(page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper']")!=null){
                pricesNodes2 = page.DocumentNode.SelectNodes("//div[@class='additional-info']//span[@class='price-wrapper']//span[@class='price']");  
            }
            int priceID = 0;
            int priceID2 = 0;
            foreach (var item in pricesHtml)
            {
                if (item.Contains("/m"))
                {
                    
                   prices.Add(pricesNodes.Select(node => node.InnerText.Replace(" ", "").Replace("\n", "").Replace("Kaina:", "").Replace("€","").Replace(".", "").Replace(",",".")).ToList()[priceID++].ToString());
                }else{
                    
                    prices.Add(pricesNodes2.Select(node => node.InnerText.Replace(" ", "").Replace("\n", "").Replace("€","").Replace(".", "").Replace(",",".")).ToList()[priceID2++].ToString());
                }
            }
            List<Data> sets = codes
                .Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();
            foreach(var set in sets)
            {
            inTheList.Add(set.Code);


            var line = String.Format("{0,-40} {1}", set.Code, set.Price);
            
            var Coding = ProductRecognitionTopoCentras(set.Code);
            Console.WriteLine(Coding);

            }

        }
        public string ProductRecognitionTopoCentras(string product)
        { 
           int index = 0;
           foreach (Char item in product)
           {
               if(Char.IsUpper(item) && index!=0) break;
               index++;
           }
            product = product.Remove(0,index);
            var parts = product.Split(new[] { ' ','-','/','+' });
            List<Product> searchingProd = new List<Product>();
            var queryName = $"SELECT * FROM [PriceAdvisor].[dbo].[Products] WHERE [Name] LIKE '%"+parts[0]+"%' AND [Name] LIKE '%"+parts[parts.Count()-1].Replace("Dos","")+"%' ";
            
            for (int i = parts.Count()-2; i > 0; i--)
            {
                /*if(!parts[i].ToLower().Contains("juod") || !parts[i].ToLower().Equals("su") ||!parts[i].ToLower().Equals("mikrofonu")
                    || !parts[i].ToLower().Equals("belaidės") ||!parts[i].ToLower().Contains("balt")|| !parts[i].ToLower().Contains("raudo") ||!parts[i].ToLower().Contains("mėlyn")
                    || !parts[i].ToLower().Equals("bevielės") ||!parts[i].ToLower().Equals("mikrofonu")||!parts[i].ToLower().Contains("žali") ||!parts[i].ToLower().Contains("sidabr"))
                {*/
                queryName = queryName+"AND [Name] LIKE '%"+parts[i].Replace("GB","").Replace("Ti","")
                                        .Replace("TB","").Replace("GTX","").Replace("+","").Replace("SSD","")+"%' ";
                
               // }
            }
            if(parts.Count()<4){
            for (int i = 1; i < parts.Count(); i++)
            {
               var queryCode = $"SELECT * FROM [PriceAdvisor].[dbo].[Products] WHERE [Code] LIKE '%"+parts[i]+"%'";
               searchingProd = context.Products.FromSql(queryCode).ToList(); 
               if(searchingProd.Count()==1) 
                    break;
            }
            }
            if(searchingProd.Count()==1)
            {}else{
            searchingProd = context.Products.FromSql(queryName).ToList();
                }//if(searchingProd.Count()==1)
                //break;
            if(searchingProd.Count()>0)
            Console.WriteLine(searchingProd[0].Code);
            /*List<Product> FindProduct = new List<Product>();
            int cnt = 1000000;
            int idx = 0;
            for (int i = 0; i < parts.Count(); i++)
            {
               FindProduct = context.Products.Where(prod=> prod.Name.Contains(parts[i]) && prod.Name.Contains(parts[i+1])&& prod.Name.Contains(parts[i+1])).ToList();    

                if(FindProduct.Count() > 1 && FindProduct.Count()<cnt)
                {
                    cnt =FindProduct.Count();
                    idx = i;
                }
            }
                FindProduct = context.Products.Where(prod=> prod.Name.Contains(parts[idx])).ToList();    
                   var ListToRecogn = FindProduct;
                   int correct = 0;
                   int countas = 0;
                   int prodId = 0;
                   for (int j = 0; j < FindProduct.Count(); j++)
                   {
                       var parts2 = FindProduct[j].Name.ToString().Split(new[] { ' ','-','/' });
                       for (int k = 0; k < parts.Count(); k++)
                       { 
                           for (int l = 0; l < parts2.Count(); l++)
                           {
                              if(parts[k].Equals(parts2[l]))
                            {
                                correct++;
                            } 
                           }
                       }
                        if(correct>countas)
                        {
                            countas = correct;
                            prodId = j;
                            
                        } 
                        correct = 0;   
                       
                   }
                var FindProductOne = FindProduct[prodId];
                Console.WriteLine(FindProductOne.Code);*/

                   /* for (int j = i+1; j < parts.Count(); j++)
                    {
                        for (int k = 0; k < ListToRecogn.Count(); k++)
                        {  
                            if(!ListToRecogn[k].Name.Contains(parts[j].Replace("GB","")))
                            {
                                ListToRecogn.Remove(ListToRecogn[k]);
                            }
                        }
                         if(ListToRecogn.Count==1)
                         {
                            return ListToRecogn[0].Code;        
                         }
                    }*/
               
            
         return null;            
        }

        public async Task GetLinksFromTopoCentras()
        { 
            StreamWriter file = new StreamWriter(Environment.CurrentDirectory+@"\Links\TopoCentrasLinks.txt");
            HtmlWeb web = new HtmlWeb();
            var url = "https://www.topocentras.lt";
            var page = web.Load(url);

            var linkNodesInTheShop = page.DocumentNode.SelectNodes("//a[contains(@href,'https://www.topocentras.lt')]");
            var links = linkNodesInTheShop.Select(node => node.Attributes["href"]);
            
            foreach (var link in links)
            {
               if(link.Value.Contains("sveikata")  ||  link.Value.Contains("buitine"))
               {}else{
                await file.WriteLineAsync(link.Value);
                Console.WriteLine(link.Value);
               }
            }
            await file.FlushAsync();
        
        }
    }
}