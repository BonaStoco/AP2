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
    public class SubTerminalReportSaleController : Controller
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

        public ActionResult PageHari(int locationId, int subTerminalId, string subTerminalName)
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
            return View("../ReportSale/PenjualanTotal/SubTerminal/TotalPenjualanHarianSubTerminal", dailySales);
        }

        public ActionResult PageBulan(int locationId, int subTerminalId, string subTerminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<SubTerminalMonthlySales> monthlySales = subTerminalRepo.FindSubTerminalMontlySales(period, subTerminalId, locationId);
            ViewBag.TotalSalePerMonth = subTerminalRepo.TotalSalePerMonth(monthlySales).ToString("N");
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return View("../ReportSale/PenjualanTotal/SubTerminal/TotalPenjualanBulananSubTerminal", monthlySales);
        }
        public ActionResult PageTahun(int locationId, int subTerminalId, string subTerminalName)
        {
            period = today.Year.ToString();
            IList<SubTerminalYearlySales> yearlySales = subTerminalRepo.FindSubTerminalYearlySales(period, subTerminalId, locationId);
            ViewBag.TotalSalePerYear = subTerminalRepo.TotalSalePerYear(yearlySales).ToString("N");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return View("../ReportSale/PenjualanTotal/SubTerminal/TotalPenjualanTahunanSubTerminal", yearlySales);
        }

        public ActionResult RekapTenanHarian(int locationId, int subTerminalId, string subTerminalName)
        {
            GetDate();
            GetMonth();     
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<TenantSubterminalDailySales> dailySales = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantSubterminalDailySales> previousDay = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantSubterminalDailySales> twoDaysBefore = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_twoDaysBeforeDate, locationId);

            ViewBag.TotalSalePerDay = subTerminalRepo.TotalTenanSubTerminalSalePerDay(dailySales).ToString("N");
            ViewBag.PreviousDayDate = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N");
            ViewBag.TwoDaysBeforeDate = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N");

            ViewBag.TotalSalePerDayUSD = dailySales.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");
            ViewBag.PreviousDayDateUSD = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");
            ViewBag.TwoDaysBeforeDateUSD = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");

            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            ViewBag.Today = DateTime.Today;
           
            return View("../ReportSale/RekapPerTenan/SubTerminal/RekapPerTenanHarianSubTerminal", dailySales);
        }

        public JsonResult RekapTenanHarianData(int no, int locationId)
        {
            today = today.AddDays(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<TenantSubterminalDailySales> dailySales = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(today, locationId);

            return Json(dailySales, JsonRequestBehavior.AllowGet);
        }       

        public JsonResult Previous(int no, int locationId)
        {
            today = today.AddDays(no);

            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<TenantSubterminalDailySales> dailySales = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantSubterminalDailySales> previousDay = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantSubterminalDailySales> twoDaysBefore = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_twoDaysBeforeDate, locationId);
            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = subTerminalRepo.TotalTenanSubTerminalSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N")

            };

            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Next(int no, int locationId)
        {

            today = today.AddDays(no);

            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TenantSubterminalDailySales> dailySales = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantSubterminalDailySales> previousDay = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantSubterminalDailySales> twoDaysBefore = subTerminalRepo.FindTenantSubTerminalDailySaleByPeriod(_twoDaysBeforeDate, locationId);
            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = subTerminalRepo.TotalTenanSubTerminalSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<TenantSubterminalDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N")

            };

            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RekapTenanBulanan(int locationId, int subTerminalId, string subTerminalName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<TenantSubterminalMonthlySales> monthlySales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(period, locationId);
            IList<TenantSubterminalMonthlySales> monthlyPreviousSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantSubterminalMonthlySales> monthlyTwoBeforeSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(twoBeforeMonth, locationId);
   
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;

            ViewBag.TotalSaleCurrentPerMonth = subTerminalRepo.TotalTenanSubTerminalSalePerMonth(monthlySales).ToString("N");
            ViewBag.TotalSalePreviousPerMonth = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N");
            ViewBag.TotalSaleTwoBeforePerMonth = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N");

            ViewBag.TotalSaleCurrentPerMonthUSD = monthlySales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N2");
            ViewBag.TotalSalePreviousPerMonthUSD = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N2");
            ViewBag.TotalSaleTwoBeforePerMonthUSD = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N2");
            return View("../ReportSale/RekapPerTenan/SubTerminal/RekapPerTenanBulananSubTerminal", monthlySales);
        }

        public JsonResult RekapTenanBulananData(int no, int locationId)
        {
            today = today.AddDays(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TenantSubterminalMonthlySales> monthlySales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(period, locationId);

            return Json(monthlySales, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PreviousMonth(int no, int locationId)
        {
            today = today.AddDays(no);

            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<TenantSubterminalMonthlySales> monthlySales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(period, locationId);
            IList<TenantSubterminalMonthlySales> monthlyPreviousSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantSubterminalMonthlySales> monthlyTwoBeforeSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = monthlySales[0].CompanyName,
                LocationId = locationId,
                TotalSaleIDRCurrentMonth = subTerminalRepo.TotalTenanSubTerminalSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),
  
            };

            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NextMonth(int no, int locationId)
        {
            today = today.AddDays(no);

            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<TenantSubterminalMonthlySales> monthlySales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(period, locationId);
            IList<TenantSubterminalMonthlySales> monthlyPreviousSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantSubterminalMonthlySales> monthlyTwoBeforeSales = subTerminalRepo.FindTenantSubTerminalMonthlySaleByPeriod(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = monthlySales[0].CompanyName,
                LocationId = locationId,
                TotalSaleIDRCurrentMonth = subTerminalRepo.TotalTenanSubTerminalSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantSubterminalMonthlySales>(y => y.MonthlyTotalSalesPerTenantInUSD).ToString("N"),

            };

            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RekapTenanTahunan(int locationId, int subTerminalId, string subTerminalName)
        {
            period = today.Year.ToString();
            IList<TenantSubterminalYearlySales> yearlySales = subTerminalRepo.FindTenantSubTerminalYearlySaleByPeriod(period, subTerminalId);
            ViewBag.TotalSalePerYear = yearlySales.Sum<TenantSubterminalYearlySales>(y => y.YearlyTotalSalePerTenan).ToString("N");
            ViewBag.TotalSalesPerTenanInUSD = yearlySales.Sum<TenantSubterminalYearlySales>(y => y.TotalSalesPerTenanInUSD).ToString("N2");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.TerminalId = subTerminalId;
            ViewBag.SubTerminalName = subTerminalName;
            return View("../ReportSale/RekapPerTenan/SubTerminal/RekapPerTenanTahunanSubTerminal", yearlySales);
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

        private string ConvertMonth(DateTime date)
        {
            if (date.Month < 10)
            {
                return bulan = string.Format("{0}{1}", 0, date.Month);
            }
            else
            {
                return bulan = date.Month.ToString();
            }
        }
    }
}
