using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Models
{
    [NamedSqlQuery("FindMonitoringByPeriode", "SELECT * From monitoring where companyid = @noBandara And periode between @dari and @sampai order by periode asc")]

    public class TenanActivityMonitoring : IViewModel
    {
        public TenanActivityMonitoring()
        {
            Periode = String.Empty;
        }
        public int CompanyId { get; set; }
        public string Periode { get; set; }
        public string Data { get; set; }
        public int GetYear()
        {
            string year = Periode.Substring(0, 4);
            return Convert.ToInt32(year);
        }
        public int GetMonth()
        {
            string month = Periode.Substring(4, 2);
            return Convert.ToInt32(month);
        }
    }
}