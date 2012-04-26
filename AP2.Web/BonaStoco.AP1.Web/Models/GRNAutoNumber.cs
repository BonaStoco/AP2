using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BonaStoco.AP1.Web.Models
{
    public class GRNAutoNumbering
    {
        [Key]
        public int Id { get; set; }
        public int Index { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TenantId { get; set; }
        public void Next()
        {
            Index++;
        }
        public string GetNumberString()
        {
            return string.Format("{0}{1}-{2}",
                Year.ToString(),
                Month.ToString().PadLeft(2, '0'),
                Index.ToString().PadLeft(6, '0'));
        }
    }
}