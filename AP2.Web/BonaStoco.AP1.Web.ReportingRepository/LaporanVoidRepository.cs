using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class LaporanVoidRepository : ILaporanVoidRepository
    {
        QueryObjectMapper queryObjectMapper;

        public LaporanVoidRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<LaporanVoidDetail> FindDetailVoidPerKasirByDate(int tenanid, int sessionid, string dari, string sampai)
        {
            return queryObjectMapper.Map<LaporanVoidDetail>("FindDetailVoidPerKasirByDate",
                new string[] { "tenanid", "sessionid", "dari", "sampai" },
                new object[] { tenanid, sessionid, dari, sampai }).ToList();
        }

        public IList<LaporanVoidSummary> FindSummaryVoidPerKasirByDate(int tenanid, int sessionid, string dari, string sampai)
        {
            return queryObjectMapper.Map<LaporanVoidSummary>("FindSummaryVoidPerKasirByDate",
                new string[] { "tenanid", "sessionid", "dari", "sampai" },
                new object[] { tenanid, sessionid, dari, sampai }).ToList();
        }

        public IList<SessionKasir> FindSessionByTenantAndDate(int tenanid, string dari, string sampai)
        {
            return queryObjectMapper.Map<SessionKasir>("FindSessionByTenantAndDate",
                    new string[] { "tenanid", "dari", "sampai" },
                    new object[] { tenanid, dari, sampai }).ToList();
        }

    }
}
