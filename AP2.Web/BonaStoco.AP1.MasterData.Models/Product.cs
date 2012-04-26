using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenanId",
        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid 
          where p.tenanid=@tenanid")]
    [NamedSqlQuery("FindByBarcodeOrCode",
        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid 
          where p.tenanid=@tenanid AND (LOWER(barcode)=LOWER(@code) or LOWER(p.kode)=LOWER(@code))")]
    [NamedSqlQuery("FindById",
        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid
          where p.tenanid=@tenanid AND productid=@productid")]
    [NamedSqlQuery("FindByCode",
            @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid 
          where p.tenanid=@tenanid AND LOWER(p.kode)=LOWER(@code)")]
    [NamedSqlQuery("FindByGroup",
        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid
          where p.tenanid=@tenanid AND p.groupid=@groupid")]

    [NamedSqlQuery("FindSearchProductByName",
        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid
          where p.tenanid=@tenanid and LOWER(p.nama) like @name limit 20")]
    public class Product : ModelBase
    {
        [Required(ErrorMessage = "Masukkan kode barang")]
        public override string Kode { get; set; }

        [Required(ErrorMessage = "Nama barang harus diisi")]
        public override string Nama { get; set; }

        public int ProductId { get; set; }

        [Required(ErrorMessage="Group harus diisi")]
        [DisplayName("Group")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Currency harus diisi")]
        [DisplayName("Mata uang")]
        public int CcyId { get; set; }

        [Required(ErrorMessage = "Unit harus diisi")]
        [DisplayName("Satuan")]
        public int UnitId { get; set; }
        
        [Required(ErrorMessage="Barcode tidak boleh kosong")]
        public string Barcode { get; set; }

        [Required(ErrorMessage="Massukkan harga beli")]
        [DisplayName("Harga beli")]
        public decimal HargaBeli { get; set; }

        [Required(ErrorMessage = "Massukkan harga jual")]
        [DisplayName("Harga jual")]
        public decimal HargaJual { get; set; }

        [DisplayName("Group")]
        public string GroupName { get; private set; }
        [DisplayName("Mata Uang")]
        public string CcyName { get; private set; }
        [DisplayName("Satuan")]
        public string UnitName { get; private set; }

        [DisplayName("Status Print")]
        public bool StatusPrint { get; set; }

        [DisplayName("Status Product")]
        public bool StatusProduct { get; set; }

    }
}