using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    public class When_update_existing_tenant_payment:PaymentTenantContext
    {
        [Test]
        public void Should_update_existing_billing_tenant()
        {
            _masterRepo.UpdateTenanPaymentByChecked(4083, "2011", "apr", true);
            var updatedBillingTenant = _qryObjectMapper.Map<BillingTenant>("FindBillingTenantByTenanIdAndYear", new string[] { "TenanId", "Tahun" }, new object[] { 4083, "2011" }).FirstOrDefault();
            Assert.IsNotNull(updatedBillingTenant);
        }
    }
}
