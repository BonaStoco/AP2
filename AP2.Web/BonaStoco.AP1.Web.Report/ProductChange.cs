using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindAllProductChangeByTenan",@"SELECT tenanid,kode,perubahan,tenanname, 
                                                    to_char(tanggal, 'dd/mm/yyyy') as tanggal 
                                                    FROM productchange 
                                                    WHERE tenanid = @tenanid
                                                    ORDER BY tanggal DESC limit @totalrow offset @currpage
                  ")]
    [NamedSqlQuery("FindProductChangeByTenanAndDate", 
                    @"SELECT tenanid,kode,perubahan,tenanname,
                        to_char(tanggal, 'dd/mm/yyyy') as tanggal 
                    FROM productchange WHERE tenanid=@tenanid 
                        AND to_char(tanggal, 'dd/mm/yyyy')=to_char(now(), 'dd/mm/yyyy')
                        ORDER BY tanggal DESC")]
    [NamedSqlQuery("FindProductChangeByTenanAndMonth", @"SELECT tenanid,kode,perubahan,tenanname, to_char(tanggal, 'mm/yyyy') as tanggal FROM productchange WHERE tenanid=@tenanid AND to_char(tanggal, 'mm-yyyy')=to_char(now(), 'mm-yyyy') ORDER BY tanggal DESC")]
    [NamedSqlQuery("FindProductChangeByTenanAndWeek", @"SELECT tenanid,kode,perubahan,tenanname, to_char(tanggal, 'dd/mm/yyyy') as tanggal FROM productchange WHERE tenanid=@tenanid AND tanggal BETWEEN @startdate AND @enddate ORDER BY tanggal DESC")]
    [NamedSqlQuery("CountAllProductChangeByTenan", @"SELECT tenanid,kode,perubahan,tenanname, to_char(tanggal, 'dd/mm/yyyy') as tanggal FROM productchange WHERE tenanid=@tenanid")]
    [Serializable]
    public class ProductChange : IViewModel
    {
        public int TenanId {get; set;}
        public string TenanName { get; set; }
        public string Tanggal { get; set; }
        public string Kode { get; set; }
        public string Perubahan { get; set; }
    }

    [SqlQuery("SELECT DISTINCT ON(tenanid) tenanid, tenanname, to_char(tanggal, 'dd/mm/yyyy') as tanggal FROM productchange ORDER BY tenanid")]
    [NamedSqlQuery("FindTenanByDate", @"SELECT DISTINCT ON(tenanid) tenanid, tenanname, to_char(tanggal, 'dd/mm/yyyy') as tanggal FROM productchange WHERE tanggal = @tanggal ORDER BY tenanid")]
    [NamedSqlQuery("FindAllTenanByWeek", @"SELECT DISTINCT ON(tenanid) tenanid, tenanname, to_char(tanggal, 'dd/mm/yyyy') as tanggal FROM productchange WHERE tanggal BETWEEN @startdate AND @enddate ORDER BY tenanid")]
    [Serializable]
    public class TenanProduct : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Tanggal { get; set; }
    }

    [NamedSqlQuery("FindAllProductByCode", @"SELECT p.kode, p.barcode, p.nama, p.hargajual, p.statusprint, c.kode as CcyKode FROM product p inner join ccy c on p.ccyid= c.ccyid WHERE p.kode = @kode and p.tenanid = @tenanid")]
    [Serializable]
    public class ProductPrint : IViewModel
    {
        public string Kode { get; set; }
        public string Barcode { get; set; }
        public string Nama {get; set;}
        public decimal HargaJual { get; set; }
        public bool StatusPrint {get;set;}
        public string CcyKode { get; set; }
    }
}
