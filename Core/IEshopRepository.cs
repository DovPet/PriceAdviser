using System.Collections.Generic;
using System.Threading.Tasks;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Core
{
    public interface IEshopRepository
    {
        Task<EShop> GetEshop(int id); 
        Task<IEnumerable<EShop>> GetEshops();
        void Add(EShop eshop);
    }
}