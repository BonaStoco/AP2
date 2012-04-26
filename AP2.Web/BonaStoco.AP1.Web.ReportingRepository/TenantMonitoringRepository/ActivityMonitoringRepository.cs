using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Reporting;
using BonaStoco.AP1.TenantMonitoring.Dto;
using Newtonsoft.Json;
using BonaStoco.AP1.TenantMonitoring.ReportMonitoring;


namespace BonaStoco.AP1.TenantMonitoring.Repository
{
    public class ActivityMonitoringRepository
    {
        public IReportingRepository ReportingRepository { get; set; }
      
       
        public void Save(ActivityMonitoring activityMonitoring)
        {
               ReportingRepositoryShouldNotNull();
               var activityData = activityMonitoring.GetData();
               var existData = ReportingRepository.GetByExample<TenantActivityMonitoring>(new { Code = activityData.Date, Bandara = activityData.Bandara }).FirstOrDefault();
   
                if (existData == null)
                {
                    TenantActivityMonitoring tenantActivityMonitoring = new TenantActivityMonitoring()
                    {
                        Code = activityData.Date,
                        Bandara = activityData.Bandara,
                        Data = JsonConvert.SerializeObject(activityData)
                    };

                    ReportingRepository.Save(tenantActivityMonitoring);
                }
                else
                {
                    ReportingRepository.Update<TenantActivityMonitoring>(new { Data = JsonConvert.SerializeObject(activityData) }, new { Code = activityData.Date, Bandara = activityData.Bandara });
                }
        }

        public TenantActivityMonitoring GetTenantActivityMonitoringByCodeAndBandaraId(string code, int bandaraId)
        {
            return ReportingRepository.GetByExample<TenantActivityMonitoring>(new { Code = code, Bandara = bandaraId }).FirstOrDefault();
        }
        private void ReportingRepositoryShouldNotNull()
        {
            if (ReportingRepository == null)
            {
                throw new NullReferenceException("Tenant Activity Monitoring Repository doesn't exist");
            }
        }

        public void CreateTableTenantActivityMonitoring()
        {
            try
            {
                ReportingRepository.CreateTable<TenantActivityMonitoring>();
               

            }
            catch (Exception)
            {
                
            }
        }

        public void CreateTableMonitoring()
        {
            try
            {
                ReportingRepository.CreateTable<Monitoring>();
            }
            catch (Exception)
            {
                
              
            }
           
        }

        public ActivityMonitoring Get(DateTime date, int bandaraId)
        {
            ReportingRepositoryShouldNotNull();
            var loadedData = ReportingRepository.GetByExample<TenantActivityMonitoring>(new { Code = date.ToString("yyyyMMdd"), Bandara = bandaraId }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD=JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            return new ActivityMonitoring(amSD);
            
        }

       


        //public Monitoring GetDataMonitoringByPeriodAndBandara(DateTime period, int bandaraId)
        //{
        //  ReportingRepositoryShouldNotNull();
        //  return ReportingRepository.GetByExample<Monitoring>(new { Periode = period.ToString("yyyyMM"), CompanyId = bandaraId }).FirstOrDefault();
    
        //}
    }
}
