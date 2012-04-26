using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BonaStoco.AP1.MasterData.Models
{

    [SqlQuery(@"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname, t.tenanname
          FROM 
          requestproduct p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid inner join tenan t on p.tenanid=t.tenanid where p.status=0 order by p.tenanid asc limit 100")]

    [NamedSqlQuery("FindAllProductPendingByTenanId", @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname, t.tenanname,p.kode as Code,p.barcode as Barcode
          FROM 
          requestproduct p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid inner join tenan t on p.tenanid=t.tenanid where p.status=0 AND p.tenanid=@tenanid order by p.tenanid  asc limit 100")]
   

    [NamedSqlQuery("FindAllProductAproveByGuidId", @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname, t.tenanname
          FROM 
          requestproduct p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid inner join tenan t on p.tenanid=t.tenanid where p.modelguid=@guidid")]

    [NamedSqlQuery("FindByStatusAndTenanId", @"select p.*,g.nama as groupname, c.nama as ccyname, u.nama as unitname, t.tenanname
                                    FROM requestproduct p
                                        inner join partgroup g on g.groupid=p.groupid 
                                        inner join unit u on u.unitid=p.unitid 
                                        inner join ccy c on c.ccyid=p.ccyid
                                        inner join tenan t on t.tenanid = p.tenanid
                                    WHERE p.tenanid=@tenanid and p.createddate between @dari and @sampai")]
    [NamedSqlQuery("FindProductApprovedAndPendingByTenanId", @"select p.*,g.nama as groupname, c.nama as ccyname, u.nama as unitname, t.tenanname
                                    FROM requestproduct p
                                        inner join partgroup g on g.groupid=p.groupid 
                                        inner join unit u on u.unitid=p.unitid 
                                        inner join ccy c on c.ccyid=p.ccyid
                                        inner join tenan t on t.tenanid = p.tenanid
                                    WHERE p.tenanid=@tenanid and p.status IN (0,1)")]
//    [NamedSqlQuery("FindByBarcodeOrCode",
//        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
//          FROM 
//          aprovepart p inner join 
//           partgroup g on p.groupid = g.groupid inner join 
//            ccy c on p.ccyid = c.ccyid inner join 
//             unit u on p.unitid = u.unitid 
//          where p.tenanid=@tenanid AND (barcode=@code or p.kode=@code)")]
//    [NamedSqlQuery("FindById",
//        @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
//          FROM 
//          aprovepart p inner join 
//           partgroup g on p.groupid = g.groupid inner join 
//            ccy c on p.ccyid = c.ccyid inner join 
//             unit u on p.unitid = u.unitid
//          where p.tenanid=@tenanid AND productid=@productid")]
    public class RequestProduct : ModelBase
    {
        [Required(ErrorMessage = "Masukkan kode barang")]
        public override string Kode { get; set; }

        [Required(ErrorMessage = "Nama barang harus diisi")]
        public override string Nama { get; set; }

        public long ProductId { get; set; }

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

        public string TenanName { get; set; }

        public int Status { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
    }
    
}