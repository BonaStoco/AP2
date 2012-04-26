using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers
{
    //[HandleError]
    //[Authorize(Roles = APRoles.TENANT_ROLES)]
    public class PartRequestStatusController : Controller
    {
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

        public ActionResult PartRequestStatus()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<RequestProduct> products = MasterDataRepository().FindByStatusAndTenanId(cp.CompanyId, DateTime.Now.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
            return View(products);
        }
        public PartialViewResult GetPartRequestStatus(DateTime dari,DateTime sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<RequestProduct> products = MasterDataRepository().FindByStatusAndTenanId(cp.CompanyId, dari, sampai);
            return PartialView("GetPartRequestStatus",products);
        }

    }
}