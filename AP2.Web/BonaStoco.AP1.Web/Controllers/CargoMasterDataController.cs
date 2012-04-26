using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers
{
    public class CargoMasterDataController : Controller
    {
        //
        // GET: /CargoMasterData/

        public JsonResult AirCrafts()
        {
            IList<AirCraft> ac = MasterDataRepository().FindAirCraft();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Customers()
        {
            IList<Customer> ac = MasterDataRepository().FindCustomer();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Commodities()
        {
            IList<Comodity> ac = MasterDataRepository().FindComodity();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Destinations()
        {
            IList<Destinetion> ac = MasterDataRepository().FindDestinetion();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        //MasterData Repository
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

    }
}
