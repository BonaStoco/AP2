using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;

namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class LaporanVoidAPController : Controller
    {
        IMasterDataRepository repo = null;
        ILaporanVoidRepository _repo = new LaporanVoidRepository();

        public ActionResult Index()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }
        public ActionResult ViewLaporanVoidAp(string tenanId)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(Int32.Parse(tenanId));
            ViewBag.tenanId = tenanId;
            ViewBag.tenanName = tenan.TenanName;
            return View("FormVoidByTenat");
        }

        public JsonResult FindDetailVoidPerKasirByDate(string tenanId, int sessionid, string dari, string sampai)
        {
            IList<LaporanVoidDetail> detail = _repo.FindDetailVoidPerKasirByDate(Int32.Parse(tenanId), sessionid, dari, sampai);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindSummaryVoidPerKasirByDate(string tenanId, int sessionid, string dari, string sampai)
        {
            IList<LaporanVoidSummary> summary = _repo.FindSummaryVoidPerKasirByDate(Int32.Parse(tenanId), sessionid, dari, sampai);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindSessionByTenantAndDate(string tenantId, string dari, string sampai)
        {
            IList<SessionKasir> session = _repo.FindSessionByTenantAndDate(Int32.Parse(tenantId), dari, sampai);
            return Json(session, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenanByName(string key)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenanByName(key);
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndName(key, cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraTerminalAndName(key, cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminalAndSubTerminalAndName(key, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }

            return Json(tenans, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenans()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().GetAllTenan();
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandara(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }

            return Json(tenans, JsonRequestBehavior.AllowGet);
        }

        private IMasterDataRepository MasterDataRepository
        {
            get
            {
                if (repo == null)
                    repo = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
                return repo;
            }
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }

    }
}
