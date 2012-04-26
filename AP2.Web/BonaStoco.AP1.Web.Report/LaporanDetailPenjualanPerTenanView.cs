using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindPenjualanByTenanIdAndDateInCategory", @"select i.transactiondate,i.tenanid,
	                            tenan.tenanname,i.companylocationid, i.transactionno, 
	                            case i.sellingpertransaction = 0 when true then i.sellingpertransactioninusd else i.sellingpertransaction end as sellingpertransaction,
                                case i.sellingpertransaction = 0 when true then 'USD' else 'IDR' end as ccy,
                                (select case sum(i.sellingpertransaction) = 0 when true then sum(i.sellingpertransactioninusd) else sum(i.sellingpertransaction) end as total from tenantdailysalesmonitoring i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai) as total
                                             from 
                                                tenantdailysalesmonitoring i inner join
				                                 tenan on i.tenanid = tenan.tenanid
				                             where 
                                                i.tenanid = @tenanid AND i.date between @dari AND @sampai")]
    [NamedSqlQuery("FindPenjualanByTenanIdAndDateInBandara", @"select i.transactiondate,i.tenanid,
	                            tenan.tenanname,i.companylocationid, i.transactionno, 
	                            case i.sellingpertransaction = 0 when true then i.sellingpertransactioninusd else i.sellingpertransaction end as sellingpertransaction,
                                case i.sellingpertransaction = 0 when true then 'USD' else 'IDR' end as ccy,
                                (select case sum(i.sellingpertransaction) = 0 when true then sum(i.sellingpertransactioninusd) else sum(i.sellingpertransaction) end as total from tenantdailysalesmonitoring i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.companylocationid = @locationid) as total
                                from 
                                     tenantdailysalesmonitoring i inner join
				                     tenan on i.tenanid = tenan.tenanid
				                where 
                                     i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.companylocationid = @locationid")]
    [NamedSqlQuery("FindPenjualanByTenanIdAndDateInTerminal", @"select i.transactiondate,i.tenanid,
	                            tenan.tenanname,i.companylocationid, i.transactionno, 
	                            case i.sellingpertransaction = 0 when true then i.sellingpertransactioninusd else i.sellingpertransaction end as sellingpertransaction,
                                case i.sellingpertransaction = 0 when true then 'USD' else 'IDR' end as ccy,
                                (select case sum(i.sellingpertransaction) = 0 when true then sum(i.sellingpertransactioninusd) else sum(i.sellingpertransaction) end as total from tenantdailysalesmonitoring i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.terminalid = @terminalid) as total
                                from 
                                     tenantdailysalesmonitoring i inner join
				                     tenan on i.tenanid = tenan.tenanid
				                where 
                                     i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.terminalid = @terminalid")]
    [NamedSqlQuery("FindPenjualanByTenanIdAndDateInSubTerminal", @"select i.transactiondate,i.tenanid,
	                            tenan.tenanname,i.companylocationid, i.transactionno, 
	                            case i.sellingpertransaction = 0 when true then i.sellingpertransactioninusd else i.sellingpertransaction end as sellingpertransaction,
                                case i.sellingpertransaction = 0 when true then 'USD' else 'IDR' end as ccy,
                                (select case sum(i.sellingpertransaction) = 0 when true then sum(i.sellingpertransactioninusd) else sum(i.sellingpertransaction) end as total from tenantdailysalesmonitoring i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.subterminalid = @subterminalid) as total
                                from 
                                     tenantdailysalesmonitoring i inner join
				                     tenan on i.tenanid = tenan.tenanid
				                where 
                                     i.tenanid = @tenanid AND i.date between @dari AND @sampai AND i.subterminalid = @subterminalid")]
    public class LaporanDetailPenjualanPerTenanView : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string TransactionDate { get; set; }
        public int CompanyLocationId {get; set;}
        public decimal SellingPerTransaction { get; set; }
        public string TransactionNo { get; set; }
        public string Ccy { get; set; }
        public decimal Total { get; set; }
    }
}
