using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Inventory.Models;

namespace BonaStoco.AP1.Web.Report
{
    public interface IInventoryRepository
    {
        IList<InventoryPartGroup> FindPartGroupByTenanId(long tenanId);
    }
}
