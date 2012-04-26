using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindGroupNameByTenanId", "select distinct on (groupname) 0 as GroupId, groupname, companyid as TenantId from stockcard where companyid=@id")]
    public class GroupStock:IViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int TenantId { get; set; }

    }
}