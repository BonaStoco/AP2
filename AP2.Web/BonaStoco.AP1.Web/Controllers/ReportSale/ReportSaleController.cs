using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using System.Collections;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;

namespace BonaStoco.AP1.Web.Controllers
{
    public class ReportSaleController : Controller
    {
        IList<BonaStoco.AP1.MasterData.Models.MappingCompany> company;
        IList<BonaStoco.AP1.MasterData.Models.MappingTerminal> terminal;
        IList<BonaStoco.AP1.MasterData.Models.MappingSubTerminal> subTerminal;
        IAPMasterRepository _repo = new APMasterRepository();

        public ActionResult PenjualanTotal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            if (APRoles.IsRoot(cp.RoleName))
            {
                company = MasterDataRepository().FindBandaraByCategoryId(cp.Role.Category);
                terminal = MasterDataRepository().FindTerminalByCategoryId(cp.Role.Category);
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryId(cp.Role.Category);
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                terminal = MasterDataRepository().FindTerminalByCategoryIdAndLocationId(cp.Role.Category,cp.Role.Bandara);
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryIdAndLocationId(cp.Role.Category,cp.Role.Bandara);
                if (terminal.Count == 0)
                    company = MasterDataRepository().FindBandaraById(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryIdAndTerminalId(cp.Role.Category,cp.Role.Terminal);
                if (subTerminal.Count == 0)
                    terminal = MasterDataRepository().FindTerminalById(cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                string _subTerminalName = MasterDataRepository().FindSubTerminalById(cp.Role.SubTerminal).SubTerminalName;
                return RedirectToAction("PageHari", "SubTerminalReportSale", new { locationId = cp.Role.Bandara.ToString(), subTerminalId = cp.Role.SubTerminal, subTerminalName = _subTerminalName});
            }

            ViewBag.SubTerminal = subTerminal;
            ViewBag.Bandara = company;
            ViewBag.Terminal = terminal;
            return View("../ReportSale/PenjualanTotal",cp);
        }

        public ActionResult RekapPerTenant()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            if (APRoles.IsRoot(cp.RoleName))
            {
                company = MasterDataRepository().FindBandaraByCategoryId(cp.Role.Category);
                terminal = MasterDataRepository().FindTerminalByCategoryId(cp.Role.Category);
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryId(cp.Role.Category);
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                terminal = MasterDataRepository().FindTerminalByCategoryIdAndLocationId(cp.Role.Category, cp.Role.Bandara);
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryIdAndLocationId(cp.Role.Category, cp.Role.Bandara);
                if (terminal.Count == 0)
                    company = MasterDataRepository().FindBandaraById(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                subTerminal = MasterDataRepository().FindSubTerminalByCategoryIdAndTerminalId(cp.Role.Category, cp.Role.Terminal);
                if (subTerminal.Count == 0)
                    terminal = MasterDataRepository().FindTerminalById(cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                string _subTerminalName = MasterDataRepository().FindSubTerminalById(cp.Role.SubTerminal).SubTerminalName;
                return RedirectToAction("RekapTenanHarian", "SubTerminalReportSale", new { locationId = cp.Role.Bandara.ToString(), subTerminalId = cp.Role.SubTerminal.ToString(), subTerminalName = _subTerminalName });
            }

            ViewBag.SubTerminal = subTerminal;
            ViewBag.Bandara = company;
            ViewBag.Terminal = terminal;
            return View("../ReportSale/RekapPerTenant",cp);
        }

        public ActionResult DetailPerTenant()
        {
            return View("../ReportSale/DetailPerTenan");
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
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndName(key,cp.Role.Bandara);
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
        public JsonResult ListDetailPenjualanPerTenan(string tenant, string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanDetailPenjualanPerTenanView> detail = new List<LaporanDetailPenjualanPerTenanView>();
            ILaporanDetailPenjualanPerTenanRepository repo = new LaporanDetailPenjualanPerTenanRepository();
            if (APRoles.IsRoot(cp.RoleName))
            {
                detail = repo.FindPenjualanByTenanIdAndDate(dari, sampai, Int32.Parse(tenant));
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                detail = repo.FindPenjualanByTenanIdAndDateInBandara(dari, sampai, Int32.Parse(tenant), cp.Role.Bandara.ToString());
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                detail = repo.FindPenjualanByTenanIdAndDateInTerminal(dari, sampai, Int32.Parse(tenant), cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                detail = repo.FindPenjualanByTenanIdAndDateInSubTerminal(dari, sampai, Int32.Parse(tenant), cp.Role.SubTerminal);
            }

            return Json(detail.OrderBy(no => no.TransactionNo), JsonRequestBehavior.AllowGet);
        }
		
        public JsonResult ListDetailRingkasanPenjualanTenantPerHari(string tenanName, string from, string to)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanRingkasanPenjualanPerHari> detail = new List<LaporanRingkasanPenjualanPerHari>();
            ILaporanRingkasanPenjualanPerHariRepository repo = new LaporanRingkasanPenjualanPerHariRepository();
            if (APRoles.IsRoot(cp.RoleName))
            {
                detail = repo.FindRingkasanPenjualanPerHariBycategory(Int32.Parse(tenanName),from,to);
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                detail = repo.FindRingkasanPenjualanPerHariByBandara(Int32.Parse(tenanName),from,to, cp.Role.Bandara);                
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                detail = repo.FindRingkasanPenjualanPerHariByTerminal(Int32.Parse(tenanName), from, to, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                detail = repo.FindRingkasanPenjualanPerHariBySubTerminal(Int32.Parse(tenanName),from,to,cp.Role.SubTerminal);
            }
            return Json(detail,JsonRequestBehavior.AllowGet);
        }
		
        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository().FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSalesProductDetailByTransactionNumber(string transactionNo, string tenanId)
        {
            IList<SalesProductDetail> ProductDetails = _repo.FindSalesDetailProductByTransactionNo(transactionNo,Int64.Parse(tenanId));
            return Json(ProductDetails.OrderBy(kode => kode.KodeProduk), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSummaryPenjualanPerTenan(string tenanId, string tanggal)
        {
            IList<SalesSummaryProduct> summary = _repo.FindSalesSummaryByTenantAndDate(tenanId, DateTime.Parse(tanggal));
            return Json(summary.OrderBy(kode => kode.KodeProduk), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSessionIdByTenantAndDate(int tenanId, string tanggal)
        {
            IList<SessionSummaryPerKasir> session = _repo.FindSessionPerkasirByTenantAndDate(tenanId, DateTime.Parse(tanggal));
            return Json(session, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSummaryPerkasirByDateAndTenan(int tenanId, string tanggal, int sessionId)
        {
            IList<SummaryPerKasir> summary = _repo.FindSummaryPerkasirByDateAndTenan(tenanId, DateTime.Parse(tanggal), sessionId);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
    }
}
