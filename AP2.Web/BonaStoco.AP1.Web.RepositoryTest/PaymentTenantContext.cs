using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.Web.ReportingRepository;
using NServiceBus;
using BonaStoco.Inf.DataMapper;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    [TestFixture]
    abstract public class PaymentTenantContext
    {
        protected IAPMasterRepository _masterRepo;
        protected IQueryObjectMapper _qryObjectMapper;
        protected IBus bus;

        [TestFixtureSetUp]
        public void SetUp()
        {
            InitRepository();
            InitBus();
        }

        private void InitBus()
        {
            bus = NServiceBus.Configure.With()
                    .SpringBuilder()
                    .BinarySerializer()
                    .MsmqTransport()
                        .IsTransactional(true)
                        .PurgeOnStartup(false)
                        .TransactionTimeout(TimeSpan.FromHours(24))
                    .UnicastBus()
                        .ImpersonateSender(false)
                        .LoadMessageHandlers()
                    .CreateBus()
                    .Start();
        }

        protected void InitRepository()
        {
            _qryObjectMapper = (IQueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
            _masterRepo = new APMasterRepository();
        }
    }
}
