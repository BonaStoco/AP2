using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByGuid", "SELECT * from grnitem where guid = @guid ")]
    public class GrnItem : IViewModel
    {
        public string GRNId { get; set; }
        public string Guid { get; set; }
        public string ProductGuid { get; set; }
        public decimal Qty { get; set; }
        public decimal ActualQty { get; set; }
        public string UnitGuid { get; set; }
        public decimal Harga { get; set; }
        public decimal Jumlah { get; set; }

    }
}
