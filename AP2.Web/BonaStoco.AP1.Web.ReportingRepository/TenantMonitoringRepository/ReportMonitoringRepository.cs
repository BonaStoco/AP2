using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Reporting;
using BonaStoco.AP1.TenantMonitoring;
using Newtonsoft.Json;
using BonaStoco.AP1.TenantMonitoring.ReportMonitoring;


namespace BonaStoco.AP1.TenantMonitoring.Repository
{
    public class ReportMonitoringRepository
    {
        public IReportingRepository ReportingRepository { get; set; }
        

        public void Save(Monitoring monitoring)
        {
            
            var existData = ReportingRepository.GetByExample<Monitoring>(new { CompanyId = monitoring.CompanyId, Periode = monitoring.Periode }).FirstOrDefault();
            if (existData == null)
            {              

                ReportingRepository.Save(monitoring);
            }
            else
            {
                ReportingRepository.Update<Monitoring>(monitoring, new { CompanyId = monitoring.CompanyId, Periode = monitoring.Periode });
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

        public List<Monitoring> LoadDataMonitoring()
        {
           return ReportingRepository.GetByExample<Monitoring>(new { }).ToList();

        }
    
        public Monitoring GetMonitoring(string periode,int bandaraId)
        {
            return ReportingRepository.GetByExample<Monitoring>(new { CompanyId = bandaraId, Periode = periode }).FirstOrDefault();   
        }
    }
}
