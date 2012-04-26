using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Messages
{
    public class UpdateActualQtyMessage : ITenanIdentity
    {       
           public UpdateActualQtyMessage() { }

           public int TenanId { get; set; }
           public Guid Guid { get; set; }
           public decimal ActualQty { get; set; }
       
    }
}
