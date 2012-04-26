using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IStockOpnameReportRepository
    {
        StockOpnameReport[] FindAllById(Guid _id);
    }
}
