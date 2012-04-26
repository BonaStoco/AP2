using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTerminalDailySalesByTransactionDate", "select td.*, mt.terminalname from terminaldailysales td inner join mappingterminal mt on td.terminalid=mt.terminalid where date=@transactiondate and companylocationid=@companyid and td.terminalid=@terminalid")]
    public class TerminalDailySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public DateTime Date { get; set; }
        public string TerminalName { get; set; }
 
    }
}
