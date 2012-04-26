using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Spring.Objects.Factory.Attributes;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.Web.Models
{
    public class GRNModel
    {
        public int TenantId { get; set; }
        
        [DisplayName("Nama Tenan")]
        public string NamaTenan { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        [DisplayName("Nama Pengirim")]
        public string NamaPengirim { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [DisplayName("Mata Uang")]
        public int CcyId { get; set; }
        
        public string Referensi { get; set; }

        public string KodeTransaksi { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        [DisplayName("Tanggal Transaksi")]
        public DateTime TanggalTransaksi { get; set; }
        public string Keterangan { get; set; }
    }
}