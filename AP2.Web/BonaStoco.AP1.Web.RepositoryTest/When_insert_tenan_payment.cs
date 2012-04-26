using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.Web.ReportingRepository;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    public class When_insert_tenan_payment : PaymentTenantContext
    {
        [Test]
        public void Should_insert_all_tenan()
        {
            _masterRepo.CreateUnregisterTenantPaymentByYear("2013");
        }
    }
}
