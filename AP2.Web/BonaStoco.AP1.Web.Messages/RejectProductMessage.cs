using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Messages
{
    public class RejectProductMessage : ITenanIdentity
    {
        public int TenanId { get; set; }
        public long ProductId { get; set; }
        public Guid ProductGuid { get; set; }
    }
}