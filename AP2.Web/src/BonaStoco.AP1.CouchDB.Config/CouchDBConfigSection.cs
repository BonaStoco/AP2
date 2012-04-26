using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BonaStoco.AP1.CouchDB.Config
{
    public class CouchDBConfigSection : ConfigurationSection
    {
        public const string DefaultSectionName = "CouchDBConfig";

        [ConfigurationProperty("database", IsRequired = true)]
        public string DatabaseName
        {
            get { return (string)this["database"]; }
            set { this["database"] = value; }
        }

        [ConfigurationProperty("username")]
        public string UserName
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }
        [ConfigurationCollection(typeof(ServerAddressCollection))]
        [ConfigurationProperty("ServerAddress")]
        public ServerAddressCollection ServerAddress
        {
            get { return (ServerAddressCollection)this["ServerAddress"]; }
            set { this["ServerAddress"] = value; }
        }
    }
}
