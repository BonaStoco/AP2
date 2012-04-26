using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace BonaStoco.AP1.Hubs.Models
{
    [HubName("stockOpnameHub")]
    public class StockOpnameHub : Hub
    {
        public void register(string stockOpnameId)
        {
            AddToGroup(stockOpnameId);
        }

        public void unreg(string stockOpnameId)
        {
            RemoveFromGroup(stockOpnameId);
        }

        public void headerChanged(string stockOpnameId, object json)
        {
            Clients[stockOpnameId].updateHeader(json);
        }

        public void itemChanged(string stockOpnameId, object json)
        {
            Clients[stockOpnameId].updateItem(json);
        }

        public void itemAdded(string stockOpnameId,object json)
        {
            Clients[stockOpnameId].itemAdded(json);
        }

        public void onError(string stockOpnameId, object json)
        {
            Clients[stockOpnameId].showError(json);
        }
    }
}