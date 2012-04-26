using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class OpenOpnameCommand : ITenanIdentity
    {
        public int TenanId { get; set; }
        public Guid _id { get; set; }
        public string Username { get; set; }
    }

}
