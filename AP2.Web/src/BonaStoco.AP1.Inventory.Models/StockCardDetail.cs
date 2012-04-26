using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Inventory.Models
{
    public class StockCardDetail
    {
        public DateTime Date { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionType { get; set; }
        public decimal Incoming { get; set; }
        public decimal OutGoing { get; set; }
        public decimal Balance { get; set; }
    }
}
