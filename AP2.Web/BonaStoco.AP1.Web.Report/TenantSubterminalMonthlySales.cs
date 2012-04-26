using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantSubTerminalMonthlySalesBySubTerminalAndMonthly", "select tm.*, t.tenanname, mc.namecompany as CompanyName, mt.terminalname, ms.subterminalname  from tenan t inner join tenantmonthlysales tm on t.tenanid=tm.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid inner join mappingsubterminal ms on t.subterminalid=ms.subterminalid where tm.monthlyperiode=@monthPeriod and tm.subterminalid=@subterminalId")]
    public class TenantSubterminalMonthlySales:IViewModel
    {
        public int TenanId { get; set; }
        public int CompanyLocationId { get; set; }
        public string MonthlyPeriode { get; set; }
        public decimal MonthlyTotalSalePerTenan { get; set; }
        public decimal MonthlyTotalSalesPerTenantInUSD { get; set; }
        public string TenanName { get; set; }
        public string CompanyName { get; set; }
        public string TerminalName { get; set; }
        public string SubTerminalName { get; set; }
    }
}
