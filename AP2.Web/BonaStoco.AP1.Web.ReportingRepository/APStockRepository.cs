using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class APStockRepository:IAPStockRepository
    {
        QueryObjectMapper _QueryMapper;
        public APStockRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }


        #region IAPStockRepository Members

        public IList<StockCard> LoadStockbyGroupName()
        {
            return _QueryMapper.Map<StockCard>("select distinct(groupname) as GroupName, from stockcard").ToList();
         
        }

        public IList<StockCard> FindStockCardByGroupId(string groupId)
        {
            return _QueryMapper.Map<StockCard>("FindStockCardById",new string[]{"id"},new object[]{groupId}).ToList();
        }

     

        public IList<StockCard> FindStockCardByTenanId(int id)
        {
            
            return _QueryMapper.Map<StockCard>("FindStockCardByTenanId", new string[] { "id" }, new object[] { id }).ToList();
        }

        #endregion

        #region IAPStockRepository Members


        public IList<GroupStock> FindGroupNameByTenanId(int id)
        {
            return _QueryMapper.Map<GroupStock>("FindGroupNameByTenanId", new string[] { "id" }, new object[] { id }).ToList();
        }

        #endregion

        #region IAPStockRepository Members


        public IList<StockCard> FindStockCardByGroupName(string groupname, string tenantid)
        {
            return _QueryMapper.Map<StockCard>("FindStockCardByGroupName", new string[] { "groupname", "tenantid" }, new object[] { groupname, tenantid }).ToList();
        }

      
        #endregion
    }
}
