using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantMonthlySalesByMonth", "select t.*,  n.tenanname from tenantmonthlysales t inner join tenan n on t.tenanid=n.tenanid where t.monthlyperiode=@monthPeriod AND companylocationid=@locationId")]
    public class TenantMonthlySales:IViewModel
    {
        public int TenanId { get; set; }
        public string CompanyLocationId { get; set; }
        public string MonthlyPeriode { get; set; }
        public decimal MonthlyTotalSalePerTenan { get; set; }
        public string TenanName { get; set; }
    }
}
