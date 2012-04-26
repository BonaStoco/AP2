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
    public class BandaraReportSaleController : Controller
    {
        ITerminalReportSaleRepository terminalRepo = new TerminalReportSaleRepository();
        DateTime today = DateTime.Today;
        string hari;
        string bulan;
        string period;

        public PartialViewResult PageHari(int locationId,int terminalId, string terminalName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}-{1}-{2}", today.Year, bulan, hari);
            IList<TerminalDailySales> dailySales = terminalRepo.FindTerminalDailySalesByTransactionDate(DateTime.Parse(period), locationId,terminalId);
            ViewBag.TotalSalePerDay = terminalRepo.TotalSalePerDay(dailySales).ToString("N");
            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/PenjualanTotal/Bandara/TotalPenjualanHarianBandara", dailySales);
        }

        public PartialViewResult PageBulan(int locationId,int terminalId, string terminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TerminalMonthlySales> monthlySales = terminalRepo.FindTerminalMonthlySalesByMonthPeriod(period, locationId,terminalId);
            ViewBag.TotalSalePerMonth = terminalRepo.TotalSalePerMonth(monthlySales).ToString("N");
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/PenjualanTotal/Bandara/TotalPenjualanBulananBandara", monthlySales);

        }
        public PartialViewResult PageTahun(int locationId,int terminalId, string terminalName)
        {
            period = today.Year.ToString();
            IList<TerminalYearlySales> yearlySales = terminalRepo.FindTerminalYearlySalesByMonthPeriod(period, locationId,terminalId);
            ViewBag.TotalSalePerYear = terminalRepo.TotalSalePerYear(yearlySales).ToString("N");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/PenjualanTotal/Bandara/TotalPenjualanTahunanBandara", yearlySales);
        }

        public PartialViewResult RekapTenanHarian(int terminalId, string terminalName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}-{1}-{2}", today.Year, bulan, hari);
            IList<TenantTerminalDailySales> dailySales = terminalRepo.FindTenantTerminalDailySaleByPeriod(period, terminalId);
            ViewBag.TotalSalePerDay = terminalRepo.TotalTenanTerminalSalePerDay(dailySales).ToString("N");
            ViewBag.Waktu = "Hari ini";
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/RekapPerTenan/Bandara/RekapPerTenanHarianBandara", dailySales);
        }
        public PartialViewResult RekapTenanBulanan(int terminalId, string terminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TenantTerminalMonthlySales> monthlySales = terminalRepo.FindTenantTerminalMonthlySaleByPeriod(period, terminalId);
            ViewBag.TotalSalePerMonth = terminalRepo.TotalTenanTerminalSalePerMonth(monthlySales).ToString("N");
            ViewBag.Waktu = "Bulan ini";
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/RekapPerTenan/Bandara/RekapPerTenanBulananBandara", monthlySales);
        }

        public PartialViewResult RekapTenanTahunan(int terminalId, string terminalName)
        {
            period = today.Year.ToString();
            IList<TenantTerminalYearlySales> yearlySales = terminalRepo.FindTenantTerminalYearlySaleByPeriod(period, terminalId);
            ViewBag.TotalSalePerYear = terminalRepo.TotalTenanTerminalSalePerYear(yearlySales).ToString("N");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.TerminalId = terminalId;
            ViewBag.TerminalName = terminalName;
            return PartialView("../ReportSale/RekapPerTenan/Bandara/RekapPerTenanTahunanBandara", yearlySales);
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
