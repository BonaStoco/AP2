using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    public class Company : IViewModel
    {
        public string LocationId { get; set; }
        public string NameCompany { get; set; }
    }
}
