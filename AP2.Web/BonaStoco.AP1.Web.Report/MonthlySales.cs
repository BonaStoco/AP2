using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindmontlySalesByMonth", "select m.companylocationid as LocationId, c.namecompany as CompanyName,m.monthlyperiode as MonthlyPeriod,m.monthlytotalsalepercompany as TotalSale, monthlytotalsalepercompanyinusd as TotalSaleInUSD from monthlysales m inner join mappingcompany c on m.companylocationid=c.locationid where m.monthlyperiode=@monthPeriod")]
    [NamedSqlQuery("FindMonthlySaleByTransactionDateAndCompanyLocatioId", "select m.companylocationid as LocationId, c.namecompany as CompanyName,m.monthlyperiode as MonthlyPeriod,m.monthlytotalsalepercompany as TotalSale, monthlytotalsalepercompanyinusd as TotalSaleInUSD from monthlysales m inner join mappingcompany c on m.companylocationid=c.locationid where m.monthlyperiode=@monthPeriod AND m.companylocationid=@locationId")]
    public class MonthlySales:IViewModel
    {
        public int LocationId { get; set; }
        public string CompanyName { set; get; }
        public string MonthlyPeriod { set; get; }
        public decimal TotalSale { set; get; }
        public decimal TotalSaleInUSD { set; get; }

    }
}