using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantTerminalDailySalesByTerminalAndTransactionDate", "select td.*, t.tenanname, mc.namecompany, mt.terminalname  from tenan t inner join tenantdailysales td on t.tenanid=td.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid where td.date=@transactionDate and td.terminalid=@terminalId")]
    public class TenantTerminalDailySales:IViewModel
    {
        public Int64 Id { get; set; }
        public int TenanId { get; set; }
        public string TransactionDate { get; set; }
        public decimal TotalSalePerTenan { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public string TenanName { get; set; }
        public string NameCompany { get; set; }
        public string TerminalName { get; set; }
    }
}
