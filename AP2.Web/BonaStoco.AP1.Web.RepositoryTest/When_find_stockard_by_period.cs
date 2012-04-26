using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.CouchDB.Config;
using LoveSeat;
using BonaStoco.AP1.Inventory.Models;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    [TestFixture]
    public class When_find_stockard_by_period
    {
        CouchDBConfig _config;
        CouchClient _client;
        CouchDatabase db;
        [SetUp]
        public void Setup()
        {
            _config = new CouchDBConfig();
            _client = new CouchClient(_config.ServerName, _config.ServerPort, _config.UserName, _config.Password, false, AuthenticationType.Cookie);
             db = _client.GetDatabase("inventory");
        }

        [Test]
        public void should_get_by_yearly()
        {
           object[] startKeys= new object[3];
            startKeys[0]= 4952;
            startKeys[1] = "102";
            startKeys[2] = 2000;        

          object[] endKeys= new object[3];
            endKeys[0]= 4952;
            endKeys[1] = "102";
            endKeys[2] = 2012;
            ViewOptions options = new ViewOptions();
            options.StartKey.Add(startKeys);
            options.EndKey.Add(endKeys);
            var yearly = db.View("yearly", options, "inventory");
          //List<YearlyStockCard> yearly=  db.View(<YearlyStockCard>("yearly", options, "inventory").Items.ToList();
          Assert.IsNotNull(yearly);
        }

        [Test]
        public void should_get_by_daily()
        {
            //object[] startKeys = new object[3];
            //startKeys[0] = 4952;
            //startKeys[1] = "102";
            //startKeys[2] = DateTime.Now.Date.AddDays(-1);

            object[] endKeys = new object[3];
            endKeys[0] = 4952;
            endKeys[1] = "102";
            endKeys[2] = DateTime.Now.Date;
            ViewOptions options = new ViewOptions();
           
            //options.StartKey.Add(startKeys);
            options.EndKey.Add(endKeys);
            ViewResult doc = db.GetAllDocuments(options);
          
            //List<DailyStockCard> daily = db.View<DailyStockCard>("daily", options, "inventory").Items.ToList();
          //  Assert.IsNotNull(daily);

        }

    }
}
