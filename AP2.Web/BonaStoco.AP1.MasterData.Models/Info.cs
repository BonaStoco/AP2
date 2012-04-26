using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    public class Info : IViewModel
    {
        public DateTime Tanggal { get; set; }
        public string Headline { get; set; }
        public string Story { get; set; }
    }
}