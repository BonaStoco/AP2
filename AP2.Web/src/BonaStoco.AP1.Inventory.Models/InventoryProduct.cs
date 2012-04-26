using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.Inventory.Models
{
    [Serializable]
    public class InventoryProduct
    {
        public string Kode { get; set; }
        public string Barcode { get; set; }
        public string Nama { get; set; }
        public decimal Balance { get; set; }
    }
}
