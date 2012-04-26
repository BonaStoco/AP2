using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Inventory.Models;
using BonaStoco.Inf.DataMapper;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    [TestFixture]
    public class StockCardReportFixture
    {

        [Test]
        public void GetItemBeginningBalanceByDate()
        {
          InventoryMongoRepository invMongoRepo = new InventoryMongoRepository();
          StockCardDetail saldo= invMongoRepo.GetItemBeginningBalanceByDate(4451, "305", new DateTime(2012, 4, 30));
          Assert.Greater(saldo.Balance, 0);
          Assert.Greater(saldo.Incoming, 0);
          Assert.Greater(saldo.OutGoing, 0);

        }

        [Test]
        public void FindStockCardItemByDate()
        {
            InventoryMongoRepository invMongoRepo = new InventoryMongoRepository();
            IList<StockCardDetail> stockCardDetailList = invMongoRepo.FindStockCardItemByDate(4451, "305", new DateTime(2012, 4, 30));
            Assert.IsNotNull(stockCardDetailList, null);
        }

        [Test]
        public void GetCurrentBalance()
        {
            InventoryMongoRepository invMongoRepo = new InventoryMongoRepository();
          StockCardDetail saldo= invMongoRepo.GetCurrentBalance(4451, "305");
          Assert.AreNotEqual(saldo.Balance, 0m);
          Assert.AreEqual(saldo.Incoming, 0m);
          Assert.AreEqual(saldo.OutGoing, 0m);
        }

        
        

      
       
    }
}
