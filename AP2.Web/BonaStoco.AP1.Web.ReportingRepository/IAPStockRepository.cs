using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IAPStockRepository
    {

        IList<StockCard> LoadStockbyGroupName();
        IList<StockCard> FindStockCardByGroupId(string groupId);       
        IList<StockCard> FindStockCardByTenanId(int id);
        IList<GroupStock> FindGroupNameByTenanId(int id);
        IList<StockCard> FindStockCardByGroupName(string nama, string tenanid);
    }
}
