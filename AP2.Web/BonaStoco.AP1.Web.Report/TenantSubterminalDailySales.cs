using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantSubTerminalDailySalesBySubTerminalAndTransactionDate", "select td.*, t.tenanname, mc.namecompany as CompanyName, mt.terminalname, ms.subterminalname from tenan t inner join tenantdailysales td on t.tenanid=td.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid inner join mappingsubterminal ms on t.subterminalid=ms.subterminalid where td.date=@transactionDate and td.subterminalid=@subterminalId")]
    public class TenantSubterminalDailySales:IViewModel
    {
        public int TenanId { get; set; }
        public int CompanyLocationId { get; set; }
        public string TransactionDate { get; set; }
        public decimal TotalSalePerTenan { get; set; }
        public decimal TotalSalesPerTenantInUSD {get;set;}
        public string TenanName { get; set; }
        public string CompanyName { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalname { get; set; }
    }
}
