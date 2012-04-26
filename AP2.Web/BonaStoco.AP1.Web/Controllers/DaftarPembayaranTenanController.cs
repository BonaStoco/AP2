using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.TELKOM_ROLES)]
    public class DaftarPembayaranTenanController : Controller
    {
        IAPMasterRepository _apRepo = new APMasterRepository();
        IList<TenanSpeedyPayment> billings;
        //
        // GET: /DaftarPembayaranTenan/
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FindDataBillingByYear(string year)
        {
            billings = _apRepo.FindBillingTenantByYear(year);
            return Json(billings, JsonRequestBehavior.AllowGet);
        }

        public void AddAllActiveTenanForNextYear(string year)
        {
            billings = _apRepo.FindBillingTenantByYear(year);
            if (billings.Count == 0)
            {
                _apRepo.CreatePaymentListForYear(year);
            }
        }

        public void UpdateTenanPayment(string tenanId, string tahun, string bulan, string blnCeklist)
        {
            _apRepo.UpdateTenanPaymentByChecked(long.Parse(tenanId), tahun, bulan, bool.Parse(blnCeklist));
        }

        public void AddUnregisterTenanPayment(string year)
        {
            _apRepo.CreateUnregisterTenantPaymentByYear(year);
        }
    }
}
