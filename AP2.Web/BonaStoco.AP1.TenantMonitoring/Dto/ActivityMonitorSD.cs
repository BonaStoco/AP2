using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.TenantMonitoring.Dto
{
    public class ActivityMonitorSD
    {
        public string Date { get; set; }
        public int Bandara { get; set; }
        public int Day { get; set; }
        public int Active { get; set; }
        public int NonActive { get; set; }
        public int ActiveNonTransaction { get; set; }
        public int[] Actives { get; set; }
        public int[] NonActives { get; set; }
        public int[] ActiveNonTransactions { get; set; }
       
    }
}
