using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Report;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.ReportSale.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class TerminalReportSaleController : Controller
    {
        ISubTerminalReportSaleRepository subTerminalRepo = new SubTerminalReportSaleRepository();
        DateTime today = DateTime.Today;
        string hari;
        string bulan;
        string period;

        public JsonResult GetSalesByDate(string id)
        {
            var list = new KalenderViewRepository().ReposetoryKalenderView(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PageHari(int locationId, int subTerminalId, string subTerminalName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}-{1}-{2}", today.Year, bulan, hari);
            IList<SubTerminalDailySales> dailySales = subTerminalRepo.FindSubTerminalDailySales(DateTime.Parse(period), subTerminalId, locationId);
            ViewBag.TotalSalePerDay = subTerminalRepo.TotalSalePerDay(dailySales).ToString("N");
            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/PenjualanTotal/Terminal/TotalPenjualanHarianTerminal", dailySales);
        }

        public PartialViewResult PageBulan(int locationId, int subTerminalId, string subTerminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<SubTerminalMonthlySales> monthlySales = subTerminalRepo.FindSubTerminalMontlySales(period, subTerminalId, locationId);
            ViewBag.TotalSalePerMonth = subTerminalRepo.TotalSalePerMonth(monthlySales).ToString("N");
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/PenjualanTotal/Terminal/TotalPenjualanBulananTerminal", monthlySales);
        }
        public PartialViewResult PageTahun(int locationId, int subTerminalId, string subTerminalName)
        {
            period = today.Year.ToString();
            IList<SubTerminalYearlySales> yearlySales = subTerminalRepo.FindSubTerminalYearlySales(period, subTerminalId, locationId);
            ViewBag.TotalSalePerYear = subTerminalRepo.TotalSalePerYear(yearlySales).ToString("N");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/PenjualanTotal/Terminal/TotalPenjualanTahunanTerminal", yearlySales);
        }

        public PartialViewResult RekapTenanHarian(int locationId, int subTerminalId, string subTerminalName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}-{1}-{2}", today.Year, bulan, hari);
            IList<TenantSubterminalDailySales> dailySales = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(DateTime.Parse(period), subTerminalId);
            ViewBag.TotalSalePerDay = subTerminalRepo.TotalTenanSubTerminalSalePerDay(dailySales).ToString("N");
            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/RekapPerTenan/Terminal/RekapPerTenanHarianTerminal", dailySales);
        }
        public PartialViewResult RekapTenanBulanan(int locationId, int subTerminalId, string subTerminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TenantSubterminalMonthlySales> monthlySales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(period, subTerminalId);
            ViewBag.TotalSalePerMonth = subTerminalRepo.TotalTenanSubTerminalSalePerMonth(monthlySales).ToString("N");
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/RekapPerTenan/Terminal/RekapPerTenanBulananTerminal", monthlySales);
        }
        public PartialViewResult RekapTenanTahunan(int locationId, int subTerminalId, string subTerminalName)
        {
            period = today.Year.ToString();
            IList<TenantSubterminalYearlySales> yearlySales = subTerminalRepo.FindTenantSubTerminalYearlySaleByPeriod(period, subTerminalId);
            ViewBag.TotalSalePerYear = subTerminalRepo.TotalTenanSubTerminalSalePerYear(yearlySales).ToString("N");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return PartialView("../ReportSale/RekapPerTenan/Terminal/RekapPerTenanTahunanTerminal", yearlySales);
        }
        private string GetDate()
        {
            if (today.Day < 10)
            {
                return hari = string.Format("{0}{1}", 0, today.Day);
            }
            else
            {
                return hari = today.Day.ToString();
            }
        }
        private string GetMonth()
        {
            if (today.Month < 10)
            {
                return bulan = string.Format("{0}{1}", 0, today.Month);
            }
            else
            {
                return bulan = today.Month.ToString();
            }
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
    }
}
