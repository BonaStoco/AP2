using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
        [NamedSqlQuery("FindByMonth",@"select sum(totalsalepercompany) as sales, 
                        CAST(substr(transactiondate,7) as int) as tanggal 
                            from dailysales where substr(transactiondate,0,7)=@transDate group by tanggal order by tanggal asc ")]
        public class KalenderView : IViewModel
        {
            public decimal Sales { get; set; }
            public int Tanggal { get; set; }
        }
}