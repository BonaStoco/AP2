using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindStockOpnameReportById", @"select oh.*,t.tenanname, oi.partgroup,oi.partcode,oi.barcode,oi.partname,oi.sysqty,oi.realqty,oi.differentqty 
	    from opnameheaderreport oh inner join opnameitemreport oi on oh._id=oi.headerid inner join tenan t on oh.tenantid=t.tenanid
	    where oh._id=@id")]
    public class StockOpnameReport:IViewModel
    {
        public Guid _id { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string UserName { get; set; }
        public string OpnameNumber { get; set; }
        public int TenantId { get; set; }
        public string ApprovalOne { get; set; }
        public string ApprovalTwo { get; set; }
        public string ApprovalThree { get; set; }
        public string OpnameNote { get; set; }
        public string Status { get; set; }
        public string TenanName { get; set; }
        public string PartGroup { get; set; }
        public string PartCode { get; set; }
        public string Barcode { get; set; }
        public string PartName { get; set; }
        public int SysQty { get; set; }
        public int RealQty { get; set; }
        public int DifferentQty { get; set; }
    }
}
