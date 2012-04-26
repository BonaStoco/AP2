using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    [Serializable]
    public class CloseStockOpnameCommand : ITenanIdentity
    {
        public Guid _id { get; set; }
        public int TenanId { get; set; }   
        public string Username { get; set; }
        public string OpnameNumber { get; set; }
        public string ApprovalOne { get; set; }
        public string ApprovalTwo { get; set; }
        public string ApprovalThree { get; set; }
        public string OpnameNote { get; set; }
    }
}
