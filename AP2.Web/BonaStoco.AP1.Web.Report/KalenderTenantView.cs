using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindByMonth", @"select sum(totalsalepertenan) as sales, 
                        CAST(substr(transactiondate,7) as int) as tanggal 
                            from tenantdailysales where substr(transactiondate,0,7)=@transDate AND tenanid = @companyid group by tanggal order by tanggal asc ")]

    public class KalenderTenantView : IViewModel
    {
        public decimal Sales { get; set; }
        public int Tanggal { get; set; }
    }
}
