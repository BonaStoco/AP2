using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.TenantMonitoring
{
    public class ActivityTransaction
    {
        public ActivityTransaction()
        {

        }
        public ActivityTransaction(DateTime transactionDate, int tenanId)
        {
            this.TransactionDate = transactionDate;
            this.TenantId = tenanId;
        }
        public int TenantId { get; private set; }
        public DateTime TransactionDate { get; private set; }
    }
}
