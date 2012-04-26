using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantTerminalMonthlySalesByTerminalAndMonthly", "select tm.*, t.tenanname, mc.namecompany, mt.terminalname  from tenan t inner join tenantmonthlysales tm on t.tenanid=tm.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid where tm.monthlyperiode=@monthPeriod and tm.terminalid=@terminalId")]
    public class TenantTerminalMonthlySales:IViewModel
    {
        public Int64 Id { get; set; }
        public int TenanId { get; set; }
        public string MonthlyPeriode { get; set; }
        public decimal MonthlyTotalSalePerTenan { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public string TenanName { get; set; }
        public string NameCompany { get; set; }
        public string TerminalName { get; set; }
    }
}
