using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class DaftarPembayaranTenanAPController : Controller
    {
        IAPMasterRepository _apRepo = new APMasterRepository();
        IList<TenanSpeedyPayment> billings;
        CompanyProfiles cp;

        public ActionResult Index()
        {
            cp = new CompanyProfiles(this.HttpContext);
            if (APRoles.IsRoot(cp.RoleName))
            {
                return View("../DaftarPembayaranTenanAP/APTenant");
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                return View("../DaftarPembayaranTenanAP/Bandara");
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                return View("../DaftarPembayaranTenanAP/Terminal");
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                return View("../DaftarPembayaranTenanAP/SubTerminal");
            }
            return View();
        }

        public JsonResult FindDataBillingTenantByYearAndCategory(string year)
        {
            cp = new CompanyProfiles(this.HttpContext);
            billings = _apRepo.FindBillingTenantByYearAndCategory(year, cp.Role.Category);
            return Json(billings, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindDataBillingTenantByYearAndBandara(string year)
        {
            cp = new CompanyProfiles(this.HttpContext);
            billings = _apRepo.FindBillingTenantByYearAndBandara(year, cp.Role.Bandara);
            return Json(billings, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindDataBillingTenantByYearAndBandaraAndTerminal(string year)
        {
            cp = new CompanyProfiles(this.HttpContext);
            billings = _apRepo.FindBillingTenantByYearAndBandaraAndTerminal(year, cp.Role.Bandara, cp.Role.Terminal);
            return Json(billings, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult FindDataBillingTenantByYearAndBandaraAndTerminalAndSubTerminal(string year)
        {
            cp = new CompanyProfiles(this.HttpContext);
            billings = _apRepo.FindBillingTenantByYearAndBandaraAndTerminalAndSubTerminal(year, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            return Json(billings, JsonRequestBehavior.AllowGet);
        }
    }
}
