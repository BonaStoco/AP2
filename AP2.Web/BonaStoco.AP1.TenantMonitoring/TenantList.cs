using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.TenantMonitoring
{
    public class TenantList
    {
        public TenantList(int[] tenants)
        {
            Tenants = tenants;
        }
        public int[] Tenants { get; private set; }
    }

    public class Tenant:IViewModel
    {
        public int tenanId { get; set; }
    }
}
