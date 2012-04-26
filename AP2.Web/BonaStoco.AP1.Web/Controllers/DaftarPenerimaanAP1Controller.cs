using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.PengirimanBarang.Models;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class DaftarPenerimaanAP1Controller : Controller
    {
        //
        // GET: /DaftarPenerimaanAP1/

        public ActionResult Index()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View("DaftarPenerimaan", tenans);
        }
        public PartialViewResult ListPenerimaanBarang(string tenant, string dari, string sampai, string status)
        {
            IList<GRN> grn = PengirimanBarangRepository().GetGRNFForDaftarPengiriman(Int32.Parse(tenant), dari, sampai, status,DiscriminatorPengirimanBarang.GRN);
            if (grn == null || grn.Count == 0)
            {
                return PartialView("_EmptyResult");
            }
            ViewBag.TenanId = tenant;
            return PartialView("_ListPenerimaanBarangAP1", grn);
        }
        public ActionResult LaporanPenerimaanBarang(Guid guid, int tenanId)
        {
            Tenan tenan = MasterDataRepository().FindTenanById(tenanId);
            GRN grn = PengirimanBarangRepository().FindByGuidAllstatus(guid);
            IList<GRNItem> grnItem = PengirimanBarangRepository().FindItemsByGRNId(guid);
            ViewBag.GRNItems = grnItem;
            ViewBag.Tenan = tenan;
            return View(grn);
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
        private IPengirimanBarangRepository PengirimanBarangRepository()
        {
            return (IPengirimanBarangRepository)ContextRegistry.
                GetContext().GetObject("PengirimanBarangRepository");
        }
       
        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository().FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult DetailPenerimaanBarang(string grnId)
        {
            IPengirimanBarangRepository _repo = PengirimanBarangRepository();
            GRN grn = _repo.FindByGuid(new Guid(grnId));
            ViewBag.GRN = grn;
            IList<GRNItem> items = _repo.FindItemsByGRNId(new Guid(grnId));
            return PartialView("_DetailPenerimaanBarang",items);
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
    }
}
