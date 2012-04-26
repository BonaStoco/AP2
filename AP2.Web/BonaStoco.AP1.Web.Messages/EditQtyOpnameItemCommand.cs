using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Messages
{
    [Serializable]
    public class EditQtyOpnameItemCommand : ITenanIdentity
    {
        public int TenanId { get; set; }
        public Guid Id { get; set; }
        public int Qty { get; set; }
        public string OpnameNumber { get; set; }
        public Guid OpnameId { get; set; }
    }
}
