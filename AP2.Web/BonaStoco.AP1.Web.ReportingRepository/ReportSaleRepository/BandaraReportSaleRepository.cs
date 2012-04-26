using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class BandaraReportSaleRepository : IBandaraReportSaleRepository
    {
        decimal total = 0;
        QueryObjectMapper _QueryMapper;
        public BandaraReportSaleRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<DailySales> FindDailySaleByPeriodAndLocationId(DateTime datePeriod, int locationId)
        {
            IList<DailySales> _dailySales = _QueryMapper.Map<DailySales>("FindDailySaleByTransactionDateAndCompanyLocatioId", new string[] { "datePeriod", "locationId" }, new object[] { datePeriod, locationId }).ToList();
            return _dailySales;
        }
        public IList<MonthlySales> FindMonthlySaleByMonthPeriodAndLocationId(string monthPeriod, int locationId)
        {
            IList<MonthlySales> _monthlySales = _QueryMapper.Map<MonthlySales>("FindMonthlySaleByTransactionDateAndCompanyLocatioId", new string[] { "monthPeriod", "locationId" }, new object[] { monthPeriod, locationId }).ToList();
            return _monthlySales;
        }
        public IList<YearlySales> FindYearlySaleByYearPeriodAndLocationId(string yearPeriod, int locationId)
        {
            IList<YearlySales> _yearlySales = _QueryMapper.Map<YearlySales>("FindYearlySaleByTransactionDateAndCompanyLocatioId", new string[] { "yearPeriod", "locationId" }, new object[] { yearPeriod, locationId }).ToList();
            return _yearlySales;
        }

        public decimal TotalSalePerDay(IList<DailySales> dailySales)
        {
            foreach (var item in dailySales)
            {
                total = total + item.TotalSale;
            }
            return total;
        }

        public decimal TotalSalePerMonth(IList<MonthlySales> monthSales)
        {
            foreach (var item in monthSales)
            {
                total = total + item.TotalSale;
            }
            return total;
        }

        public decimal TotalSalePerYear(IList<YearlySales> yearSales)
        {
            foreach (var item in yearSales)
            {
                total = total + item.TotalSale;
            }
            return total;
        }

        public IList<TenantBandaraDailySales> FindTenantBandaraDailySaleByPeriod(DateTime datePeriod, int locationId)
        {
            IList<TenantBandaraDailySales> _dailySales = _QueryMapper.Map<TenantBandaraDailySales>("FindTenantDailySalesByBandaraAndTransactionDate", new string[] { "transactionDate", "locationid" }, new object[] { datePeriod.Date, locationId }).ToList();
            return _dailySales;
        }

        public decimal TotalTenanBandaraSalePerDay(IList<TenantBandaraDailySales> tenanBandaraDailySales)
        {
            foreach (var item in tenanBandaraDailySales)
            {
                total = total + item.TotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantBandaraMonthlySales> FindTenantBandaraMonthlySaleByPeriod(string monthPeriod, int locationId)
        {
            IList<TenantBandaraMonthlySales> _monthlySales = _QueryMapper.Map<TenantBandaraMonthlySales>("FindTenantMonthlySalesByBandaraAndMonth", new string[] { "monthPeriod", "locationid" }, new object[] { monthPeriod, locationId }).ToList();
            return _monthlySales;
        }

        public decimal TotalTenanBandaraSalePerMonth(IList<TenantBandaraMonthlySales> tenanBandaraMonthlySales)
        {
            foreach (var item in tenanBandaraMonthlySales)
            {
                total = total + item.MonthlyTotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantBandaraYearlySales> FindTenantBandaraYearlySaleByPeriod(string yearPeriod, int locationId)
        {
            IList<TenantBandaraYearlySales> _yearlySales = _QueryMapper.Map<TenantBandaraYearlySales>("FindTenantBandaraYearlySalesByBandaraAndYear", new string[] { "yearPeriod", "locationid" }, new object[] { yearPeriod, locationId }).ToList();
            return _yearlySales;
        }

        public decimal TotalTenanBandaraSalePerYear(IList<TenantBandaraYearlySales> tenanBandaraYearlySales)
        {
            foreach (var item in tenanBandaraYearlySales)
            {
                total = total + item.YearlyTotalSalePerTenan;
            }
            return total;
        }
    }
}
