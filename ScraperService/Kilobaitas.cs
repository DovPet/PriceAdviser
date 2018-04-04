using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using OpenQA.Selenium;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.ScraperService
{
    public class Kilobaitas
    {
        //static StreamWriter kilobaitas = new StreamWriter(@"C:\Users\Dovydas.Petrutis\source\repos\ConsoleApp1\ConsoleApp1\Kilobaitas.txt");

        public static void Kilobaitass(IWebDriver driver, HtmlDocument doc)
        {
            List<int> kategorijuID =
                new List<int>() { 390, 406, 435, 438, 638, 442, 557, 476, 482, 489, 652, 510, 550, 570, 583 };
            driver.Navigate()
                .GoToUrl("https://www.kilobaitas.lt/Kompiuteriai/Plansetiniai_(Tablet)/CatalogStore.aspx?CatID=PL_0");
            //390-781
            for (int kategorijosNr = 0; kategorijosNr < kategorijuID.Count; kategorijosNr++)
            {
                driver.Navigate()
                    .GoToUrl(
                        "https://www.kilobaitas.lt/Kompiuteriai/Plansetiniai_(Tablet)/CatalogStore.aspx?CatID=PL_" +
                        kategorijuID[kategorijosNr] + "");
                //td[@class='hdMain']//a[contains(text(),'Buitinė technika')]
                if (driver.FindElements(By.XPath("//img[@src='/Images/design/notify_information.gif']")).Count > 0 ||
                        driver.FindElements(By.XPath("//td[@class='hdMain']//a[contains(text(),'Buitinė technika')]")).Count > 0)
                {
                }
                else
                {
                    driver.FindElement(By.XPath("//select[@class='simpleSelect']")).Click();
                    driver.FindElement(By.XPath("//select[@class='simpleSelect']//option[@value='90']")).Click();

                    while (driver.FindElements(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]"))
                               .Count > 0)
                    {

                        gautiDuomenisIsKilobaito(driver, doc);
                    }
                    gautiDuomenisIsKilobaito(driver, doc);
                }
            }
            driver.Close();
        }

        public static void gautiDuomenisIsKilobaito(IWebDriver driver, HtmlDocument doc)
        {
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
                    .Replace("\r", ""));

            List<Data> sets =
                codes.Zip(prices, (code, price) => new Data() { Code = code, Price = price }).ToList();

            foreach (var set in sets)
            {

                {
                    var line = String.Format("{0,-40} {1}", set.Code, set.Price);

                    
                    Console.WriteLine(line);
                }
            }
            if (driver.FindElements(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]")).Count != 0)
            {
                next = driver.FindElement(By.XPath("(//input[contains(@onmouseover,'NextOverBottom')])[1]"));
                js.ExecuteScript("arguments[0].click();", next);

                //System.Threading.Thread.Sleep(2000);
                ie = driver.FindElement(By.XPath("//html"));
            }
        }
    }
}