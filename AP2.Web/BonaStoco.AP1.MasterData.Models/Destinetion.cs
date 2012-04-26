using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery("SELECT * from cargodestinetion")]
    [Serializable]
    public class Destinetion : IViewModel
    {
        public Guid Guid { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
    }
}
