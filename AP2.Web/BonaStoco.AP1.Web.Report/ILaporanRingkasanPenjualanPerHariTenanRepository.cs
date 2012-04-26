using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Report
{
    public interface ILaporanRingkasanPenjualanPerHariTenanRepository
    {
        IList<LaporanRingkasanPenjualanPerHariTenan> FindPenjualanByTenanIdAndDateDays(string dari, string sampai, int tenanid);

    }
}
