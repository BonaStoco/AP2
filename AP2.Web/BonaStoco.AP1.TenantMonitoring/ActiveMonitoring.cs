using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.TenantMonitoring
{
    public class ActiveMonitoring
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Active { get; set; }
        public int NonActive { get; set; }
        public int ActiveNonTransaction { get; set; }
        public DateTime Date { get { return new DateTime(Year, Month, Day); } }
        public string DateString
        {
            get
            {
                return String.Format("{0:ddd, d MMM yyyy}", Date);
            }
        }
    }
}
