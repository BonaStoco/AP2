using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface ITerminalReportSaleRepository
    {
        IList<TerminalDailySales> FindTerminalDailySalesByTransactionDate(DateTime transactionDate, int companyId, int terminalid);
        IList<TerminalMonthlySales> FindTerminalMonthlySalesByMonthPeriod(string monthPeriode, int companyId, int terminalid);
        IList<TerminalYearlySales> FindTerminalYearlySalesByMonthPeriod(string yearPeriode, int companyId, int terminalid);
        decimal TotalSalePerDay(IList<TerminalDailySales> dailySales);
        decimal TotalSalePerMonth(IList<TerminalMonthlySales> monthSales);
        decimal TotalSalePerYear(IList<TerminalYearlySales> yearSales);
        IList<TenantTerminalDailySales> FindTenantTerminalDailySaleByPeriod(string datePeriod, int terminalId);
        decimal TotalTenanTerminalSalePerDay(IList<TenantTerminalDailySales> tenanTerminalDailySales);
        IList<TenantTerminalMonthlySales> FindTenantTerminalMonthlySaleByPeriod(string monthPeriod, int terminalId);
        decimal TotalTenanTerminalSalePerMonth(IList<TenantTerminalMonthlySales> tenanTerminalMonthlySales);
        IList<TenantTerminalYearlySales> FindTenantTerminalYearlySaleByPeriod(string yearPeriod, int terminalId);
        decimal TotalTenanTerminalSalePerYear(IList<TenantTerminalYearlySales> tenanTerminalYearlySales);
    }
}
