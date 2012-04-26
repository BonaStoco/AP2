using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindBandaraById",@"SELECT locationid,namecompany FROM mappingcompany WHERE locationid = @locationid")]
    [NamedSqlQuery("FindBandaraByCategoryId",@"SELECT locationid,namecompany FROM mappingcompany WHERE categoryid = @categoryid")]
    public class MappingCompany : IViewModel
       {
           public int locationId { get; set; }
           public string NameCompany { get; set; }
       }   
}
