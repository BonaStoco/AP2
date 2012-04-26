using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class StockOpnameReportRepository : IStockOpnameReportRepository
    {
        QueryObjectMapper _QueryMapper;

        public StockOpnameReportRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }
        public StockOpnameReport[] FindAllById(Guid _id)
        {
            return _QueryMapper.Map<StockOpnameReport>("FindStockOpnameReportById", new string[] { "id" }, new object[] { _id }).ToArray();
        }
    }
}
