using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantYearlySalesByYear", "select t.*, n.tenanname from tenantyearlysales t inner join tenan n on t.tenanid=n.tenanid where t.yearlyperiode=@yearPeriod AND companylocationid=@locationId")]
    public class TenantYearlySales:IViewModel
    {
        public int TenanId { get; set; }
        public string CompanyLocationId { get; set; }
        public string YearlyPeriode { get; set; }
        public decimal YearlyTotalSalePerTenan { get; set; }
        public string TenanName { get; set; }
    }
}
