using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class RegisterProductMessage : ITenanIdentity
    {
        public RegisterProductMessage() { }

        public int TenanId { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
        public string Barcode { get; set; }
        public decimal HargaBeli { get; set; }
        public decimal HargaJual { get; set; }

        public int ProductId { get; set; }
        public int GroupId { get; set; }
        public int CcyId { get; set; }
        public int UnitId { get; set; }

        public Guid ProductGuid { get; set; }

        public Guid GroupGUID { get; set; }
        public Guid UnitGUID { get; set; }
        public string CcyCode { get; set; }
        public bool StatusPrint { get; set; }
        public bool StatusProduct { get; set; }
    }
}
