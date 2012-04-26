using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.Models;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class kalenderViewByTenantController : Controller
    {
        //
        // GET: /kalenderViewByTenant/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSalesByDate(string date)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            var list = new KalenderTenanViewRepository().ReposetoryKalenderViewTenan(date, cp.CompanyId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
    }
}