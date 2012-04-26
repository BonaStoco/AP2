using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MongoDB.Driver;

namespace BonaStoco.AP1.CouchDB.Config
{
    public class MongoConfig
    {
        public MongoConfig()
        {
            MongoConfigSection mongoSection = (MongoConfigSection)ConfigurationManager.GetSection(MongoConfigSection.DefaultSectionName);

            MongoServerSettings settings = new MongoServerSettings();
            settings.ConnectionMode = ConnectionMode.Direct;
            settings.Server = new MongoServerAddress(mongoSection.Host, mongoSection.Port);
            MongoCredentials credential = new MongoCredentials(mongoSection.UserName, mongoSection.Password);
            settings.DefaultCredentials = credential;
            settings.SafeMode = SafeMode.True;
            settings.SlaveOk = true;
            MongoServer mongoSvr = MongoServer.Create(settings);
            MongoDatabase = mongoSvr.GetDatabase(mongoSection.Database);
        }
        public MongoDatabase MongoDatabase { get; private set; }
    }
}
