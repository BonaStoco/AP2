using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindFakturAPByPeriodeandTenanId", @"select b.id as Id,
                                        b.period as Period,
	                                    b.status as Status,
	                                    b.categoryId as CategoryId,
	                                    b.tenanId as TenanId,
	                                    b.nofaktur as NoFaktur,
	                                    b.nofakturpajak as NoFakturPajak,
	                                    b.ccycode,                                       
                                        b.totalpenjualan,
	                                    b.totalpenjualaninusd,
	                                    b.totalbagihasil,
	                                    b.totalbagihasilinusd,
						                (t.tarif / 100) as Tarif,
						                b.pajak as Pajak,
						                b.target,   
						                b.ttd as Ttd,
						                b.nip as Nip,
                                        b.konsesi,
						                b.penjualan,
						                b.bagihasil,
						                b.pajakbagihasil,
						                b.tagihan,
	                                    t.tenanname as TenanName,
	                                    t.alamat as Alamat,
	                                    t.npwp as Npwp,
                                        t.formulakonsesi	   
	                                    from billing b inner join tenan t on b.tenanid = t.tenanid 
                    where b.period=@period and b.tenanid=@tenanid")]
    
    [NamedSqlQuery("UpdateBilling", "UPDATE billing SET nofaktur= @nofaktur, ttd=@ttd,nip=@nip WHERE period=@periode and tenanid=@tenanid")]
    public class FakturAP:IViewModel
    {

        public long Id { get; set; }
        public string Period { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public long TenanId { get; set; }
        public string NoFaktur { get; set; }
        public string NoFakturPajak { get; set; }
        public string CcyCode { get; set; }          
        public decimal TotalPenjualan { get; set; }      
        public decimal TotalBagiHasil { get; set; }       
        public decimal Tarif { get; set; }
        public decimal Pajak { get; set; }
        public decimal Target { get; set; }  
        public decimal TotalPenjualanInUSD { get; set; }
        public decimal TotalBagiHasilInUSD { get; set; }       
        public string Ttd { get; set; }
        public string Nip { get; set; }
        public string TenanName { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public string FormulaKonsesi { get; set; }
        public decimal Konsesi { get; set; }
        public decimal Penjualan { get; set; }
        public decimal BagiHasil{ get; set; }
        public decimal PajakBagiHasil { get; set; }
        public decimal Tagihan{ get; set; }
    }

    [NamedSqlQuery("FindBillingDetailByPeriodeandTenanId", @"
select bd.id,
	                            bd.billingid,
	                            bd.tenanid, 
                                    b.ccycode,
	                            bd.dailysales,
                                   bd.dailysalesinusd, 
                                   case when b.ccycode='USD' then (bd.dailysales / exc.rate) + bd.dailysalesinusd else bd.dailysales  + (bd.dailysalesinusd * exc.rate) end as totalpenjualanharian,                               	
	                            bd.categoryid, 
	                            bd.bandaraid, 
	                            bd.terminalid,
	                            bd.subterminalid,
	                            bd.tarif, 
	                            bd.bagihasil,
	                            bd.pajak, 
	                            bd.pajakbagihasil, 
	                            bd.totaltagihan,
                                to_char(bd.date,'dd-month-yyyy') as TransactionDate 
              from billing b inner join billingdetail bd on b.id= bd.billingid left join (select eri.rate from exchangerateitem eri inner join exchangerate er on eri.modelguid=er.modelguid where date_trunc('day', date_trunc('day', now())) >= er.startdate and date_trunc('day', date_trunc('day', now())) <= er.enddate and eri.ccycode='USD' order by er.id desc limit 1) as exc on true  where b.period=@period and bd.tenanid=@tenanid")]

    [NamedSqlQuery("FindBillingDetailByPeriodeandBandaraId", @"select bd.id,
	                            bd.billingid,
	                            bd.tenanid, 
                                    b.ccycode,
	                            bd.dailysales,
                                   bd.dailysalesinusd, 
                                   case when b.ccycode='USD' then (bd.dailysales / exc.rate) + bd.dailysalesinusd else bd.dailysales  + (bd.dailysalesinusd * exc.rate) end as totalpenjualanharian,                               	
	                            bd.categoryid, 
	                            bd.bandaraid, 
	                            bd.terminalid,
	                            bd.subterminalid,
	                            bd.tarif, 
	                            bd.bagihasil,
	                            bd.pajak, 
	                            bd.pajakbagihasil, 
	                            bd.totaltagihan,
                                to_char(bd.date,'dd-month-yyyy') as TransactionDate 
              from billing b inner join billingdetail bd on b.id= bd.billingid left join (select eri.rate from exchangerateitem eri inner join exchangerate er on eri.modelguid=er.modelguid where date_trunc('day', date_trunc('day', now())) >= er.startdate and date_trunc('day', date_trunc('day', now())) <= er.enddate and eri.ccycode='USD' order by er.id desc limit 1) as exc on true where b.period=@period and bd.tenanid=@tenanid and bd.bandaraid=@bandaraid")]

    [NamedSqlQuery("FindBillingDetailByPeriodeandTerminalId", @"select bd.id,
	                            bd.billingid,
	                            bd.tenanid, 
                                    b.ccycode,
	                            bd.dailysales,
                                   bd.dailysalesinusd, 
                                   case when b.ccycode='USD' then (bd.dailysales / exc.rate) + bd.dailysalesinusd else bd.dailysales  + (bd.dailysalesinusd * exc.rate) end as totalpenjualanharian,                               	
	                            bd.categoryid, 
	                            bd.bandaraid, 
	                            bd.terminalid,
	                            bd.subterminalid,
	                            bd.tarif, 
	                            bd.bagihasil,
	                            bd.pajak, 
	                            bd.pajakbagihasil, 
	                            bd.totaltagihan,
                                to_char(bd.date,'dd-month-yyyy') as TransactionDate 
              from billing b inner join billingdetail bd on b.id= bd.billingid left join (select eri.rate from exchangerateitem eri inner join exchangerate er on eri.modelguid=er.modelguid where date_trunc('day', date_trunc('day', now())) >= er.startdate and date_trunc('day', date_trunc('day', now())) <= er.enddate and eri.ccycode='USD' order by er.id desc limit 1) as exc on true where b.period=@period and bd.tenanid=@tenanid  and bd.bandaraid=@bandaraid and bd.terminalid=@terminalid")]

    [NamedSqlQuery("FindBillingDetailByPeriodeandSubTerminalId", @"select bd.id,
	                            bd.billingid,
	                            bd.tenanid, 
                                    b.ccycode,
	                            bd.dailysales,
                                   bd.dailysalesinusd, 
                                   case when b.ccycode='USD' then (bd.dailysales / exc.rate) + bd.dailysalesinusd else bd.dailysales  + (bd.dailysalesinusd * exc.rate) end as totalpenjualanharian,                               	
	                            bd.categoryid, 
	                            bd.bandaraid, 
	                            bd.terminalid,
	                            bd.subterminalid,
	                            bd.tarif, 
	                            bd.bagihasil,
	                            bd.pajak, 
	                            bd.pajakbagihasil, 
	                            bd.totaltagihan,
                                to_char(bd.date,'dd-month-yyyy') as TransactionDate 
              from billing b inner join billingdetail bd on b.id= bd.billingid left join (select eri.rate from exchangerateitem eri inner join exchangerate er on eri.modelguid=er.modelguid where date_trunc('day', date_trunc('day', now())) >= er.startdate and date_trunc('day', date_trunc('day', now())) <= er.enddate and eri.ccycode='USD' order by er.id desc limit 1) as exc on true where b.period=@period and bd.tenanid=@tenanid and bd.bandaraid=@bandaraid and bd.terminalid=@terminalid and bd.subterminalid=@subterminalid")]

    public class DetailFakturAP : IViewModel
    {
        public long Id { get; set; }
        public long BillingId { get; set; }
        public long TenanId { get; set; }
        public string CcyCode { get; set; }
        public decimal DailySales { get; set; }
        public int CategoryId { get; set; }
        public int BandaraId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public decimal Tarif { get; set; }       
        public decimal DailySalesInUSD { get; set; }
        public decimal TotalPenjualanHarian { get; set; }
        public decimal Bagihasil { get; set; }
        public decimal Pajak { get; set; }
        public decimal PajakbagiHasil { get; set; }
        public decimal TotalTagihan { get; set; }
        public string TransactionDate { get; set; }
    }

    public class UpdateFakturAP : IViewModel
    {

        public long Id { get; set; }
        public string Period { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public long TenanId { get; set; }
        public string NoFaktur { get; set; }
        public string NoFakturPajak { get; set; }
        public string CcyCode { get; set; }
        public decimal TotalPenjualan { get; set; }
        public decimal TotalBagiHasil { get; set; }
        public decimal Tarif { get; set; }
        public decimal Pajak { get; set; }
        public decimal Target { get; set; }
        public decimal TotalPenjualanInUSD { get; set; }
        public decimal TotalBagiHasilInUSD { get; set; }
        public string Ttd { get; set; }
        public string Nip { get; set; }
        public string TenanName { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public string FormulaKonsesi { get; set; }
        public decimal Konsesi { get; set; }
        public decimal Penjualan { get; set; }
        public decimal BagiHasil { get; set; }
        public decimal PajakBagiHasil { get; set; }
        public decimal Tagihan { get; set; }  
    }

}
