using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Inventory.Models
{
    [Serializable]
    [NamedSqlQuery("FindProductIdByTenanAndGroup", @"select nama,kode, barcode, modelguid as ProductGuid from product where tenanid = @tenanid and groupid = @groupid order by nama asc offset @offset limit @limit")]
    [NamedSqlQuery("FindProductIdStockByTenanAndGroup", @"select nama,kode, barcode, modelguid as ProductGuid from product where tenanid = @tenanid and groupid = @groupid")]
    public class ProductId : IViewModel
    {
        public Guid ProductGuid { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
        public string Barcode { get; set; }
    }
}
