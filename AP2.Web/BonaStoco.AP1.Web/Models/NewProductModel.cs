using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BonaStoco.AP1.Web.Models
{
    public class NewProductModel
    {
        [Key]
        public int PartGroup { get; set; }
        public Guid ProductGuid { get; set; }
        public string PartGroupName { get; set; }
        public string Barcode { get; set; }
        public string Kode { get; set; }
        public string NamaBArang { get; set; }
        public int CcyId { get; set; }
        public string CcyName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal HargaBeli { get; set; }
        public decimal HargaJual { get; set; }
        public decimal Qty { get; set; }
        public int ProductId { get; set; }
        public bool StatusPrint { get; set; }
    }

    public class Items
    {
        public int PartGroup { get; set; }
        public Guid ProductGuid { get; set; }
        public string PartGroupName { get; set; }
        public string Barcode { get; set; }
        public string Kode { get; set; }
        public string NamaBArang { get; set; }
        public int CcyId { get; set; }
        public string CcyName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal HargaBeli { get; set; }
        public decimal HargaJual { get; set; }
        public decimal Qty { get; set; }
        //public int ProductId { get; set; }
        public bool StatusPrint { get; set; }
    }
}