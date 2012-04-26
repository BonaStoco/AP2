using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using System.Web.Security;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AP1Page()
        {
            SummaryHome summaryHome = MasterDataRepository().GetSummaryAP();
            return View(summaryHome);
        }
        public ActionResult AP2Page()
        {
            SummaryHome summaryHome = MasterDataRepository().GetSummaryAP2();
            return View(summaryHome);
        }
        public ActionResult UmumPage()
        {
            SummaryHome summaryHome = MasterDataRepository().GetSummaryUmum();
            return View(summaryHome);
        }
        public ActionResult AP2BandaraPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeBandara summaryBandara = MasterDataRepository().GetSummaryBandara(cp.Role.Bandara.ToString(), cp.Role.Category.ToString());
            return View(summaryBandara);
        }
        public ActionResult AP2TerminalPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeTerminal summaryTerminal = MasterDataRepository().GetSummaryTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return View(summaryTerminal);
        }
        public ActionResult AP2CargoPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeTerminal summaryTerminal = MasterDataRepository().GetSummaryTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return View(summaryTerminal);
        }
        public ActionResult AP2SubTerminalPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeSubTerminal summarySubTerminal = MasterDataRepository().GetSummarySubTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString(), cp.Role.SubTerminal.ToString());
            return View(summarySubTerminal);
        }

        public ActionResult AP1BandaraPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeBandara summaryBandara = MasterDataRepository().GetSummaryBandara(cp.Role.Bandara.ToString(), cp.Role.Category.ToString());
            return View(summaryBandara);
        }
        public ActionResult AP1TerminalPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeTerminal summaryTerminal = MasterDataRepository().GetSummaryTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return View(summaryTerminal);
        }
        public ActionResult AP1SubTerminalPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeSubTerminal summarySubTerminal = MasterDataRepository().GetSummarySubTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString(), cp.Role.SubTerminal.ToString());
            return View(summarySubTerminal);
        }

        public ActionResult TenantPage()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            SummaryHomeTenan summaryHomeTenan = MasterDataRepository().GetSummaryTenant(cp.CompanyId);
            if (summaryHomeTenan == null)
                summaryHomeTenan = new SummaryHomeTenan()
                {
                    TenanId = cp.CompanyId
                };
               
            return View(summaryHomeTenan);
        }

        public ActionResult TelkomPage()
        {
            return View();
        }
        public ActionResult Guest()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public string Test(string username, string password)
        {
            return String.Format("Hello {0}, {1}", username, password);
        }
        
        private IDashBoardRepository MasterDataRepository()
        {
            IDashBoardRepository _apRepo = new DashboardRepository();
            return _apRepo;
        }
        
        [Authorize(Roles=APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktif()
        {
            IList<DetailTenanAktif> detail = MasterDataRepository().FindDetaiTenanAktif();
            return Json(detail, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult FindTenanAktifByName(string key)
        {
            IList<DetailTenanAktif> tenans = MasterDataRepository().FindTenanAktifByName(key);
            return Json(tenans, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifInBandara()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktif> detail = MasterDataRepository().FindDetailAkifInBandara(cp.Role.Bandara.ToString(), cp.Role.Category.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifInTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktif> detail = MasterDataRepository().FindDetailAkifInTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifInSubTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktif> detail = MasterDataRepository().FindDetailAkifInSubTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString(), cp.Role.SubTerminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        //tenan yang aktif kemarin
        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifKemarin()
        {
            IList<DetailTenanAktifKemarin> detail = MasterDataRepository().FindDetailTenanAktifKemarin();
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifKemarinInBandara()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifKemarin> detail = MasterDataRepository().FindDetailAkifKemarinInBandara(cp.Role.Bandara.ToString(), cp.Role.Category.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifKemarinInTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifKemarin> detail = MasterDataRepository().FindDetailAkifKemarinInTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifKemarinInSubTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifKemarin> detail = MasterDataRepository().FindDetailAkifKemarinInSubTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString(), cp.Role.SubTerminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        //tenan yang aktif hari ini
        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifHariIni()
        {
            IList<DetailTenanAktifHariIni> detail = MasterDataRepository().FindDetailTenanAktifHariIni();
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifHariIniInBandara()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifHariIni> detail = MasterDataRepository().FindDetailAkifHariIniInBandara(cp.Role.Bandara.ToString(), cp.Role.Category.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifHariIniInTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifHariIni> detail = MasterDataRepository().FindDetailAkifHariIniInTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.AP_ROLES)]
        public JsonResult DetailTenanAktifHariIniInSubTerminal()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailTenanAktifHariIni> detail = MasterDataRepository().FindDetailAkifHariIniInSubTerminal(cp.Role.Bandara.ToString(), cp.Role.Category.ToString(), cp.Role.Terminal.ToString(), cp.Role.SubTerminal.ToString());
            return Json(detail, JsonRequestBehavior.AllowGet);
        }
        
        private IAPMasterRepository APMasterRepository()
        {
            IAPMasterRepository _apMasterRepo = new APMasterRepository();
            return _apMasterRepo;
        }

        private string ConvertPeriod(string tahun, string bulan)
        {
            string bln;
            if (int.Parse(bulan) < 10)
            {
                bln = string.Format("{0}{1}", 0, bulan);
            }
            else
            {
                bln = bulan;
            }

            string period = string.Format("{0}{1}", tahun, bln);
            return period;
        }

        public JsonResult GetDetailFakturbyPeriod()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            DateTime firstDay = DateTime.Today.AddDays(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) * -1);
            IList<TenantSalesMonitoring> salesMonitoring = MasterDataRepository().FindTenantSalesMonitoringByTenanAndMonthPeriode(cp.CompanyId, firstDay.ToString("yyy-MM-dd"), DateTime.Today.ToString("yyy-MM-dd")).ToList();
            //IList<DetailFakturAP> detailFakturAP = APMasterRepository().FindDetailFakturByPeriodeandTenanId(ConvertPeriod(tahun, bulan), cp.CompanyId);
            return Json(salesMonitoring, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewInfo()
        {
            IList<Info> info = MasterDataRepository().FindNewInfo();
            return Json(info, JsonRequestBehavior.AllowGet);
        }
    }
}