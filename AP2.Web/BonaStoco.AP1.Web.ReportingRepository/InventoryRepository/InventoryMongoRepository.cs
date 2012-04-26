using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.CouchDB.Config;
using BonaStoco.AP1.Inventory.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class InventoryMongoRepository
    {
        MongoConfig Mongo;

        public InventoryMongoRepository()
        {
            Mongo = new MongoConfig();
        }
        public StockCardDetail GetItemBeginningBalanceByDate(int tenanId, string productId, DateTime dateTime)
        {
            StockCardDetail daily = findDailyBB(tenanId, productId, dateTime);
            StockCardDetail monthly = findMontlyBB(tenanId, productId, dateTime);
            StockCardDetail yearly = findYearlyBB(tenanId, productId, dateTime);
            DateTime date = DateTime.SpecifyKind(dateTime.AddDays(dateTime.Day * -1), DateTimeKind.Utc);
            return new StockCardDetail()
            {
                TransactionType = "Saldo Awal",
                TransactionNumber = "-",
                Date = date,
                Incoming = 0,
                OutGoing = 0,
                Balance = daily.Balance + monthly.Balance + yearly.Balance
            };
        }

        private StockCardDetail findYearlyBB(int tenanId, string productId, DateTime dateTime)
        {
            dateTime = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Utc);
            MongoCollection collectionInvDetail = Mongo.MongoDatabase.GetCollection("yearly");
            IList<BsonDocument> yearDoc = collectionInvDetail.FindAs<BsonDocument>(Query.And(Query.EQ("_id.Tenant", tenanId), Query.And(Query.EQ("_id.Item", productId), Query.LT("_id.Year", dateTime.Year)))).ToList();
            decimal balance = yearDoc.Sum(x => decimal.Parse(x.AsBsonDocument["value"].AsBsonDocument["Balance"].ToString()));
            return new StockCardDetail()
            {
                TransactionType = "Saldo Awal",
                TransactionNumber = "-",
                Date = dateTime,
                Incoming = 0,
                OutGoing = 0,
                Balance = balance
            };
        }

        private StockCardDetail findMontlyBB(int tenanId, string productId, DateTime dateTime)
        {
            dateTime = DateTime.SpecifyKind(dateTime.Date, DateTimeKind.Utc);
            MongoCollection collectionInvDetail = Mongo.MongoDatabase.GetCollection("monthly");
            IList<BsonDocument> monthDoc = collectionInvDetail.FindAs<BsonDocument>(
                Query.And(Query.EQ("_id.Tenant", tenanId),
                Query.And(Query.EQ("_id.Item", productId),
                Query.And(Query.EQ("_id.Year", dateTime.Year),
                Query.And(Query.GTE("_id.Month", 1),
                Query.LT("_id.Month", dateTime.Month)))))).ToList();
            decimal balance = monthDoc.Sum(x => decimal.Parse(x.AsBsonDocument["value"].AsBsonDocument["Balance"].ToString()));
            return new StockCardDetail()
            {
                TransactionType = "Saldo Awal",
                TransactionNumber = "-",
                Date = dateTime,
                Incoming = 0,
                OutGoing = 0,
                Balance = balance
            };
        }

        private StockCardDetail findDailyBB(int tenanId, string productId, DateTime dateTime)
        {
            DateTime startDate = DateTime.SpecifyKind(dateTime.AddDays(dateTime.Day * -1), DateTimeKind.Utc);
            DateTime endDate = DateTime.SpecifyKind(dateTime.AddDays(-1), DateTimeKind.Utc);
            MongoCollection collectionInvDetail = Mongo.MongoDatabase.GetCollection("inventoryDetail");
            IList<BsonDocument> dayDoc = collectionInvDetail.FindAs<BsonDocument>(Query.And(Query.EQ("_id.Tenant", tenanId), Query.And(Query.EQ("_id.Item", productId),
                Query.And(Query.GTE("_id.Date", startDate), Query.LTE("_id.Date", endDate))))).ToList();
            decimal balance = dayDoc.Sum(x => decimal.Parse(x.AsBsonDocument["value"].AsBsonDocument["Balance"].ToString()));
            return new StockCardDetail()
            {
                TransactionType = "Saldo Awal",
                TransactionNumber = "-",
                Date = dateTime,
                Incoming = 0,
                OutGoing = 0,
                Balance = balance
            };
        }

        public IList<StockCardDetail> FindStockCardItemByDate(int tenanId, string productId, DateTime endDate)
        {
            IList<StockCardDetail> stockCardItemList = new List<StockCardDetail>();
            StockCardDetail saldoAwal = GetItemBeginningBalanceByDate(tenanId, productId, endDate);

            stockCardItemList.Add(saldoAwal);
            DateTime _startDate = DateTime.SpecifyKind(saldoAwal.Date.AddDays(1), DateTimeKind.Utc);
            DateTime _endDate = DateTime.SpecifyKind(endDate.Date, DateTimeKind.Utc);
            IList<BsonDocument> stockCardItem = InventoryDetailCollection.FindAs<BsonDocument>(
                 Query.And(Query.EQ("_id.Tenant", tenanId),
                 Query.And(Query.EQ("_id.Item", productId),
                 Query.And(Query.GTE("_id.Date", _startDate), Query.LTE("_id.Date", _endDate))))).ToList();

            foreach (var item in stockCardItem)
            {
                StockCardDetail stockItem = new StockCardDetail()
                {
                    TransactionType = MappingEventType(item["_id"].AsBsonDocument["InvType"].ToString()),
                    TransactionNumber = item["_id"].AsBsonDocument["TransNo"].ToString(),
                    Date = item["_id"].AsBsonDocument["Date"].AsDateTime,
                    Incoming = decimal.Parse(item["value"].AsBsonDocument["InQty"].ToString()),
                    OutGoing = decimal.Parse(item["value"].AsBsonDocument["OutQty"].ToString()),
                    Balance = decimal.Parse(item["value"].AsBsonDocument["Balance"].ToString())
                };

                stockCardItemList.Add(stockItem);
            }

            IList<StockCardDetail> stockCardDetail = incrementBalance(stockCardItemList);
            stockCardDetail.OrderBy(x => x.Date);
            return stockCardDetail;

        }

        private IList<StockCardDetail> incrementBalance(IList<StockCardDetail> stockCardItemList)
        {
            IList<StockCardDetail> stockDetailList = new List<StockCardDetail>();
            var _balance = 0m;
            foreach (var item in stockCardItemList)
            {

                _balance += item.Balance;
                stockDetailList.Add(new StockCardDetail()
                {
                    TransactionType = item.TransactionType,
                    TransactionNumber = item.TransactionNumber,
                    Date = item.Date,
                    Incoming = item.Incoming,
                    OutGoing = item.OutGoing,
                    Balance = _balance

                });
            };


            return stockDetailList;
        }

        public string MappingEventType(string eventType)
        {
           string evenTypeName="";
            switch (eventType)
	        {
                case "0":
                    evenTypeName = "TransferIn";
                break;
                case "1":
                    evenTypeName = "InventoryReturn";
                break;
                case "2":
                    evenTypeName = "POSSales";
                break;
                case  "3":
                    evenTypeName = "POSReturn";
                break;
                case "4":
                    evenTypeName = "StockOpname";
                break;                
	        }

            return evenTypeName;

        }

        public StockCardDetail GetCurrentBalance(int tenantId, string productId)
        {
            DateTime currentDate = DateTime.UtcNow.Date;
            DateTime tomorowDate = currentDate.AddDays(1);

            return GetItemBeginningBalanceByDate(tenantId, productId, tomorowDate);
        }

        public MongoCollection InventoryDetailCollection
        {

            get { return Mongo.MongoDatabase.GetCollection("inventoryDetail"); }
        }

        public MongoCollection InventoryCollection
        {

            get { return Mongo.MongoDatabase.GetCollection("inventory"); }
        }

        public MongoCollection YearlyCollection
        {

            get { return Mongo.MongoDatabase.GetCollection("yearly"); }
        }

        public MongoCollection MonthlyCollection
        {

            get { return Mongo.MongoDatabase.GetCollection("monthly"); }
        }




    }
}
