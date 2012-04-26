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
    public class LaporanVoidController : Controller
    {
        ILaporanVoidRepository _repo = new LaporanVoidRepository();
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult FindDetailVoidPerKasirByDate(int sessionid, string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanVoidDetail> detail = _repo.FindDetailVoidPerKasirByDate(cp.CompanyId, sessionid, dari, sampai);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult FindSummaryVoidPerKasirByDate(int sessionid, string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanVoidSummary> summary = _repo.FindSummaryVoidPerKasirByDate(cp.CompanyId, sessionid, dari, sampai);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindSessionByTenantAndDate(string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<SessionKasir> session = _repo.FindSessionByTenantAndDate(cp.CompanyId, dari, sampai);
            return Json(session, JsonRequestBehavior.AllowGet);
        }

    }
}
