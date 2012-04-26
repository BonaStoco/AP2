using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class TenanCreatedMessage : ITenanIdentity
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
        public int HeadOffice { get; set; }
    }
}