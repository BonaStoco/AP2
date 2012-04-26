using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    public class SalesAmountDay : IViewModel
    {
        public int LocationId { get; set; }
        public string CompanyName { set; get; }
        public string Transactiondate { set; get; }
        public string TwoDaysBefore { set; get; }
        public string PreviousDay {get;set;}
        public string TotalSaleIDR { set; get; }
        public string TotalSaleUSD { set; get; }
        public string TotalSaleIDRPreviousDay { get; set; }
        public string TotalSaleUSDPreviousDay { get; set; }
        public string TotalSaleIDRTwoDaysBefore { get; set; }
        public string TotalSaleUSDTwoDaysBefore { get; set; }
    }
}