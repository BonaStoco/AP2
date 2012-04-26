using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.Controllers
{
    public class DisplayMonitoringEPOSController : Controller
    {
        IAPMasterRepository _repo = new APMasterRepository();

        //
        // GET: /DisplayMonitoringEPOS/

        public ActionResult Index()
        {
            return View("MonitoringEPOS");
        }

        public JsonResult FindMonitoringByDate(string date)
        {
            IList<TenantDailySalesMonitoringEPOS> tdsMonitoringEPOS = _repo.FindTenantDailySalesMoniyoringByDate(date);
            return Json(tdsMonitoringEPOS, JsonRequestBehavior.AllowGet);
        }

    }
}
