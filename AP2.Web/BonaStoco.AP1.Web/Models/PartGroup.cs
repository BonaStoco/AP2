using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonaStoco.AP1.Web.Models
{
    public class PartGroup
    {
        public int TenanId { get; set; }
        public Guid ModelGuid { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }

    }
}