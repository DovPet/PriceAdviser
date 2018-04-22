using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;

namespace PriceAdvisor.ScraperService
{
    public interface IEshop
    {
        Task PrepareEshop(List<string> category,int from, int to); 
        Task PrepareEshop(int from, int to);
        Task PrepareEshop(IWebDriver driver, HtmlDocument page, List<int> categoryID);
        Task GetDataFromEshop(IWebDriver driver, HtmlDocument page);
    }
}