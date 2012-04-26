using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    
    [NamedSqlQuery("FindStockCardById","select * from stockcard where companyid=@id order by partname")]
    [NamedSqlQuery("FindStockCardByTenanId", "select * from stockcard where companyid=@id order by partname")]
    [NamedSqlQuery("FindStockCardByGroupName", "select * from stockcard where groupname = @groupname and companyid = @tenantid order by partname")]

    public class StockCard:IViewModel
    {
       public string GroupName {get;set;}
       public int PartId {get;set;}
       public string Code { get; set; }
       public string PartName {get;set;}
       public decimal Stock {get;set;}
       public string  Unit {get;set;}
       public int CompanyId{get;set;}
       public string CompanyLocationId { get; set;}
       public string Barcode { get; set; }
    }

  
}