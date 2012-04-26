using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Web.Messages
{
    public class ExchangeRateMessage : ITenanIdentity
    {
        public Guid ExchangeRateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ExchangeRateItem[] Items { get; set; }
        public int TenanId { get; set; }
    }

    public class ExchangeRateItem
    {
        public string CcyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
