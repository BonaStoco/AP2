using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonaStoco.AP1.Web.Models
{
    public class MonthlyActivityMonitoringDto
    {
        public ActivityCounter[] Data { get; set; }
    }
    public class ActivityCounter
    {
        public int Day { get; set; }
        public int Active { get; set; }
        public int NonActive { get; set; }
    }
}