using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindByTransactionNo", @"select s.transactionno,s.kodeproduk,s.namaproduk,s.qty as qty,
    case s.hargajualidr = 0 when true then s.hargajualusd else s.hargajualidr end as hargajual,
    case s.hargajualidr = 0 when true then 'USD' else 'IDR' end as ccy,
    case s.discountidr = 0 when true then s.discountusd else s.discountidr end as DiscountItemAmount,
    case s.netamountidr = 0 when true then s.netamountusd else s.netamountidr end as NetAmount,
    case t.discountidr = 0 when true then t.discountusd else t.discountidr end as DiscountTotalAmount,
    case t.chargeidr = 0 when true then t.chargeusd else t.chargeidr end as ChargeAmount,
    case t.taxidr = 0 when true then t.taxusd else t.taxidr end as TaxAmount,
    case t.sellingpertransaction = 0 when true then t.sellingpertransactioninusd else t.sellingpertransaction end as TotalAmount,
    case t.servicechargeidr=0 when true then t.servicechargeusd else t.servicechargeidr end as ServiceCharge
     from salesproductdetail s

    left join tenantdailysalesmonitoring t on t.transactionno = s.transactionno and t.tenanid = s.tenanid
    where transactiontype IN(0,1) 
    and s.transactionno=@transactionNo and s.tenanid=@tenanId")]
    public class SalesProductDetail:IViewModel
    {
        public string TransactionNo { get; set; }
        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }
        public decimal HargaJual { get; set; }
        public int Qty { get; set; }
        public string Ccy { get; set; }
        public decimal DiscountItemAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal DiscountTotalAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ServiceCharge { get; set; }
    }

    [NamedSqlQuery("FindSummaryByTenantAndDate", @"
select s.kodeproduk,s.namaproduk,sum(s.qty + case r.qty isnull when true then 0 else r.qty end) as qty,
        s.hargajual,
        s.ccy,
        s.servicecharge
        from 
            (select kodeproduk,kasir,sessionid,namaproduk,qty,transactiontype, transactiondate, tenanid,
             case hargajualidr = 0 when true then hargajualusd else hargajualidr end as hargajual,
             case hargajualidr = 0 when true then 'USD' else 'IDR' end as ccy,
             case servicechargeitemidr=0 when true then servicechargeitemusd else servicechargeitemidr end as servicecharge
		     from salesproductdetail) s
        left join
            (select kasir,transactionno,kodeproduk,namaproduk, qty,sessionid,transactiondate,
             case hargajualidr = 0 when true then 'USD' else 'IDR' end as ccy
             from salesproductdetail
	         where transactiontype=1) r on r.kasir=s.kasir and r.kodeproduk=s.kodeproduk and r.ccy = s.ccy
	         and r.namaproduk=s.namaproduk and r.sessionid=s.sessionid
	    where transactiontype=0 and s.tenanid = @tenanId and date(s.transactiondate) = @transactionDate
	    and (s.qty + case r.qty isnull when true then 0 else r.qty end) > 0
	    group by s.kodeproduk, s.namaproduk, s.hargajual, s.ccy,s.servicecharge")]
    public class SalesSummaryProduct : IViewModel
    {
        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }
        public long Qty { get; set; }
        public decimal HargaJual { get; set; }
        public string Ccy { get; set; }
        public decimal ServiceCharge { get; set; }
    }

    [NamedSqlQuery("FindSessionByTenantAndDate", "select distinct(sessionid),kasir from salesproductdetail where tenanid=@tenanId and date(transactiondate)=@tanggal")]
    public class SessionSummaryPerKasir : IViewModel
    {
        public string Kasir { get; set; }
        public long SessionId { get; set; }
    }
    [NamedSqlQuery("FindSummaryPerkasirByDateAndTenan", @"
                    select s.kodeproduk,s.kasir,s.sessionid,s.namaproduk,sum(s.qty + case r.qty isnull when true then 0 else r.qty end) as qty,
                           s.hargajual,
                           s.ccy,
                           s.servicecharge
                           from 
                           (select s.kodeproduk,s.kasir,s.sessionid,s.namaproduk, s.qty, s.transactiontype, s.transactiondate, s.tenanid,
                           case s.hargajualidr = 0 when true then s.hargajualusd else s.hargajualidr end as hargajual,
                           case s.hargajualidr = 0 when true then 'USD' else 'IDR' end as ccy,
                           case servicechargeitemidr=0 when true then servicechargeitemusd else servicechargeitemidr end as servicecharge
		                    from salesproductdetail s) s
		                    left join
		                    (select kasir,transactionno,kodeproduk,namaproduk, qty,sessionid,transactiondate,
		                    case hargajualidr = 0 when true then 'USD' else 'IDR' end as ccy
		                    from salesproductdetail
		                    where transactiontype=1) r on r.kasir=s.kasir and r.kodeproduk=s.kodeproduk and s.ccy=r.ccy
		                    and r.namaproduk=s.namaproduk and r.sessionid=s.sessionid and date(r.transactiondate)=date(s.transactiondate)
	                    where transactiontype=0 and s.tenanid = @tenanId and date(s.transactiondate) = @tanggal
	                    and (s.qty + case r.qty isnull when true then 0 else r.qty end)>0 
	                    and s.sessionid = @sessionId
	                    group by s.kodeproduk, s.namaproduk, s.kasir, s.sessionid, s.hargajual,s.ccy,s.servicecharge")]
    public class SummaryPerKasir : IViewModel
    {
        public string Kasir { get; set; }
        public long SessionId { get; set; }
        public string KodeProduk { get; set; }
        public string NamaProduk { get; set; }
        public long Qty { get; set; }
        public decimal HargaJual { get; set; }
        public string Ccy { get; set; }
        public decimal ServiceCharge { get; set; }
    }
}
