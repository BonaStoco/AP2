using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BonaStoco.AP1.Web.Models
{
    public class RecoverImportProductResponse
    {
        public int TenantId { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
    }
}