using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class ApproveOpnameMessage : ITenanIdentity
    {
        public ApproveOpnameMessage() { }
        public Guid _id { get; set; }
        public int TenanId { get; set; }
        public string Username { get; set; }
        public OpnameItemMessage[] OpnameItem { get; set; }
    }

   
    public class OpnameItemMessage
    {
        public Guid _id { get; set; }
        public Guid HeaderId { get; set; }
        public string Barcode { get; set; }
        public string PartCode { get; set; }
        public Guid PartGuid { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
    }
}
