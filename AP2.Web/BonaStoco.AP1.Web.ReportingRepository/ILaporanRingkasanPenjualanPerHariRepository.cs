using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface ILaporanRingkasanPenjualanPerHariRepository
    {
        IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariBycategory(int tenanid, string from, string to);
        IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariByBandara(int tenanid, string from, string to, int locationid);
        IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariByTerminal(int tenanid, string from, string to, int terminalid);
        IList<LaporanRingkasanPenjualanPerHari> FindRingkasanPenjualanPerHariBySubTerminal(int tenanid, string from, string to, int subterminalid);
    }
}
