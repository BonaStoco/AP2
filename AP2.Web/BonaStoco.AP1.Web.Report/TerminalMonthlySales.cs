using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTerminalMonthlySalesByMonthPeriod", "select tm.*, mt.terminalname from terminalmonthlysales tm inner join mappingterminal mt on tm.terminalid=mt.terminalid where monthperiode=@monthPeriode and companylocationid=@companyid and tm.terminalid=@terminalid")]
    public class TerminalMonthlySales:IViewModel
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public int TerminalId { get; set; }
        public decimal TotalSaleperCompany { get; set; }
        public string MonthPeriode { get; set; }
        public string TerminalName { get; set; }
    }
}
