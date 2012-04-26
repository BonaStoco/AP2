using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Report
{
    public interface ILaporanDetailPenjualanPerTenanRepository
    {
        IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDate(string dari, string sampai, int tenanid);
        IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInBandara(string dari, string sampai, int tenanid, string locationId);
        IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInTerminal(string dari, string sampai, int tenanid, int terminalId);
        IList<LaporanDetailPenjualanPerTenanView> FindPenjualanByTenanIdAndDateInSubTerminal(string dari, string sampai, int tenanid, int subTerminalId);
    }
}
