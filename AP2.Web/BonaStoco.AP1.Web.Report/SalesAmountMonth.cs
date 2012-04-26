using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    public class SalesAmountMonth : IViewModel
    {
        public int LocationId { get; set; }
        public string CompanyName { set; get; }
        public string Transactiondate { set; get; }
        public string OneMonthBefore { set; get; }
        public string TwoMonthBefore {get;set;}
        public string TotalSaleIDRCurrentMonth { set; get; }
        public string TotalSaleUSDCurrentMonth { set; get; }
        public string TotalSaleIDROneMonthBefore { get; set; }
        public string TotalSaleUSDOneMonthBefore { get; set; }
        public string TotalSaleIDRTwoMonthBefore { get; set; }
        public string TotalSaleUSDTwoMonthBefore { get; set; }
    }
}