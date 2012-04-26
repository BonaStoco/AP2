using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class LaporanRingkasanPenjualanPerHariRepository : ILaporanRingkasanPenjualanPerHariRepository
    {
        QueryObjectMapper queryObjectMapper;

        public LaporanRingkasanPenjualanPerHariRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariBycategory(int tenanid, string from, string to)
        {
            return queryObjectMapper.Map<LaporanRingkasanPenjualanPerHari>("FindDataRingkasanPerHari",
                new string[] { "tenanid", "from", "to" },
                new object[] { tenanid, from, to });
        }
        public IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariByBandara(int tenanid, string from, string to, int locationid)
        {
            return queryObjectMapper.Map<LaporanRingkasanPenjualanPerHari>("FindDataRingkasanPerHariByBandara",
                new string[] { "tenanid", "from", "to", "locationid"  },
                new object[] { tenanid, from, to, locationid });
        }
        public IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariByTerminal(int tenanid, string from, string to, int terminalid)
        {
            return queryObjectMapper.Map<LaporanRingkasanPenjualanPerHari>("FindDataRingkasanPerHariByTerminal",
                new string[] { "tenanid", "from", "to", "terminalid" },
                new object[] { tenanid, from, to, terminalid });
        }
        public IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariBySubTerminal(int tenanid, string from, string to, int subterminalid)
        {
            return queryObjectMapper.Map<LaporanRingkasanPenjualanPerHari>("FindDataRingkasanPerHariBysubterminal",
                new string[] { "tenanid", "from", "to", "subterminalid" },
                new object[] { tenanid, from, to, subterminalid });
        }
    }
}
