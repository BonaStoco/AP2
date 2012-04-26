using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class AddOpnameItemCommand : ITenanIdentity
    {
        public int TenanId { get; set; }
        public Guid Id { get; set; }
        public int PartId { get; set; }
        public Guid PartGuid { get; set; }
        public int PartGroupId { get; set; }
        public Guid OpnameId { get; set; } 
        public int Qty { get; set; }
        public string Barcode { get; set; }    
        public string PartCode { get; set; }
        public string PartGroupName { get; set; }
        public string PartName { get; set; }
      
    }

}
