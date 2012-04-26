using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindByDate", @"select t.tenanid, t.tenanname, tds.starttime, tds.endtime, tds.totalitem, tds.totaltransaction,
	                case totalsalepertenan=0 when true then totalsalespertenantinusd else totalsalepertenan end as totalsalespertenan,
	                case totalsalepertenan=0 when true then 'USD' else 'IDR' end as ccy
                    from tenantdailysales tds inner join tenan t on tds.tenanid=t.tenanid where tds.date=@date")]
    public class TenantDailySalesMonitoringEPOS:IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TotalItem { get; set; }
        public long TotalTransaction { get; set; }
        public decimal TotalSalesPerTenan { get; set; }
        public string Ccy { get; set; }
        public string StartTimeString
        {
            get { return string.Format("{0:HH:mm:ss}", StartTime); }
        }
        public string EndTimeString
        {
            get { return string.Format("{0:HH:mm:ss}", EndTime); }
        }
    }
}
