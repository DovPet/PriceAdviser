using System.Collections.Generic;
using System.Threading.Tasks;
using PriceAdvisor.Core.Models;

namespace PriceAdvisor.Core
{
    public interface IExportRepository
    {
         Task<IEnumerable<Price>> GetPricesAsync();
    }
}