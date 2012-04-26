using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Inventory.Models
{
    public class StockCard
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public decimal Incoming { get; set; }
        public decimal OutGoing { get; set; }
        public decimal Balance { get; set; }       
     
    }
}
