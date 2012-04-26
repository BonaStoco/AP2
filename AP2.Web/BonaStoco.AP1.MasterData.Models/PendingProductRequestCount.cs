using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery(
        @"select count(*) as count from requestproduct p inner join 
          partgroup g on p.groupid = g.groupid inner join 
          ccy c on p.ccyid = c.ccyid inner join 
          unit u on p.unitid = u.unitid inner join 
          tenan t on p.tenanid=t.tenanid
          where p.status=0;")]
    public class PendingProductRequestCount : IViewModel
    {
        public long Count { get; set; } 
    }
}