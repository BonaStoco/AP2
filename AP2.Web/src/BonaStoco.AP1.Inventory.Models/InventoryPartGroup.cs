using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Inventory.Models
{
    [NamedSqlQuery("FindPartGroupByTenanId", @"select nama as PartGroupName, tenanid as TenanId, groupid as PartGroupId from partgroup where tenanid = @tenanid order by nama")]
    [Serializable]
    public class InventoryPartGroup : IViewModel
    {
        public string PartGroupName { get; set; }
        public long TenanId { get; set; }
        public long PartGroupId { get; set; }
    }
}
