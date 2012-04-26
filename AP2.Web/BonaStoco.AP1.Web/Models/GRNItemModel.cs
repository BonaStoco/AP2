using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.Web.Models
{
    public class GRNItemModel
    {
        [Key]
        public int Id { get; set; }
        public int TenanId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public string NamaBarang { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public Guid UnitId { get; set; }
        public decimal Harga { get; set; }
        public decimal Jumlah { get; set; }
        public string Discriminator { get; set; }
        public string Items { get; set; }

        public void CalculateTotal()
        {
            Jumlah = Qty * Harga;
        }
    }
}