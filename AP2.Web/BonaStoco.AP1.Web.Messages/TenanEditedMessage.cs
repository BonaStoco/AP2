using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class TenanEditedMessage : ITenanIdentity
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Alamat { get; set; }
        public string Npwp { get; set; }
        public string Nppkp { get; set; }
        public int LocationId { get; set; }
        public int TerminalId { get; set; }
        public int CategoryId { get; set; }
        public int SubTerminalId { get; set; }
        public DateTime TanggalBergabung { get; set; }
        public decimal Tarif { get; set; }
        public int TenanTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string Gate { get; set; }
        public decimal Target { get; set; }
        public string CcyCode { get; set; }
        public int HeadOffice { get; set; }
        public string FormulaKonsesi { get; set; }
    }
}