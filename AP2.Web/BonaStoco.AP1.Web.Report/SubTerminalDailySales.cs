using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindSubTerminalDailySales", "select sd.*,mt.terminalname,ms.subterminalname from subterminaldailysales sd inner join mappingterminal mt on sd.terminalid=mt.terminalid inner join mappingsubterminal ms on sd.subterminalid=ms.subterminalid where date=@transactionDate and sd.subterminalid=@subterminalId and sd.companylocationid=@locationId ")]
    public class SubTerminalDailySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public DateTime Date { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalName { get; set; }
    }
}
