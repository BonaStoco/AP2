using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class LaporanRingkasanPenjualanPerHariTenanRepository : ILaporanRingkasanPenjualanPerHariTenanRepository
    {
        QueryObjectMapper queryObjectMapper;

        public LaporanRingkasanPenjualanPerHariTenanRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<LaporanRingkasanPenjualanPerHariTenan> FindPenjualanByTenanIdAndDateDays(string dari, string sampai, int tenanid)
        {
            return queryObjectMapper.Map<LaporanRingkasanPenjualanPerHariTenan>("FindDataRingkasanPerHariTenan",
                new string[] { "dari", "sampai", "tenanid" },
                new object[] { dari, sampai, tenanid });
        }
    }
}
