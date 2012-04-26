using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindSubTerminalMontlySales", "select sm.*,mt.terminalname,ms.subterminalname from subterminalmonthlysales sm inner join mappingterminal mt on sm.terminalid=mt.terminalid inner join mappingsubterminal ms on sm.subterminalid=ms.subterminalid where monthperiode=@monthPeriode and sm.subterminalid=@subterminalId and sm.companylocationid=@locationId ")]
    public class SubTerminalMonthlySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public string MonthPeriode { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalName { get; set; }
    }
}
