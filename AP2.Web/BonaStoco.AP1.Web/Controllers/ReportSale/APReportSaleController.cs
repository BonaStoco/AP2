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
    public class APReportSaleController : Controller
    {
        IBandaraReportSaleRepository _apRepo = new BandaraReportSaleRepository();
        DateTime today = DateTime.Today;
        string hari;
        string bulan;
        string period;       
        

        //
        // GET: /ReportSale/

        public PartialViewResult PagePenjualanHarianBandara(int locationId, string companyName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<DailySales> dailySales = _apRepo.FindDailySaleByPeriodAndLocationId(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<DailySales> previousDay = _apRepo.FindDailySaleByPeriodAndLocationId(_previousDayDate, locationId);
            IList<DailySales> twoDaysBefore = _apRepo.FindDailySaleByPeriodAndLocationId(_twoDaysBeforeDate, locationId);
            
            ViewBag.TotalSalePerDay = _apRepo.TotalSalePerDay(dailySales).ToString("N");
            ViewBag.PreviousDayDate = previousDay.Sum<DailySales>(y => y.TotalSale).ToString("N");
            ViewBag.TwoDaysBeforeDate = twoDaysBefore.Sum<DailySales>(y => y.TotalSale).ToString("N");

            ViewBag.TotalSalePerDayUSD = dailySales.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N");
            ViewBag.PreviousDayDateUSD = previousDay.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N");
            ViewBag.TwoDaysBeforeDateUSD = twoDaysBefore.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N");

            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;
            ViewBag.Today = DateTime.Today;
            return PartialView("../ReportSale/PenjualanTotal/AP/_PageTotalPenjualanHarianPerAP", dailySales);
        }

        public JsonResult NextPenjualanHarian(int no, int locationId)
        {
            today = today.AddDays(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<DailySales> dailySales = _apRepo.FindDailySaleByPeriodAndLocationId(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<DailySales> previousDay = _apRepo.FindDailySaleByPeriodAndLocationId(_previousDayDate, locationId);
            IList<DailySales> twoDaysBefore = _apRepo.FindDailySaleByPeriodAndLocationId(_twoDaysBeforeDate, locationId);

            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = _apRepo.TotalSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<DailySales>(y => y.TotalSale).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<DailySales>(y => y.TotalSale).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N")
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreviousPenjualanHarian(int no, int locationId)
        {
            today = today.AddDays(no);          
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<DailySales> dailySales = _apRepo.FindDailySaleByPeriodAndLocationId(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<DailySales> previousDay = _apRepo.FindDailySaleByPeriodAndLocationId(_previousDayDate, locationId);
            IList<DailySales> twoDaysBefore = _apRepo.FindDailySaleByPeriodAndLocationId(_twoDaysBeforeDate, locationId);

            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = _apRepo.TotalSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<DailySales>(y => y.TotalSale).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<DailySales>(y => y.TotalSale).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<DailySales>(y => y.TotalSaleInUSD).ToString("N")

            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PagePenjualanBulananBandara(int locationId, string companyName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));

            IList<MonthlySales> monthlySales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(period, locationId);
            IList<MonthlySales> monthlyPreviousSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(previousMonth, locationId);
            IList<MonthlySales> monthlyTwoBeforeSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(twoBeforeMonth, locationId);

            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;

            ViewBag.TotalSalePerMonth = _apRepo.TotalSalePerMonth(monthlySales).ToString("N");
            ViewBag.TotalSalePreviousPerMonth = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N");
            ViewBag.TotalSaleTwoBeforePerMonth = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N");

            ViewBag.TotalSaleCurrentPerMonthUSD = monthlySales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N");
            ViewBag.TotalSalePreviousPerMonthUSD = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N");
            ViewBag.TotalSaleTwoBeforePerMonthUSD = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N");

            return PartialView("../ReportSale/PenjualanTotal/AP/_PageTotalPenjualanBulananPerAP", monthlySales);
        }

        public JsonResult NextMonthPenjualan(int no, int locationId)
        {
            today = today.AddMonths(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<MonthlySales> monthlySales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(period, locationId);
            IList<MonthlySales> monthlyPreviousSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(previousMonth, locationId);
            IList<MonthlySales> monthlyTwoBeforeSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDRCurrentMonth = _apRepo.TotalSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreviousMonthPenjualan(int no, int locationId)
        {
            today = today.AddMonths(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<MonthlySales> monthlySales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(period, locationId);
            IList<MonthlySales> monthlyPreviousSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(previousMonth, locationId);
            IList<MonthlySales> monthlyTwoBeforeSales = _apRepo.FindMonthlySaleByMonthPeriodAndLocationId(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDRCurrentMonth = _apRepo.TotalSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSale).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<MonthlySales>(y => y.TotalSaleInUSD).ToString("N"),
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PagePenjualanTahunanBandara(int locationId, string companyName)
        {
            period = today.Year.ToString();
            IList<YearlySales> yearlySales = _apRepo.FindYearlySaleByYearPeriodAndLocationId(period, locationId);
            ViewBag.TotalSalePerYear = _apRepo.TotalSalePerYear(yearlySales).ToString("N");
            ViewBag.TotalSalePerYearUSD = yearlySales.Sum<YearlySales>(y => y.TotalSaleInUSD).ToString("N");
            ViewBag.Waktu = "Tahun";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;
            return PartialView("../ReportSale/PenjualanTotal/AP/_PageTotalPenjualanTahunanPerAP", yearlySales);
        }

        public PartialViewResult RekapTenanHarian(int locationId, string companyName)
        {
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<TenantBandaraDailySales> dailySales = _apRepo.FindTenantBandaraDailySaleByPeriod(today, locationId);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantBandaraDailySales> previousDay = _apRepo.FindTenantBandaraDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantBandaraDailySales> twoDaysBefore = _apRepo.FindTenantBandaraDailySaleByPeriod(_twoDaysBeforeDate, locationId);

            ViewBag.TotalSalePerDay = _apRepo.TotalTenanBandaraSalePerDay(dailySales).ToString("N");
            ViewBag.PreviousDayDate = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N");
            ViewBag.TwoDaysBeforeDate = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N");

            ViewBag.TotalSalePerDayUSD = dailySales.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");
            ViewBag.PreviousDayDateUSD = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");
            ViewBag.TwoDaysBeforeDateUSD = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N");
            
            ViewBag.Waktu = "Hari ini";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;
            ViewBag.Today = DateTime.Today;
            return PartialView("../ReportSale/RekapPerTenan/AP/RekapPerTenanHarianAP", dailySales);
        }


        public JsonResult RekapTenanHarianData(int no, int locationId)
        {           
            today = today.AddDays(no);            
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            IList<TenantBandaraDailySales> dailySales = _apRepo.FindTenantBandaraDailySaleByPeriod(today, locationId);        
            return Json(dailySales, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RekapTenanBulananData(int no, int locationId)
        {
            today = today.AddMonths(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            IList<TenantBandaraMonthlySales> monthlySales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(period, locationId);
            return Json(monthlySales, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Previous(int no, int locationId)
        {           
            today = today.AddDays(no);  
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantBandaraDailySales> dailySales = _apRepo.FindTenantBandaraDailySaleByPeriod(today, locationId);
            IList<TenantBandaraDailySales> previousDay = _apRepo.FindTenantBandaraDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantBandaraDailySales> twoDaysBefore = _apRepo.FindTenantBandaraDailySaleByPeriod(_twoDaysBeforeDate, locationId);
            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = _apRepo.TotalTenanBandaraSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N")
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Next(int no, int locationId)
        {
            today=today.AddDays(no);           
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}{2}", today.Year, bulan, hari);
            var _previousDayDate = today.AddDays(-1);
            var _twoDaysBeforeDate = today.AddDays(-2);
            IList<TenantBandaraDailySales> dailySales = _apRepo.FindTenantBandaraDailySaleByPeriod(today, locationId);
            IList<TenantBandaraDailySales> previousDay = _apRepo.FindTenantBandaraDailySaleByPeriod(_previousDayDate, locationId);
            IList<TenantBandaraDailySales> twoDaysBefore = _apRepo.FindTenantBandaraDailySaleByPeriod(_twoDaysBeforeDate, locationId);
            SalesAmountDay salesAmount = new SalesAmountDay()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDR = _apRepo.TotalTenanBandaraSalePerDay(dailySales).ToString("N"),
                TotalSaleUSD = dailySales.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                Transactiondate = today.ToString("dd MMMM yyyy"),
                PreviousDay = _previousDayDate.ToString("dd MMMM yyyy"),
                TwoDaysBefore = _twoDaysBeforeDate.ToString("dd MMMM yyyy"),
                TotalSaleIDRPreviousDay = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDPreviousDay = previousDay.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N"),
                TotalSaleIDRTwoDaysBefore = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalePerTenan).ToString("N"),
                TotalSaleUSDTwoDaysBefore = twoDaysBefore.Sum<TenantBandaraDailySales>(y => y.TotalSalesPerTenantInUSD).ToString("N")                 
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NextMonth(int no, int locationId)
        {
            today = today.AddMonths(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year,  ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year,  ConvertMonth(today.AddMonths(-2)));
            IList<TenantBandaraMonthlySales> monthlySales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(period, locationId);
            IList<TenantBandaraMonthlySales> monthlyPreviousSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantBandaraMonthlySales> monthlyTwoBeforeSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDRCurrentMonth=_apRepo.TotalTenanBandaraSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreviousMonth(int no, int locationId)
        {
            today = today.AddMonths(no);
            GetDate();
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));
            IList<TenantBandaraMonthlySales> monthlySales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(period, locationId);
            IList<TenantBandaraMonthlySales> monthlyPreviousSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantBandaraMonthlySales> monthlyTwoBeforeSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(twoBeforeMonth, locationId);
            SalesAmountMonth salesAmount = new SalesAmountMonth()
            {
                CompanyName = "",
                LocationId = locationId,
                TotalSaleIDRCurrentMonth = _apRepo.TotalTenanBandaraSalePerMonth(monthlySales).ToString("N"),
                TotalSaleIDROneMonthBefore = monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                TotalSaleIDRTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N"),
                Transactiondate = today.ToString("MMMM yyyy"),
                OneMonthBefore = today.AddMonths(-1).ToString("MMMM yyyy"),
                TwoMonthBefore = today.AddMonths(-2).ToString("MMMM yyyy"),
                TotalSaleUSDCurrentMonth = monthlySales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
                TotalSaleUSDOneMonthBefore = monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
                TotalSaleUSDTwoMonthBefore = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N"),
            };
            return Json(salesAmount, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult RekapTenanBulanan(int locationId, string companyName)
        {
            GetMonth();
            period = string.Format("{0}{1}", today.Year, bulan);
            var previousMonth = string.Format("{0}{1}", today.AddMonths(-1).Year, ConvertMonth(today.AddMonths(-1)));
            var twoBeforeMonth = string.Format("{0}{1}", today.AddMonths(-2).Year, ConvertMonth(today.AddMonths(-2)));

            IList<TenantBandaraMonthlySales> monthlySales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(period, locationId);
            IList<TenantBandaraMonthlySales> monthlyPreviousSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(previousMonth, locationId);
            IList<TenantBandaraMonthlySales> monthlyTwoBeforeSales = _apRepo.FindTenantBandaraMonthlySaleByPeriod(twoBeforeMonth, locationId);
           
            ViewBag.Waktu = "Bulan ini";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;
           
            ViewBag.TotalSaleCurrentPerMonth=_apRepo.TotalTenanBandaraSalePerMonth(monthlySales).ToString("N");
		    ViewBag.TotalSalePreviousPerMonth=monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y=>y.MonthlyTotalSalePerTenan).ToString("N");
            ViewBag.TotalSaleTwoBeforePerMonth = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.MonthlyTotalSalePerTenan).ToString("N");

            ViewBag.TotalSaleCurrentPerMonthUSD = monthlySales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N");
            ViewBag.TotalSalePreviousPerMonthUSD = monthlyPreviousSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N");
            ViewBag.TotalSaleTwoBeforePerMonthUSD = monthlyTwoBeforeSales.Sum<TenantBandaraMonthlySales>(y => y.TotalSalePerTenantInUSD).ToString("N");
           
            return PartialView("../ReportSale/RekapPerTenan/AP/RekapPerTenanBulananAP", monthlySales);
        }
        
        public PartialViewResult RekapTenanTahunan(int locationId, string companyName)
        {           

            period = today.Year.ToString();
            IList<TenantBandaraYearlySales> yearlySales = _apRepo.FindTenantBandaraYearlySaleByPeriod(period, locationId);

            ViewBag.TotalSalePerYear = yearlySales.Sum<TenantBandaraYearlySales>(y => y.YearlyTotalSalePerTenan).ToString("N");
            ViewBag.TotalSalePerYearUSD = yearlySales.Sum<TenantBandaraYearlySales>(y => y.TotalSalePertenantInUSD).ToString("N2");
            ViewBag.Waktu = "Tahun ini";
            ViewBag.LocationId = locationId;
            ViewBag.CompanyName = companyName;
            ViewBag.Today = period;
            return PartialView("../ReportSale/RekapPerTenan/AP/RekapPerTenanTahunanAP", yearlySales);
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
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
    }
}
