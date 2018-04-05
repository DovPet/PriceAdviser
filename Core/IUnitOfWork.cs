using System.Threading.Tasks;

namespace PriceAdvisor.Core
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}