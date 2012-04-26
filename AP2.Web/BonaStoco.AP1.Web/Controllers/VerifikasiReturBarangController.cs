using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.PengirimanBarang.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;

namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class VerifikasiReturBarangController : Controller
    {
        public ActionResult Index()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }

        public PartialViewResult ListReturnBarangYangBelumDiVerifikasi(string tenanId)
        {
            int id = 0;
            Int32.TryParse(tenanId, out id);
            Tenan tenan = new APMasterRepository().GetTenanByTenanId(tenanId);
            if (tenan == null)
                return PartialView("_TenanNotFound", Int32.Parse(tenanId));

            ViewBag.Tenan = tenan;
            IList<GRN> pendingGRN = PengirimanBarangRepository().FindPendingGRNByTenanId(id, DiscriminatorPengirimanBarang.RET);
            if (pendingGRN.Count == 0)
                return PartialView("_NotFound", tenan);
            return PartialView("_ListReturnBarangYangBelumDiVerifikasi", pendingGRN);
        }

        public JsonResult FindAllPendingRET()
        {
            IList<GRN> pendingRET = PengirimanBarangRepository().FindAllPendingGRNByDiscriminator(DiscriminatorPengirimanBarang.RET);
            return Json(pendingRET, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailVerifikasiReturnBarang(string grnId, string tenanId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            GRN grn = repo.FindByGuid(new Guid(grnId));
            Tenan tenan = new APMasterRepository().GetTenanByTenanId(tenanId);
            if (tenan == null)
                return View("_TenanNotFound", Int32.Parse(tenanId));

            ViewBag.GRN = grn;
            ViewBag.Tenan = tenan;
            
            IList<GRNItem> items = repo.FindItemsByGRNId(new Guid(grnId));
            return View(items);
        }

        [HttpPost]
        public ActionResult ConfirmReturnPengirimanBarang(string grnId, string tenanId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            IList<GRNItem> items = repo.FindItemsByGRNId(new Guid(grnId));
            GRN grn = new AP1.Web.Models.PengirimanBarang(this.HttpContext)
                .ConfirmReturnBarang(grnId);
            Tenan tenan = MasterDataRepository().FindTenanById(Int32.Parse(tenanId));
            ViewBag.Tenan = tenan;
            ViewBag.GrnItem = items;
            return View("VerifikasiReturnBarang", grn);
        }

        private IPengirimanBarangRepository PengirimanBarangRepository()
        {
            return (IPengirimanBarangRepository)ContextRegistry.
                GetContext().GetObject("PengirimanBarangRepository");
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
        [HttpPost]
        public JsonResult FindAllItems(string id)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            IList<GRNItem> items = repo.FindItemsByGRNId(new Guid(id));
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Edit(string id)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            GRNItem item = repo.FindItemByItemGuid(id);
            return PartialView("_Edit",item);
        }
        [HttpPost]
        public PartialViewResult UpdateItem(GRNItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

                    UpdateActualQtyMessage msg = new UpdateActualQtyMessage() 
                    {
                        TenanId = cp.CompanyId,
                        Guid = item.Guid,
                        ActualQty = item.ActualQty
                    };
                    new RabbitHelper().SendMasterDataExchange<UpdateActualQtyMessage>(msg);
                    return PartialView("_EditResult", item);
                }
                return PartialView("_EditResult", item);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(item);
            }
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
   }

}
