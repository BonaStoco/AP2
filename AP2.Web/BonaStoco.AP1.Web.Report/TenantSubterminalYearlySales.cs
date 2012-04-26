using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantSubTerminalYearlySalesBySubTerminalAndYear", "select ty.*, t.tenanname, mc.namecompany as CompanyName, mt.terminalname, ms.subterminalname  from tenan t inner join tenantyearlysales ty on t.tenanid=ty.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid inner join mappingsubterminal ms on t.subterminalid=ms.subterminalid where ty.yearlyperiode=@yearPeriod and ty.subterminalid=@subterminalId")]
    public class TenantSubterminalYearlySales:IViewModel
    {
        public int TenanId { get; set; }
        public int CompanyLocationId { get; set; }
        public string YearlyPeriode { get; set; }
        public decimal YearlyTotalSalePerTenan { get; set; }
        public decimal TotalSalesPerTenanInUSD { get; set; }
        public string TenanName { get; set; }
        public string CompanyName { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalName { get; set; }
    }
}
