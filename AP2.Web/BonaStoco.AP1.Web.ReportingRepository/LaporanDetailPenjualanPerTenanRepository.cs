using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class LaporanDetailPenjualanPerTenanRepository : ILaporanDetailPenjualanPerTenanRepository
    {
        QueryObjectMapper queryObjectMapper;

        public LaporanDetailPenjualanPerTenanRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }


        public IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDate(string dari, string sampai, int tenanid)
        {
            return queryObjectMapper.Map<LaporanDetailPenjualanPerTenanView>("FindPenjualanByTenanIdAndDateInCategory",
                new string[] { "dari", "sampai", "tenanid"},
                new object[] { dari, sampai, tenanid });
            
        }

        public IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInBandara(string dari, string sampai, int tenanid, string locationId)
        {
            return queryObjectMapper.Map<LaporanDetailPenjualanPerTenanView>("FindPenjualanByTenanIdAndDateInBandara",
                new string[] { "dari", "sampai", "tenanid", "locationid" },
                new object[] { dari, sampai, tenanid, locationId });
        }

        public IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInTerminal(string dari, string sampai, int tenanid, int terminalId)
        {
            return queryObjectMapper.Map<LaporanDetailPenjualanPerTenanView>("FindPenjualanByTenanIdAndDateInTerminal",
                new string[] { "dari", "sampai", "tenanid", "terminalid" },
                new object[] { dari, sampai, tenanid, terminalId });
        }

        public IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInSubTerminal(string dari, string sampai, int tenanid, int subTerminalId)
        {
            return queryObjectMapper.Map<LaporanDetailPenjualanPerTenanView>("FindPenjualanByTenanIdAndDateInSubTerminal",
                new string[] { "dari", "sampai", "tenanid", "subterminalid" },
                new object[] { dari, sampai, tenanid, subTerminalId });
        }
    }
}
