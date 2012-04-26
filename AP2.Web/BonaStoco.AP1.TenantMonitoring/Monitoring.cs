using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BonaStoco.AP1.TenantMonitoring.ReportMonitoring
{

    public class Monitoring
    {
        ActivityCounter[] activityCounterList= new ActivityCounter[0];
        public Monitoring(){}
        public Monitoring(string period, int bandara, ActivityCounter activityCounter)
        {
            CompanyId= bandara;
            Periode= period;
            activityCounterList = activityCounterList.Concat<ActivityCounter>(new ActivityCounter[] { activityCounter }).ToArray();
        }
        
        public int CompanyId { get; set ; }
        public string Periode { get; set ; }
        public string Data{
            get { return Newtonsoft.Json.JsonConvert.SerializeObject(activityCounterList); }
            set { activityCounterList = Newtonsoft.Json.JsonConvert.DeserializeObject<ActivityCounter[]>(value); }
        }



        public void AddNewActivityToExistingMonitoring(ActivityCounter activityCounter)
        {
            //foreach (var item in activityCounterList)
            //{
            //    if (item.Day == activityCounter.Day)
            //    {
            //        item.Active = activityCounter.Active;
            //        item.NonActive = activityCounter.NonActive;
            //    }
            //    else
            //    {

            //    }
            //}
            var item = activityCounterList.Where<ActivityCounter>(x => x.Day == activityCounter.Day).FirstOrDefault();
            if (item != null)
            {
                item.Active = activityCounter.Active;
                item.NonActive = activityCounter.NonActive;
            }
            else
            {
                activityCounterList = activityCounterList.Concat<ActivityCounter>(new ActivityCounter[] { activityCounter }).ToArray();
            }

        }    
      
    }

    public class ActivityCounter
    {
        public ActivityCounter()
        {

        }
        public int Day { get; set; }
        public int Active { get; set; }
        public int NonActive { get; set; }

    }
}
