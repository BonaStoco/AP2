using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.PengirimanBarang.Models
{
    [NamedSqlQuery("FindPendingGRNByTenanId",
        @"select grn.*, 
                 tenan.tenanname,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
          from
          grn inner join tenan on grn.tenanid = tenan.tenanid
          where grn.tenanid = @tenanid and grn.status = 0 and discriminator = @discriminator")]

    [NamedSqlQuery("FindAllPendingGRNByDiscriminator",@"select grn.*, 
                 tenan.tenanname,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
          from
          grn inner join tenan on grn.tenanid = tenan.tenanid
          where grn.status = 0 and discriminator = @discriminator")]

    [NamedSqlQuery("FindByGuid",
        @"select grn.*, 
                 tenan.tenanname ,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
          from
          grn inner join tenan on grn.tenanid = tenan.tenanid
          where grn.guid = @guid and grn.status = 0;")]

    [NamedSqlQuery("FindByDate",
        @"Select * from grn
                    where tanggaltransaksi between @'dari' AND @'sampai'")]

    [NamedSqlQuery("FindByGuidAllstatus",
        @"select grn.*, 
                 tenan.tenanname,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
          from
          grn inner join tenan on grn.tenanid = tenan.tenanid
          where grn.guid = @guid")]
    [NamedSqlQuery("DeleteGrn", @"Delete From grn where guid = @guid")]
    
    public class GRN : IViewModel
    {
        public Guid Guid { get; set; }
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string NamaPengirim { get; set; }
        public string CcyCode { get; set; }
        public string Referensi { get; set; }
        public string KodeTransaksi { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime TanggalTransaksi { get; set; }
        public string Keterangan { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
    }

    [NamedSqlQuery("FindByGRNId",
        @"select 
          i.*,
          p.barcode,
          p.kode,
          p.nama,
          p.hargajual,
          p.statusprint,
          p.tenanid,
          c.kode as ccycode,
          case when u.nama is null then '-' else u.nama end as unitname
          from 
          grnitem i inner join
          grn g on i.grnid = g.guid left outer join
          product p on lower(i.productguid) = lower(p.modelguid) inner join
          unit u on lower(u.modelguid) = lower(i.unitguid) left outer join
          ccy c on c.ccyid = p.ccyid
          where i.grnid = @grnid ORDER BY p.nama ASC;")]

    [NamedSqlQuery("FindByGuId",
       @"select 
          i.*,
          p.barcode,
          p.kode,
          p.nama,
          p.hargajual,
          p.statusprint,
          p.tenanid,
          c.kode as ccycode,
          case when u.nama is null then '-' else u.nama end as unitname
          from 
          grnitem i inner join
          grn g on i.grnid = g.guid inner join
          product p on lower(i.productguid) = lower(p.modelguid) inner join
          unit u on lower(u.modelguid) = lower(i.unitguid) inner join
          ccy c on c.ccyid = p.ccyid
          where i.guid = @guid;")]

    [NamedSqlQuery("DeleteItem","DELETE FROM grnitem WHERE guid = @id")]
    [NamedSqlQuery("DeleteGrnItem", "DELETE FROM grnitem WHERE grnid = @grnid")]
    public class GRNItem : IViewModel
    {
        public Guid GRNId { get; set; }
        public Guid Guid { get; set; }
        public Guid ProductGuid { get; set; }
        public string Barcode { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
        public decimal Qty { get; set; }
        public decimal ActualQty { get; set; }
        public Guid UnitGuid { get; set; }
        public string UnitName { get; set; }
        public decimal Harga { get; set; }
        public decimal HargaJual { get; set; }
        public decimal Jumlah { get; set; }
        public bool StatusPrint { get; set; }
        public string CcyCode { get; set; }
        public string Items { get; set; }
        public int TenanId { get; set; }

    }

    [NamedSqlQuery("FindPartGroup", "Select groupid as PartGroupId, nama as GroupName from partgroup where tenanid = @tenanid")]
    public class GetToComboBoxPartGroup : IViewModel
    {
        public int PartGroupId { get; set; }
        public string GroupName { get; set; }
    }

    [SqlQuery(@"Select ccyid as Ccyid, nama as CcyName from ccy")]
    public class GetToComboBoxCcy : IViewModel
    {
        public int Ccyid { get; set; }
        public string CcyName { get; set; }
    }

    [NamedSqlQuery("FindUnit", "Select unitid as UnitId, nama as UnitName from unit where tenanid = @tenanid")]
    public class GetToComboBoxUnit : IViewModel
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
    }
}