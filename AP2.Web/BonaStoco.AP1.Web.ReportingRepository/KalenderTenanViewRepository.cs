using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class KalenderTenanViewRepository : IKalenderTenanViewRepository
    {
        QueryObjectMapper queryObjectMapper;

        public KalenderTenanViewRepository() 
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<KalenderTenantView> ReposetoryKalenderViewTenan(string transDate, int companyid)
        {
            return queryObjectMapper.Map<KalenderTenantView>("FindByMonth", new string[] { "transDate", "companyid" }, new object[] { transDate, companyid }).ToList();
        }
    }
}
