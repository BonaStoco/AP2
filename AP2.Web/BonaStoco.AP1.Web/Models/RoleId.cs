using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BonaStoco.AP1.Web.Models
{
    public class RoleId
    {
        public RoleId(int catId, int bandaraId, int terminalId, int subTerminalId, int roleId)
        {
            this.Category = catId;
            this.Bandara = bandaraId;
            this.Terminal = terminalId;
            this.SubTerminal = subTerminalId;
            this.Role = roleId;
        }
        
        public int Category { get; set; }
        public int Bandara { get; set; }
        public int Terminal { get; set; }
        public int SubTerminal { get; set; }
        public int Role { get; set; }
    }
}