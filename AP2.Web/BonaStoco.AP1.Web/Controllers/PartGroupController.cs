using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;


namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles = APRoles.TENANT_ROLES)]
    public class PartGroupController : Controller
    {
        //
        // GET: /PartGroup/
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

        public ActionResult Index()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            ViewBag.TenanId = cp.CompanyId;
            return View();
        }
        public JsonResult FindListPartGroupByTenanId()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<BonaStoco.AP1.MasterData.Models.PartGroup> partGroup = MasterDataRepository().FindAllGroups(cp.CompanyId);
            return Json(partGroup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindListPartGroupByModelGuid(string id)
        {
           
          BonaStoco.AP1.MasterData.Models.PartGroup partGroup = MasterDataRepository().GetPartGroupByModelGuid(Guid.Parse(id));

            return Json(partGroup, JsonRequestBehavior.AllowGet);
        }

        public void UpdatePartGroup(String partGroup)
        {

            PartGroupEditedMessage msg = null;            
            IList<BonaStoco.AP1.MasterData.Models.PartGroup> listPartGroup = null;
            PartGroupEditedMessage messUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<PartGroupEditedMessage>(partGroup);
            BonaStoco.AP1.MasterData.Models.PartGroup _partGroup = MasterDataRepository().GetPartGroupByModelGuid(messUpdate.ModelGuid);
           
            msg = new PartGroupEditedMessage()
            {
                TenanId = _partGroup.TenanId,
                Kode = messUpdate.Kode,
                Nama = messUpdate.Nama,
                ModelGuid = messUpdate.ModelGuid
            };
            new RabbitHelper().SendMasterDataExchange<PartGroupEditedMessage>(msg);
        }

        public void AddPartGroup(String partGroup)
        {
            TambahPartGroupMessage msg = null;
            IList<BonaStoco.AP1.MasterData.Models.PartGroup> listPartGroup = null;
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            TambahPartGroupMessage messAdd = Newtonsoft.Json.JsonConvert.DeserializeObject<TambahPartGroupMessage>(partGroup);
            msg = new TambahPartGroupMessage()
            {
                TenanId = cp.CompanyId,
                Kode = messAdd.Kode,
                Nama = messAdd.Nama,
                ModelGuid = Guid.NewGuid()
            };
            new RabbitHelper().SendMasterDataExchange<TambahPartGroupMessage>(msg);
        }

        public ActionResult TambahPartGroup()
        {

            return View(new BonaStoco.AP1.MasterData.Models.PartGroup());
        }

        [HttpPost]
        public ActionResult TambahPartGroup(BonaStoco.AP1.MasterData.Models.PartGroup partGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

                    TambahPartGroupMessage msg = new TambahPartGroupMessage()
                    {
                        Kode = partGroup.Kode,
                        ModelGuid = Guid.NewGuid(),
                        Nama = partGroup.Nama,
                        TenanId = cp.CompanyId
                    };

                    new RabbitHelper().SendMasterDataExchange<TambahPartGroupMessage>(msg);

                    return View("TambahPartGroupSelesai");
                }

                return View(partGroup);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(partGroup);
            }
           
        }
    }
}