using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenantAndMonthRange",@"select tenanid, date, totalsalepertenan, totaltransaction, totalsalespertenantinusd from tenantdailysales
                    where tenanid=@tenanId AND date between @dari and @sampai ORDER BY date ASC")]
    public class TenantSalesMonitoring:IViewModel
    {
        public int TenanId { get; set; }
        public DateTime Date { get; set; }
        public long TotalTransaction { get; set; }
        public decimal TotalSalePerTenan { get; set; }
        public decimal TotalSalesPerTenantInUSD { get; set; }
        public string DateString
        {
            get
            {
                return String.Format("{0:ddd, d MMM yyyy}", Date);
            }
        }
        public string DateStringParam
        {
            get
            {
                return String.Format("{0:yyyy-MM-dd}", Date);
            }
        }
    }
}
