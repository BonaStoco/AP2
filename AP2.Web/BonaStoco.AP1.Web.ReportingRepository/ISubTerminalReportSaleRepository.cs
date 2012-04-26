using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface ISubTerminalReportSaleRepository
    {
        IList<SubTerminalDailySales> FindSubTerminalDailySales(DateTime transactionDate, int subTerminalId, int locationId);
        IList<SubTerminalMonthlySales> FindSubTerminalMontlySales(string monthPeriode, int subTerminalId, int locationId);
        IList<SubTerminalYearlySales> FindSubTerminalYearlySales(string yearPeriode, int subTerminalId, int locationId);
        decimal TotalSalePerDay(IList<SubTerminalDailySales> dailySales);
        decimal TotalSalePerMonth(IList<SubTerminalMonthlySales> monthSales);
        decimal TotalSalePerYear(IList<SubTerminalYearlySales> yearSales);
        IList<TenantSubterminalDailySales> FindTenantSubTerminalDailySaleByPeriod(DateTime datePeriod, int subterminalId);
        decimal TotalTenanSubTerminalSalePerDay(IList<TenantSubterminalDailySales> tenanSubTerminalDailySales);
        IList<TenantSubterminalMonthlySales> FindTenantSubTerminalMonthlySaleByPeriod(string monthPeriod, int subterminalId);
        decimal TotalTenanSubTerminalSalePerMonth(IList<TenantSubterminalMonthlySales> tenanSubTerminalMonthlySales);
        IList<TenantSubterminalYearlySales> FindTenantSubTerminalYearlySaleByPeriod(string yearPeriod, int subterminalId);
        decimal TotalTenanSubTerminalSalePerYear(IList<TenantSubterminalYearlySales> tenanSubTerminalYearlySales);
    }
}
