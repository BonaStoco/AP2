using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Messages
{
    public class PartGroupEditedMessage : ITenanIdentity
    {
        public int TenanId { get; set; }
        public Guid ModelGuid { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
    }
}
