using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IReportSaleRepository
    {
        IList<DailySales> FindDailySaleByPeriod(string datePeriod);
        decimal TotalSalePerDay(IList<DailySales> dailySales);
        IList<MonthlySales> FindMonthlySaleByPeriod(string monthPeriod);
        decimal TotalSalePerMonth(IList<MonthlySales> monthSales);
        IList<YearlySales> FindYearlySaleByPeriod(string yearPeriod);
        decimal TotalSalePerYear(IList<YearlySales> yearSales);
        IList<TenantDailySales> FindTenantDailySaleByPeriod(string datePeriod, string locationId);
        decimal TotalTenanSalePerDay(IList<TenantDailySales> tenanDailySales);
        IList<TenantMonthlySales> FindTenantMonthlySaleByPeriod(string monthPeriod, string locationId);
        decimal TotalTenanSalePerMonth(IList<TenantMonthlySales> tenanMonthlySales);
        IList<TenantYearlySales> FindTenantYearlySaleByPeriod(string yearPeriod, string locationId);
        decimal TotalTenanSalePerYear(IList<TenantYearlySales> tenanYearlySales);
    }
}
