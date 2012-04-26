using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BonaStoco.AP1.CouchDB.Config
{
    public class ServerAddressCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerAddress();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            ServerAddress serverAddress = (ServerAddress)element;
            return String.Concat(serverAddress.Server, ":", serverAddress.Port);
        }
    }
}
