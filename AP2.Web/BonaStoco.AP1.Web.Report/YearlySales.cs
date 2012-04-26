using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindYearlySalesByYear", "select y.companylocationid as LocationId, c.namecompany as CompanyName,y.yearlyperiode as YearlyPeriod,y.yearlytotalsalepercompany as TotalSale, totalsalepercompanyinusd as TotalSaleInUSD from yearlysales y inner join mappingcompany c on y.companylocationid=c.locationid where y.yearlyperiode=@yearPeriod")]
    [NamedSqlQuery("FindYearlySaleByTransactionDateAndCompanyLocatioId", "select y.companylocationid as LocationId, c.namecompany as CompanyName,y.yearlyperiode as YearlyPeriod,y.yearlytotalsalepercompany as TotalSale, totalsalepercompanyinusd as TotalSaleInUSD from yearlysales y inner join mappingcompany c on y.companylocationid=c.locationid where y.yearlyperiode=@yearPeriod AND y.companylocationid=@locationId")]
    public class YearlySales:IViewModel
    {
        public int LocationId { get; set; }
        public string CompanyName { set; get; }
        public string YearlyPeriod { set; get; }
        public decimal TotalSale { set; get; }
        public decimal TotalSaleInUSD { set; get; }
    }
}