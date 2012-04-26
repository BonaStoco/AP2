using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTerminalYearlySalesByYearPeriod", "select ty.*, mt.terminalname from terminalyearlysales ty inner join mappingterminal mt on ty.terminalid=mt.terminalid where yearperiode=@yearPeriode and companylocationid=@companyid and ty.terminalid=@terminalid")]
    public class TerminalYearlySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public string YearPeriode { get; set; }
        public string TerminalName { get; set; }
    }
}
