using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BonaStoco.AP1.Web.Models
{
    public class UploadSalesResponse
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}