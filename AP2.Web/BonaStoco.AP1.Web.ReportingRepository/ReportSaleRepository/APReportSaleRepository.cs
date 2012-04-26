using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class APReportSaleRepository : IReportSaleRepository
    {
        decimal total=0;
        QueryObjectMapper _QueryMapper;
        IList<DailySales> _dailySales;
        IList<MonthlySales> _monthlySales;
        IList<YearlySales> _yearlySales;
        IList<TenantDailySales> _tenantDailySales;
        IList<TenantMonthlySales> _tenanhtMonthlySales;
        IList<TenantYearlySales> _tenantYearlySales;

        public APReportSaleRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<DailySales> FindDailySaleByPeriod(string datePeriod)
        {
            _dailySales = _QueryMapper.Map<DailySales>("FindDailySaleByTransactionDate",new string[]{"datePeriod"},new object[]{datePeriod}).ToList();
            return _dailySales;
        }
        public IList<MonthlySales> FindMonthlySaleByPeriod(string monthPeriod)
        {
            _monthlySales = _QueryMapper.Map<MonthlySales>("FindmontlySalesByMonth",new string[]{"monthPeriod"},new object[]{monthPeriod}).ToList();
            return _monthlySales;
        }
        public IList<YearlySales> FindYearlySaleByPeriod(string yearPeriod)
        {
            _yearlySales = _QueryMapper.Map<YearlySales>("FindYearlySalesByYear",new string[]{"yearPeriod"},new object[]{yearPeriod}).ToList();
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
        public decimal TotalTenanSalePerDay(IList<TenantDailySales> tenanDailySales)
        {
            foreach (var item in tenanDailySales)
            {
                total = total + item.TotalSalePerTenan;
            }
            return total;
        }
        public decimal TotalTenanSalePerMonth(IList<TenantMonthlySales> tenanMonthlySales)
        {
            foreach (var item in tenanMonthlySales)
            {
                total = total + item.MonthlyTotalSalePerTenan;
            }
            return total;
        }
        public decimal TotalTenanSalePerYear(IList<TenantYearlySales> tenanYearlySales)
        {
            foreach (var item in tenanYearlySales)
            {
                total = total + item.YearlyTotalSalePerTenan;
            }
            return total;
        }

        public IList<TenantDailySales> FindTenantDailySaleByPeriod(string datePeriod, string locationId)
        {
            _tenantDailySales = _QueryMapper.Map<TenantDailySales>("FindTenantDailySalesByTransactionDate", new string[] { "datePeriod", "locationId" }, new object[] { datePeriod, locationId }).ToList();
            return _tenantDailySales;
        }
        public IList<TenantMonthlySales> FindTenantMonthlySaleByPeriod(string monthPeriod, string locationId)
        {
            _tenanhtMonthlySales = _QueryMapper.Map<TenantMonthlySales>("FindTenantMonthlySalesByMonth", new string[] { "monthPeriod", "locationId" }, new object[] { monthPeriod, locationId }).ToList();
            return _tenanhtMonthlySales;
        }
        public IList<TenantYearlySales> FindTenantYearlySaleByPeriod(string yearPeriod, string locationId)
        {
            _tenantYearlySales = _QueryMapper.Map<TenantYearlySales>("FindTenantYearlySalesByYear", new string[] { "yearPeriod", "locationId" }, new object[] { yearPeriod, locationId }).ToList();
            return _tenantYearlySales;
        }
        //public IList<DailySales> FindDailySaleByPeriodAndLocationId(string datePeriod, string locationId)
        //{
        //    _dailySales = _QueryMapper.Map<DailySales>("FindDailySaleByTransactionDateAndCompanyLocatioId", new string[] { "datePeriod","locationId" }, new object[] { datePeriod,locationId }).ToList();
        //    return _dailySales;
        //}
        //public IList<MonthlySales> FindMonthlySaleByMonthPeriodAndLocationId(string monthPeriod, string locationId)
        //{
        //    _monthlySales = _QueryMapper.Map<MonthlySales>("FindMonthlySaleByTransactionDateAndCompanyLocatioId", new string[] { "monthPeriod", "locationId" }, new object[] { monthPeriod, locationId }).ToList();
        //    return _monthlySales;
        //}
        //public IList<YearlySales> FindYearlySaleByYearPeriodAndLocationId(string yearPeriod, string locationId)
        //{
        //    _yearlySales = _QueryMapper.Map<YearlySales>("FindYearlySaleByTransactionDateAndCompanyLocatioId", new string[] { "yearPeriod", "locationId" }, new object[] { yearPeriod, locationId }).ToList();
        //    return _yearlySales;
        //}
    
    }
}
