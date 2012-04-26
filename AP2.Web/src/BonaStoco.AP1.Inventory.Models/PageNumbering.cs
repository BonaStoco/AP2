using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Inventory.Models
{
    [Serializable]
    [NamedSqlQuery("FindPageNumberByTenanAndGroup",@"select count(*) as RecordCount from product where tenanid = @tenanid and groupid = @groupid")]
    public class PageNumbering : IViewModel
    {
        public double RecordCount { get; set; }
    }
}
