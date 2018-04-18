using System.Collections.Generic;
using System.Threading.Tasks;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Core
{
    public interface IAdministrationRepository
    {
        Task<Administration> GetScrapable(int id); 
        Task<IEnumerable<Administration>> GetScrapables();
        void Add(Administration scrapable);
    }
}