using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindBillingTenantByTenanIdAndYear","select * from billingtenant where tenanid=@TenanId and tahun=Tahun")]
    public class BillingTenant:IViewModel
    {
        public long TenanId { get; set; }
        public bool Januari { get; set; }
        public bool Februari { get; set; }
        public bool Maret { get; set; }
        public bool April { get; set; }
        public bool Mei { get; set; }
        public bool Juni { get; set; }
        public bool Juli { get; set; }
        public bool Agustus { get; set; }
        public bool September { get; set; }
        public bool Oktober { get; set; }
        public bool November { get; set; }
        public bool Desember { get; set; }
        public string Tahun { get; set; }
    }
}
