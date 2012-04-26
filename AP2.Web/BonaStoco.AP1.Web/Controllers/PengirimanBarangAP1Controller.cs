using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using MDL = BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.PengirimanBarang.Models;
namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class PengirimanBarangAP1Controller : Controller
    {
        IMasterDataRepository repo = null;       
        private IMasterDataRepository MasterDataRepository
        {
            get
            {
                if (repo == null)
                    repo = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
                return repo;
            }
        }
        public JsonResult LoadTenan()
        {
            IList<Tenan> _tenan = MasterDataRepository.GetAllTenan();
            return Json(_tenan, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /PengirimanBarang/
       //[HttpPost]
        public ViewResult Index(string tenanId)
        {
            if(tenanId==string.Empty || tenanId == null)
                return View("TenanNotFound"); 

            ViewBag.TenanId = tenanId;
            IList<GRNItemModel> grnItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(Int32.Parse(tenanId),DiscriminatorPengirimanBarang.GRN);
            Tenan tenan = MasterDataRepository.FindTenanById(Int32.Parse(tenanId));
            if (tenan == null)
                return View("TenanNotFound");           
            ViewBag.Nama = tenan.TenanName;            
           return View(grnItems);
        }

        public ActionResult ViewTenan()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }

        [HttpGet]
        public JsonResult FindByTenant(int id)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(id);
            if (tenan == null)
                return Json(new Tenan() { TenanId = 0, TenanName = "" },JsonRequestBehavior.AllowGet);
            return Json(tenan, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CariBarangUntukGRNBerdasarkanKode(string code,string tenanId)
        {
            ViewBag.TenanId = Int32.Parse(tenanId);
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(ViewBag.TenanId, code);
            products = products.Where(p => p.StatusProduct = true).ToList();
            GRNItemModel item=null;
            if (products.Count > 0)
            {
                MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
                item = pb.FindByBarcode(products[0].Barcode, ViewBag.TenanId,DiscriminatorPengirimanBarang.GRN);
                if(item!=null)
                    return PartialView("_EmptyResult");
                item = pb.Add(products[0],DiscriminatorPengirimanBarang.GRN);
            }
            return PartialView("_HasilPencarianBarangGRN", item);
        }

        public PartialViewResult Edit(int id,int tenanId)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel item = pb.FindById(id,tenanId);
            return PartialView("_Edit", item);
        }

        [HttpPost]
        public PartialViewResult UpdateItem(GRNItemModel item)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel savedItem = pb.Update(item);
            return PartialView("_EditResult", savedItem);
        }

        [HttpPost]
        public PartialViewResult DeleteItem(GRNItemModel item)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            pb.Delete(item);
            return PartialView("_DeleteResult");
        }

        public PartialViewResult CancelEdit(int id, int tenanId)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel item = pb.FindById(id, tenanId);
            return PartialView("_EditResult", item);
        }

        public ActionResult KirimBarang(int id)
        {
            Tenan cp = MasterDataRepository.FindTenanById(id);
            GRNModel grn = new GRNModel()
            {
                TenantId = cp.TenanId,
                NamaTenan = cp.TenanName,
                TanggalTransaksi = DateTime.Now.Date
            };
            IList<Ccy> ccyList = MasterDataRepository.FindAllCurrencies(cp.TenanId);
            ViewBag.CcyId = new SelectList(ccyList, "CcyId", "Nama");
            ViewBag.GRNItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(cp.TenanId, DiscriminatorPengirimanBarang.GRN);
            return View(grn);
        }

        [HttpPost]
        public ActionResult KirimBarang(GRNModel grn)
        {
            if (ModelState.IsValid)
            {
                grn.KodeTransaksi = new MDL.PengirimanBarang(this.HttpContext).GetGRNTransactionNumber(grn.TanggalTransaksi, grn.TenantId);
                new MDL.PengirimanBarang(this.HttpContext).Kirim(grn,grn.TenantId, PengirimanBarang.Models.DiscriminatorPengirimanBarang.GRN);
                return View("_BarangTerkirim");
            }

            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Ccy> ccyList = MasterDataRepository.FindAllCurrencies(cp.CompanyId);
            ViewBag.CcyId = new SelectList(ccyList, "CcyId", "Nama");
            ViewBag.GRNItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(grn.TenantId, DiscriminatorPengirimanBarang.GRN);
            return View(grn);
        }

        public ActionResult PengirimanBarangDariAP1()
        {
            return View();
        }
        
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
    }
}