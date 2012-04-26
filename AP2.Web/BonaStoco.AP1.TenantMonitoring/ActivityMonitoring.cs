using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.TenantMonitoring;
using BonaStoco.AP1.TenantMonitoring.Dto;


namespace BonaStoco.AP1.TenantMonitoring
{
    public class ActivityMonitoring
    {
       
        private int[] activeTenants = new int[0];
        private int[] nonActiveTenants = new int[0];
        private int day;
   
        public ActivityMonitoring(DateTime date, int bandaraId)
        {
            Date = ConverDateToPeriod(date);
            Bandara = bandaraId;
            GenerateActivityCounter(date);
        }

        public ActivityMonitoring(ActivityMonitorSD jsonData)
        {
            Bandara = jsonData.Bandara;
            day = jsonData.Day;
            Date = jsonData.Date;            
            activeTenants = jsonData.Actives;
            nonActiveTenants = jsonData.NonActives;

        }
        private string Date { get;  set; }
        private int Bandara { get;  set; }
        private void GenerateActivityCounter(DateTime date)
        {
            day = date.Day;
            activeTenants = new int[0];
            nonActiveTenants = new int[0];
           
        }

        private string ConverDateToPeriod(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }


        public ActivityMonitorSD GetData()
        {
            return new ActivityMonitorSD { Date = Date, Bandara = Bandara, Day = day, Active = activeTenants.Count(), NonActive = nonActiveTenants.Count(), Actives = activeTenants, NonActives = nonActiveTenants };
        }

        public void Calculate(ActivityTransaction activityTransaction, TenantList tenants)
        {
            
            var tenantId=activityTransaction.TenantId;           
            Count(tenantId, tenants);
        }

        internal void Count(int tenantId, TenantList tenants)
        {
            if (tenants.Tenants.Contains(tenantId))
            {
                AddActiveTenantIfNotExistInTheList(tenantId);              
            }
            nonActiveTenants = tenants.Tenants.Except(activeTenants).ToArray();
        }

        private void AddActiveTenantIfNotExistInTheList(int tenantId)
        {
            if (!activeTenants.Contains(tenantId))
            {
                var currentTenant = new int[] { tenantId };
                activeTenants = this.activeTenants.Concat(currentTenant).ToArray();
            }
        }
        public int[] GetActiveTenant()
        {
            return activeTenants;     
        }

        public int[] GetNonActiveTenant()
        {
            return nonActiveTenants;
        }

        public object GetActivityCounter()
        {
            return new { Day = day, Active = activeTenants.Count(), NonActive = nonActiveTenants.Count() };
        }
    }
}
