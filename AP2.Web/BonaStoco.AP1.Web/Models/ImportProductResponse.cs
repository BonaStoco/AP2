using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BonaStoco.AP1.Web.Models
{
    public class ImportProductResponse
    {
        public bool HasError { get; set; }
        public IList<String> ErrorMessages { get; set; }
    }
}