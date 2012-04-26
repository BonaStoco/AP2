using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface ILaporanVoidRepository
    {
        IList<LaporanVoidDetail> FindDetailVoidPerKasirByDate(int tenanid, int sessionid, string dari, string sampai);
        IList<LaporanVoidSummary> FindSummaryVoidPerKasirByDate(int tenanid, int sessionid, string dari, string sampai);
        IList<SessionKasir> FindSessionByTenantAndDate(int tenanid, string dari, string sampai);
    }
}
