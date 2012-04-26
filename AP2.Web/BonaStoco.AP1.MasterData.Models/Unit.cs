using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenanId","SELECT * FROM unit WHERE tenanid=@tenanid")]
    [NamedSqlQuery("FindByTenanIdAndId", "SELECT * FROM unit WHERE tenanid=@tenanid and unitid=@unitid")]
    [NamedSqlQuery("FindByModelGiud", "SELECT * FROM unit WHERE ModelGuid = @id")]
    public class Unit : ModelBase
    {
        public int UnitId { get; set; }
    }
}