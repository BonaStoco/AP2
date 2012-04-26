using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IBandaraReportSaleRepository
    {
        IList<DailySales> FindDailySaleByPeriodAndLocationId(DateTime datePeriod, int locationId);
        IList<MonthlySales> FindMonthlySaleByMonthPeriodAndLocationId(string monthPeriod, int locationId);
        IList<YearlySales> FindYearlySaleByYearPeriodAndLocationId(string yearPeriod, int locationId);
        decimal TotalSalePerDay(IList<DailySales> dailySales);
        decimal TotalSalePerMonth(IList<MonthlySales> monthSales);
        decimal TotalSalePerYear(IList<YearlySales> yearSales);
        IList<TenantBandaraDailySales> FindTenantBandaraDailySaleByPeriod(DateTime datePeriod, int locationId);
        decimal TotalTenanBandaraSalePerDay(IList<TenantBandaraDailySales> tenanBandaraDailySales);
        IList<TenantBandaraMonthlySales> FindTenantBandaraMonthlySaleByPeriod(string monthPeriod, int locationId);
        decimal TotalTenanBandaraSalePerMonth(IList<TenantBandaraMonthlySales> tenanBandaraMonthlySales);
        IList<TenantBandaraYearlySales> FindTenantBandaraYearlySaleByPeriod(string yearPeriod, int locationId);
        decimal TotalTenanBandaraSalePerYear(IList<TenantBandaraYearlySales> tenanBandaraYearlySales);
    }
}
