using System.Collections.Generic;
using System.Threading.Tasks;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Core
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int id); 
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Price>> GetPrices();
        Task<Price> GetPrice(int id); 
    }
}