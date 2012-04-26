using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class SubTerminalReportSaleRepository : ISubTerminalReportSaleRepository
    {
        decimal total = 0;
        QueryObjectMapper _QueryMapper;
        public SubTerminalReportSaleRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<SubTerminalDailySales> FindSubTerminalDailySales(DateTime transactionDate, int subterminalId, int locationId)
        {
            IList<SubTerminalDailySales> _dailySales = _QueryMapper.Map<SubTerminalDailySales>("FindSubTerminalDailySales", new string[] { "transactionDate", "subterminalId", "locationId" }, new object[] { transactionDate, subterminalId, locationId }).ToList();
            return _dailySales;
        }

        public decimal TotalSalePerDay(IList<SubTerminalDailySales> dailySales)
        {
            foreach (var item in dailySales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<SubTerminalMonthlySales> FindSubTerminalMontlySales(string monthPeriode, int subTerminalId, int locationId)
        {
            IList<SubTerminalMonthlySales> _monthlySales = _QueryMapper.Map<SubTerminalMonthlySales>("FindSubTerminalMontlySales", new string[] { "monthPeriode", "subterminalId", "locationId" }, new object[] { monthPeriode, subTerminalId, locationId }).ToList();
            return _monthlySales;
        }

        public decimal TotalSalePerMonth(IList<SubTerminalMonthlySales> monthSales)
        {
            foreach (var item in monthSales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<SubTerminalYearlySales> FindSubTerminalYearlySales(string yearPeriode, int subTterminalId, int locationId)
        {
            IList<SubTerminalYearlySales> _yearlySales = _QueryMapper.Map<SubTerminalYearlySales>("FindSubTerminalYearlySales", new string[] { "yearPeriode", "subterminalId", "locationId" }, new object[] { yearPeriode, subTterminalId, locationId }).ToList();
            return _yearlySales;
        }

        public decimal TotalSalePerYear(IList<SubTerminalYearlySales> yearSales)
        {
            foreach (var item in yearSales)
            {
                total = total + item.TotalSaleperCompany;
            }
            return total;
        }

        public IList<TenantSubterminalDailySales> FindTenantSubTerminalDailySaleByPeriod(DateTime datePeriod, int subterminalId)
        {
            IList<TenantSubterminalDailySales> _tenanDailySales = _QueryMapper.Map<TenantSubterminalDailySales>("FindTenantSubTerminalDailySalesBySubTerminalAndTransactionDate", new string[] { "transactionDate", "subterminalId" }, new object[] { datePeriod, subterminalId}).ToList();
            return _tenanDailySales;
        }

        public decimal TotalTenanSubTerminalSalePerDay(IList<TenantSubterminalDailySales> tenanSubTerminalDailySales)
        {
            foreach (var item in tenanSubTerminalDailySales)
            {
                total = total + item.TotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantSubterminalMonthlySales> FindTenantSubTerminalMonthlySaleByPeriod(string monthPeriod, int subterminalId)
        {
            IList<TenantSubterminalMonthlySales> _tenanMonthlySales = _QueryMapper.Map<TenantSubterminalMonthlySales>("FindTenantSubTerminalMonthlySalesBySubTerminalAndMonthly", new string[] { "monthPeriod", "subterminalId" }, new object[] { monthPeriod, subterminalId }).ToList();
            return _tenanMonthlySales;
        }

        public decimal TotalTenanSubTerminalSalePerMonth(IList<TenantSubterminalMonthlySales> tenanSubTerminalMonthlySales)
        {
            foreach (var item in tenanSubTerminalMonthlySales)
            {
                total = total + item.MonthlyTotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantSubterminalYearlySales> FindTenantSubTerminalYearlySaleByPeriod(string yearPeriod, int subterminalId)
        {
            IList<TenantSubterminalYearlySales> _tenanYearlySales = _QueryMapper.Map<TenantSubterminalYearlySales>("FindTenantSubTerminalYearlySalesBySubTerminalAndYear", new string[] { "yearPeriod", "subterminalId" }, new object[] { yearPeriod, subterminalId }).ToList();
            return _tenanYearlySales;
        }

        public decimal TotalTenanSubTerminalSalePerYear(IList<TenantSubterminalYearlySales> tenanSubTerminalYearlySales)
        {
            foreach (var item in tenanSubTerminalYearlySales)
            {
                total = total + item.YearlyTotalSalePerTenan;
            }
            return total;
        }

    }
}
