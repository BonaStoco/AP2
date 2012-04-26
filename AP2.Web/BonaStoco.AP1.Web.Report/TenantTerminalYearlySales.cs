using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantTerminalYearlySalesByTerminalAndYear", "select ty.*, t.tenanname, mc.namecompany, mt.terminalname  from tenan t inner join tenantyearlysales ty on t.tenanid=ty.tenanid inner join mappingcompany mc on t.locationid=mc.locationid inner join mappingterminal mt on t.terminalid=mt.terminalid where ty.yearlyperiode=@yearPeriod and ty.terminalid=@terminalId")]
    public class TenantTerminalYearlySales:IViewModel
    {
        public Int64 Id { get; set; }
        public int TenanId { get; set; }
        public string YearlyPeriode { get; set; }
        public decimal YearlyTotalSalePerTenan { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public string TenanName { get; set; }
        public string NameCompany { get; set; }
        public string TerminalName { get; set; }
    }
}
