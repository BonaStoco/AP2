using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class TerminalReportSaleRepository : ITerminalReportSaleRepository
    {
        decimal total = 0;
        QueryObjectMapper _QueryMapper;
        public TerminalReportSaleRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<TerminalDailySales> FindTerminalDailySalesByTransactionDate(DateTime transactionDate, int companyId, int terminalid)
        {
            IList<TerminalDailySales> _dailySales = _QueryMapper.Map<TerminalDailySales>("FindTerminalDailySalesByTransactionDate", new string[] { "transactiondate", "companyid", "terminalid" }, new object[] { transactionDate, companyId, terminalid }).ToList();
            return _dailySales;
        }

        public decimal TotalSalePerDay(IList<TerminalDailySales> dailySales)
        {
            foreach (var item in dailySales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<TerminalMonthlySales> FindTerminalMonthlySalesByMonthPeriod(string monthPeriode, int companyId, int terminalid)
        {
            IList<TerminalMonthlySales> _monthlySales = _QueryMapper.Map<TerminalMonthlySales>("FindTerminalMonthlySalesByMonthPeriod", new string[] { "monthPeriode", "companyid", "terminalid" }, new object[] { monthPeriode, companyId, terminalid }).ToList();
            return _monthlySales;
        }

        public decimal TotalSalePerMonth(IList<TerminalMonthlySales> monthSales)
        {
            foreach (var item in monthSales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<TerminalYearlySales> FindTerminalYearlySalesByMonthPeriod(string yearPeriode, int companyId, int terminalid)
        {
            IList<TerminalYearlySales> _yearlySales = _QueryMapper.Map<TerminalYearlySales>("FindTerminalYearlySalesByYearPeriod", new string[] { "yearPeriode", "companyid", "terminalid" }, new object[] { yearPeriode, companyId, terminalid }).ToList();
            return _yearlySales;
        }

        public decimal TotalSalePerYear(IList<TerminalYearlySales> yearSales)
        {
            foreach (var item in yearSales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<TenantTerminalDailySales> FindTenantTerminalDailySaleByPeriod(string datePeriod, int terminalId)
        {
            IList<TenantTerminalDailySales> _tenanDailySales = _QueryMapper.Map<TenantTerminalDailySales>("FindTenantTerminalDailySalesByTerminalAndTransactionDate", new string[] { "transactionDate", "terminalId" }, new object[] { datePeriod, terminalId }).ToList();
            return _tenanDailySales;
        }

        public decimal TotalTenanTerminalSalePerDay(IList<TenantTerminalDailySales> tenanTerminalDailySales)
        {
            foreach (var item in tenanTerminalDailySales)
            {
                total = total + item.TotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantTerminalMonthlySales> FindTenantTerminalMonthlySaleByPeriod(string monthPeriod, int terminalId)
        {
            IList<TenantTerminalMonthlySales> _tenanMonthlySales = _QueryMapper.Map<TenantTerminalMonthlySales>("FindTenantTerminalMonthlySalesByTerminalAndMonthly", new string[] { "monthPeriod", "terminalId" }, new object[] { monthPeriod, terminalId }).ToList();
            return _tenanMonthlySales;
        }

        public decimal TotalTenanTerminalSalePerMonth(IList<TenantTerminalMonthlySales> tenanTerminalMonthlySales)
        {
            foreach (var item in tenanTerminalMonthlySales)
            {
                total = total + item.MonthlyTotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantTerminalYearlySales> FindTenantTerminalYearlySaleByPeriod(string yearPeriod, int terminalId)
        {
            IList<TenantTerminalYearlySales> _tenanYearlySales = _QueryMapper.Map<TenantTerminalYearlySales>("FindTenantTerminalYearlySalesByTerminalAndYear", new string[] { "yearPeriod", "terminalId" }, new object[] { yearPeriod, terminalId }).ToList();
            return _tenanYearlySales;
        }

        public decimal TotalTenanTerminalSalePerYear(IList<TenantTerminalYearlySales> tenanTerminalYearlySales)
        {
            foreach (var item in tenanTerminalYearlySales)
            {
                total = total + item.YearlyTotalSalePerTenan;
            }
            return total;
        }

    }
}
