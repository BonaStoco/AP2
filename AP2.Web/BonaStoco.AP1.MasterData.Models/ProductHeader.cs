using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery(@"select r.tenanid,t.tenanname,r.totalrequest from
                    (select tenanid, count(tenanid) as totalrequest from requestproduct where status=0 group by tenanid) as r
                        left Join tenan t On t.tenanid = r.tenanid")]
    public class ProductHeader:IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public long TotalRequest { get; set; }
    }
}
