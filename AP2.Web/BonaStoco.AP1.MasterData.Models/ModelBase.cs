using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.MasterData.Models
{
    abstract public class ModelBase : IViewModel
    {
        public virtual int TenanId { get; set; }
        public virtual Guid ModelGuid { get; set; }
        [Required(ErrorMessage = "Kode harus diisi")]
        public virtual string Kode { get; set; }
         [Required(ErrorMessage = "Nama harus diisi")]
        public virtual string Nama { get; set; }
    }
}