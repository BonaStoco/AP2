using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BonaStoco.AP1.CouchDB.Config
{    
     public class MongoConfigSection : ConfigurationSection
        {
            public const string DefaultSectionName = "mongo.config";

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
            [ConfigurationProperty("host")]
            public string Host
            {
                get { return this["host"].ToString(); }
                set { this["host"] = value; }
            }
            [ConfigurationProperty("port")]
            public int Port
            {
                get { return (int)this["port"]; }
                set { this["port"] = value; }
            }
            [ConfigurationProperty("database")]
            public string Database
            {
                get { return this["database"].ToString(); }
                set { this["database"] = value; }
            }
        }    
}
