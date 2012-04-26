using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindSubTerminalYearlySales", "select sy.*,mt.terminalname,ms.subterminalname from subterminalyearlysales sy inner join mappingterminal mt on sy.terminalid=mt.terminalid inner join mappingsubterminal ms on sy.subterminalid=ms.subterminalid where yearperiode=@yearPeriode and sy.subterminalid=@subterminalId and sy.companylocationid=@locationId")]
    public class SubTerminalYearlySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public string YearPeriode { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalName { get; set; }
    }
}
