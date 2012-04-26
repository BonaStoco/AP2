using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.TENANT_ROLES)]
    public class UnitController : Controller
    {
        //
        // GET: /Unit/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ListUnit()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Unit> units = MasterDataRepository().FindAllUnits(cp.CompanyId);
            return Json(units, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindListUnitByModelGuid(string id)
        {
            Unit unit = MasterDataRepository().FindAllUnitsByModelGuid(Guid.Parse(id));
            return Json(unit, JsonRequestBehavior.AllowGet);
        }
        public void UpdateUnit(String unit)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            UnitEditedMessage update = Newtonsoft.Json.JsonConvert.DeserializeObject<UnitEditedMessage>(unit);
            UnitEditedMessage msg = new UnitEditedMessage()
            {
                TenanId = cp.CompanyId,
                Kode = update.Kode,
                Nama = update.Nama,
                ModelGuid = update.ModelGuid
            };
            new RabbitHelper().SendMasterDataExchange(msg);
        }

        public void AddUnit(String AddUnit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                    TambahUnitMessage add = Newtonsoft.Json.JsonConvert.DeserializeObject<TambahUnitMessage>(AddUnit);
                    TambahUnitMessage msg = new TambahUnitMessage()
                    {
                        TenanId = cp.CompanyId,
                        Nama = add.Nama,
                        Kode = add.Kode,
                        UnitGuid = Guid.NewGuid()
                    };
                    new RabbitHelper().SendMasterDataExchange<TambahUnitMessage>(msg);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
    }
}