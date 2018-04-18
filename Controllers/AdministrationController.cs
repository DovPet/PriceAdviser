using Microsoft.AspNetCore.Mvc;
using PriceAdvisor.Core;
using PriceAdvisor.Persistence;

namespace PriceAdvisor.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PriceAdvisorDbContext context;

         public AdministrationController(IUnitOfWork unitOfWork,PriceAdvisorDbContext context)
        {
                this.context = context;
                this.unitOfWork = unitOfWork;
        }
    }
}