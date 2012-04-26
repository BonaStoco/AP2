using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class KalenderViewRepository : IKalenderViewRepository
    {
        QueryObjectMapper queryObjectMapper;

        public KalenderViewRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<KalenderView> ReposetoryKalenderView(string transDate)
        {
            return queryObjectMapper.Map<KalenderView>("FindByMonth", new string[] { "transDate" }, new object[] { transDate }).ToList();
        }
    }
}