using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Models;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class CargoController : Controller
    {
        //
        // GET: /Cargo/
        IAP2CargoRepository _repo = new AP2CargoRepository();

        //Air Craft/
        public ActionResult AirCraft()
        {
            return View();
        }

        public JsonResult ListAirCraft()
        {
            IList<AirCraft> ac = MasterDataRepository().FindAirCraft();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public void AddAirCraft(String data)
        {
            AirCraft add = Newtonsoft.Json.JsonConvert.DeserializeObject<AirCraft>(data);
            _repo.AddAirCraft(add);
        }

        public void UpdateAirCraft(String data)
        {
            AirCraft upadate = Newtonsoft.Json.JsonConvert.DeserializeObject<AirCraft>(data);
            _repo.UpdateAirCraft(upadate);
        }

        //Customer

        public ActionResult Customer()
        {
            return View();
        }

        public JsonResult ListCustomer()
        {
            IList<Customer> ac = MasterDataRepository().FindCustomer();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public void AddCustomer(String data)
        {
            Customer add = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(data);
            _repo.AddCustomer(add);
        }

        public void UpdateCustomer(String data)
        {
            Customer upadate = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(data);
            _repo.UpdateCustomer(upadate);
        }

        //Comodity

        public ActionResult Comodity()
        {
            return View();
        }

        public JsonResult ListComodity()
        {
            IList<Comodity> ac = MasterDataRepository().FindComodity();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public void AddComodity(String data)
        {
            Comodity add = Newtonsoft.Json.JsonConvert.DeserializeObject<Comodity>(data);
            _repo.AddComodity(add);
        }

        public void UpdateComodity(String data)
        {
            Comodity upadate = Newtonsoft.Json.JsonConvert.DeserializeObject<Comodity>(data);
            _repo.UpdateComodity(upadate);
        }

        //Destinetion

        public ActionResult Destination()
        {
            return View();
        }

        public JsonResult ListDestinetion()
        {
            IList<Destinetion> ac = MasterDataRepository().FindDestinetion();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        public void AddDestinetion(String data)
        {
            Destinetion add = Newtonsoft.Json.JsonConvert.DeserializeObject<Destinetion>(data);
            _repo.AddDestinetion(add);
        }

        public void UpdateDestinetion(String data)
        {
            Destinetion upadate = Newtonsoft.Json.JsonConvert.DeserializeObject<Destinetion>(data);
            _repo.UpdateDestinetion(upadate);
        }

        //MasterData Repository
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

    }
}
