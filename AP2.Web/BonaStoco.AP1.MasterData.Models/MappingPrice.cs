using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    
    public class MappingPrice : IViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductGuid { get; set; }
        public int TenanId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }

    [NamedSqlQuery("GetMappingPriceByTenanId", @"select m.*, p.nama, p.kode
                                                 From mappingprice m inner join
			                                          product p on m.productid = p.productid 
                                                 where m.tenanid=@tenanid")]
    [NamedSqlQuery("GetMappingByTenanIdAndProductId", @"select m.*, p.nama, p.kode
                                                 From mappingprice m inner join
			                                          product p on m.productid = p.productid 
                                                 where m.id=@id")]
    [NamedSqlQuery("Delete", "DELETE FROM mappingprice WHERE id = @id")]
    public class MappingPriceList : IViewModel
    {
        public string Id { get; set; }
        public Guid ProductGuid { get; set; }
        public int TenanId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
    }
}
