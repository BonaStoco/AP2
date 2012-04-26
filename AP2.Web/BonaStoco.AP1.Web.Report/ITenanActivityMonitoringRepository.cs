using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Models;

namespace BonaStoco.AP1.Web.Report
{
    public interface ITenanActivityMonitoringRepository
    {
        IList<TenanActivityMonitoring> tenanActivityMonitoring(int noBandara, string dari, string sampai);
    }
}
