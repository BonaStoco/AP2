using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class TenanActivityMonitoringRepository : ITenanActivityMonitoringRepository
    {
        QueryObjectMapper queryObjectMapper;

        public TenanActivityMonitoringRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<TenanActivityMonitoring> tenanActivityMonitoring(int noBandara, string dari, string sampai)
        {
            return queryObjectMapper.Map<TenanActivityMonitoring>("FindMonitoringByPeriode", new string[] { "noBandara", "dari", "sampai" }, new object[] { noBandara, dari, sampai }).ToList(); 
        }
    }
}
