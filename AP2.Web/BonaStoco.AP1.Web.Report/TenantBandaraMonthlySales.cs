using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantMonthlySalesByBandaraAndMonth", "select tm.*, t.tenanname, mc.namecompany from tenan t inner join tenantmonthlysales tm on t.tenanid=tm.tenanid inner join mappingcompany mc on t.locationid=mc.locationid where tm.monthlyperiode=@monthPeriod and tm.companylocationid=@locationid")]
    public class TenantBandaraMonthlySales:IViewModel
    {
        public Int64 Id { get; set; }
        public int TenanId { get; set; }
        public string MonthlyPeriode { get; set; }
        public decimal MonthlyTotalSalePerTenan { get; set; }
        public int CategoryId { get; set; }
        public int CompanyLocationId { get; set; }
        public string TenanName { get; set; }
        public string NameCompany { get; set; }
        public decimal TotalSalePerTenantInUSD { get; set; }
    }
}
