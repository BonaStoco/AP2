using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
namespace BonaStoco.AP1.Web.Messages
{
    [Serializable]
    public class SyncronizeProductTenanIdMessage : ITenanIdentity
    {
        public SyncronizeProductTenanIdMessage() { }
        public int TenanId { get; set; }
    }
   
}
