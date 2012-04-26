using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Inventory.Models
{
    [NamedSqlQuery("FindPGIdsByTenanId",@"select groupid from partgroup where tenanid = @tenanid")]
    public class PGIds : IViewModel
    {
        public long PartGroupId { get; set; }
    }
}
