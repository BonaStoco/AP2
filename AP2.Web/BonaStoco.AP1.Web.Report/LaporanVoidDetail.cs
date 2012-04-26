using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
        [NamedSqlQuery("FindDetailVoidPerKasirByDate", @"select s.kasir, 
                                                                s.transactiondate, 
                                                                s.transactionno, 
                                                                s.namaproduk, 
                                                                s.qty, 
                                                                s.hargajualidr, 
                                                                s.hargajualusd, 
                                                                t.sellingpertransaction as netamountidr, 
                                                                t.sellingpertransactioninusd as netamountusd 
                                                        from salesproductdetail s left join 
                                                                tenantdailysalesmonitoring t on t.transactionno = s.transactionno and t.tenanid = s.tenanid
                                                        where transactiontype=1 and s.tenanid = @tenanid and s.sessionid=@sessionid and date(s.transactiondate) between @dari and @sampai and s.qty < 0
                                                        order by s.transactiondate")]

        public class LaporanVoidDetail : IViewModel
        {
            public string Kasir { get; set; }
            public DateTime TransactionDate { get; set; }
            public string TransactionNo { get; set; }
            public string NamaProduk { get; set; }
            public int Qty { get; set; }
            public decimal HargaJualIDR { get; set; }
            public decimal HargaJualUSD { get; set; }
            public decimal NetAmountIDR { get; set; }
            public decimal NetAmountUSD { get; set; }
        }

        [NamedSqlQuery("FindSummaryVoidPerKasirByDate", @" select s.kasir, 
	                                                                s.transactiondate, 
	                                                                t.transactionno, 
	                                                                (select sum(t.sellingpertransaction)as netamountidr 
	                                                                    from tenantdailysalesmonitoring t left join 
	                                                                        salesproductdetail s on s.transactionno = t.transactionno and s.tenanid = t.tenanid 
	                                                                    where transactiontype=1 and s.tenanid = @tenanid and s.sessionid=@sessionid and date(s.transactiondate) between @dari and @sampai and s.qty < 0)as netamountidr,
	                                                                (select sum(t.sellingpertransactioninusd)as netamountusd 
	                                                                    from tenantdailysalesmonitoring t left join 
	                                                                        salesproductdetail s on s.transactionno = t.transactionno and s.tenanid = t.tenanid 
	                                                                    where transactiontype=1 and s.tenanid = @tenanid and s.sessionid=@sessionid and date(s.transactiondate) between @dari and @sampai and s.qty < 0)as netamountusd
                                                                from tenantdailysalesmonitoring t left join 
	                                                                    salesproductdetail s on s.transactionno = t.transactionno and s.tenanid = t.tenanid
                                                                where transactiontype=1 and s.tenanid = @tenanid and s.sessionid=@sessionid and date(s.transactiondate) between @dari and @sampai and s.qty < 0
                                                                group by s.transactiondate, s.kasir, t.transactionno, t.sellingpertransactioninusd ")]

        public class LaporanVoidSummary : IViewModel
        {
            public string Kasir { get; set; }
            public DateTime TransactionDate { get; set; }
            public string TransactionNo { get; set; }
            public decimal NetAmountIDR { get; set; }
            public decimal NetAmountUSD { get; set; }
        }

    [NamedSqlQuery("FindSessionByTenantAndDate", "select distinct(sessionid),kasir from salesproductdetail where tenanid=@tenanId and date(transactiondate)between @dari and @sampai")]
    public class SessionKasir : IViewModel
    {
        public string Kasir { get; set; }
        public long SessionId { get; set; }
    }
}
