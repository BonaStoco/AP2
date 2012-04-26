using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("findByTenanAndPeriode", @"select b.id as Id,
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
	                                    t.npwp as Npwp	   
	                                    from billing b inner join tenan t on b.tenanid = t.tenanid
 where b.period = @periode and b.tenanid = @tenanid")]
    [NamedSqlQuery("UpdateNoFakturByTenan", @"UPDATE billing SET nofakturpajak=@nofakturpajak WHERE tenanid=@tenanid and period=@periode")]
    public class FakturPajak : IViewModel
    {
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
        public decimal Konsesi { get; set; }
        public decimal Penjualan { get; set; }
        public decimal BagiHasil { get; set; }
        public decimal PajakBagiHasil { get; set; }
        public decimal Tagihan { get; set; }
    }
}