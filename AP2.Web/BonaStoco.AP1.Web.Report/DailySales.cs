using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindDailySaleByTransactionDate", "select d.companylocationid as LocationId, c.namecompany as CompanyName,d.transactiondate as Transactiondate,d.totalsalepercompany as TotalSale, d.totalsalespercompanyinusd as TotalSaleInUSD from dailysales d inner join mappingcompany c on d.companylocationid=c.locationid where d.transactiondate=@datePeriod")]
    [NamedSqlQuery("FindDailySaleByTransactionDateAndCompanyLocatioId", "select d.companylocationid as LocationId, c.namecompany as CompanyName,d.transactiondate as Transactiondate,d.totalsalepercompany as TotalSale, d.totalsalespercompanyinusd as TotalSaleInUSD from dailysales d inner join mappingcompany c on d.companylocationid=c.locationid where d.date=@datePeriod AND d.companylocationid=@locationId")]
    public class DailySales:IViewModel
    {
        public int LocationId { get; set; }
        public string CompanyName { set; get; }
        public string Transactiondate { set; get; }
        public decimal TotalSale { set; get; }
        public decimal TotalSaleInUSD { set; get; }
    }
}