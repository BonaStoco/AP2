using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class PengirimanBarangMessage : ITenanIdentity
    {
        public Guid Guid { get; set; }
        public int TenanId { get; set; }
        public string NamaPengirim { get; set; }
        public string CcyCode { get; set; }
        public string Referensi { get; set; }
        public string KodeTransaksi { get; set; }
        public DateTime TanggalTransaksi { get; set; }
        public string Keterangan { get; set; }
        public GRNItemMessage[] Items { get; set; }
        public string Discriminator { get; set; }
    }

    public class GRNItemMessage
    {
        public Guid Guid { get; set; }
        public Guid ProductGuid { get; set; }
        public decimal Qty { get; set; }
        public Guid UnitGuid { get; set; }
        public decimal Harga { get; set; }
        public decimal Jumlah { get; set; }
        public string Items { get; set; }
    }
}