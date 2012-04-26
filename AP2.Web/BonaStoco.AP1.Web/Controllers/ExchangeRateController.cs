using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using System.Collections;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.Web.Models;

namespace BonaStoco.AP1.Web.Controllers
{

    [Authorize(Roles= APRoles.AP_ROLES + "," + APRoles.Umum_ROLES)]
    public class ExchangeRateController : Controller
    {
        //
        // GET: /ExchangeRate/

        public ActionResult Index()
        {
            IList<Ccy> ccy = MasterDataRepository().FindAllCurrencies(123);
            return View("ExchangeRate",ccy);
        }

        [HttpPost]
        public JsonResult UpdateExchangeRate(String data)
        {
            ExchangeRateMessage exRateMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeRateMessage>(data);
            ExchangeRateMessage msg = new ExchangeRateMessage()
            {
                StartDate = exRateMsg.StartDate.Date,
                EndDate = exRateMsg.EndDate.Date,
                ExchangeRateId = Guid.NewGuid(),
                Items = exRateMsg.Items
            };
            new RabbitHelper().SendUpdteExchangeRate(msg);
            return Json("Berhasil Update", JsonRequestBehavior.AllowGet);
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
    }
}
