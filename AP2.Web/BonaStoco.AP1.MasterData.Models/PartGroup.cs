using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenanId","SELECT * FROM partgroup where tenanid=@tenanid")]
    [NamedSqlQuery("FindByTenanIdAndId", "SELECT * FROM partgroup where tenanid=@tenanid and groupid=@groupid")]
    [NamedSqlQuery("FindByTenanIdAndGuiId", "SELECT * FROM partgroup where modelguid=@modelguid")]


    public class PartGroup : ModelBase
    {
        public int GroupId { get; set; }
    }
}