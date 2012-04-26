using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenanId", "SELECT * FROM ccy")]
    [NamedSqlQuery("FindById", "SELECT * FROM ccy where ccyid = @ccyid")]
    public class Ccy : ModelBase
    {
        public int CcyId { get; set; }
        public int Rounding { get; set; }
    }
}