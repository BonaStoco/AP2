using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindAllProductByCode", @"SELECT p.kode, p.barcode, p.nama, p.hargajual, p.statusprint, c.kode as CcyKode FROM product p inner join ccy c on p.ccyid= c.ccyid WHERE lower(p.kode) = lower(@kode) and p.tenanid = @tenanid")]
    [Serializable]
    public class ProductPrint : IViewModel
    {
        public string Kode { get; set; }
        public string Barcode { get; set; }
        public string Nama { get; set; }
        public decimal HargaJual { get; set; }
        public bool StatusPrint { get; set; }
        public string CcyKode { get; set; }
    }
}