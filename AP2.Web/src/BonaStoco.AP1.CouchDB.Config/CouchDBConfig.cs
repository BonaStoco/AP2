using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BonaStoco.AP1.CouchDB.Config
{
    public class CouchDBConfig
    {
        public CouchDBConfig()
        {
            //CouchDBConfigSection couchSection = (CouchDBConfigSection)ConfigurationManager.GetSection(CouchDBConfigSection.DefaultSectionName);
            UserName = "admin";//couchSection.UserName;
            Password = "S31panas";//couchSection.Password;
            ServerName = "tcloud2.bonastoco.com";
            ServerPort = 5984;
            //getServerName(couchSection);
        }

        private void getServerName(CouchDBConfigSection couchSection)
        {
            foreach (ServerAddress server in couchSection.ServerAddress)
            {
                ServerName = server.Server;
                ServerPort = server.Port;
            }
        }
        public string ServerName { get; private set; }
        public int ServerPort { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}
