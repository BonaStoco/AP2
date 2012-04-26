using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindTenantDailySalesByTransactionDate", "select t.*,  n.tenanname from tenantdailysales t inner join tenan n on t.tenanid=n.tenanid where t.transactiondate=@datePeriod AND companylocationid=@locationId")]
    public class TenantDailySales:IViewModel
    {
        public int TenanId { get; set; }
        public string CompanyLocationId { get; set; }
        public string TransactionDate { get; set; }
        public decimal TotalSalePerTenan { get; set; }
        public string TenanName { set; get; }
    }
}
