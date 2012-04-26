using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.PengirimanBarang.Models;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.PengirimanBarang.Repository;
using BonaStoco.AP1.Web.Messages;
using Newtonsoft.Json;
using System.Web.UI;
namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class VerifikasiPengirimanBarangController : Controller
    {
        RabbitHelper rabbitHelper;
        //
        // GET: /VerifikasiPengirimanBarang/
        public ActionResult Index()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }

        public PartialViewResult ListPengirimanBarangYangBelumDiVerifikasi(string tenanId)
        {
            int id = 0;
            Int32.TryParse(tenanId, out id);
            Tenan tenan = new APMasterRepository().GetTenanByTenanId(tenanId);
            if (tenan == null)
                return PartialView("_TenanNotFound", Int32.Parse(tenanId));

            ViewBag.Tenan = tenan;
            IList<GRN> pendingGRN = PengirimanBarangRepository().FindPendingGRNByTenanId(id,DiscriminatorPengirimanBarang.GRN);
            if (pendingGRN.Count == 0)
                return PartialView("_NotFound", tenan);
            return PartialView("_ListPengirimanBarangYangBelumDiVerifikasi", pendingGRN);
        }

        public JsonResult FindAllPendingGRN()
        {
            IList<GRN> pendingGRN = PengirimanBarangRepository().FindAllPendingGRNByDiscriminator(DiscriminatorPengirimanBarang.GRN);
            return Json(pendingGRN, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DetailVerifikasiPengirimanBarang(string grnId, string tenanId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            GRN grn = repo.FindByGuid(new Guid(grnId));
            Tenan tenan = new APMasterRepository().GetTenanByTenanId(tenanId);
            if (tenan == null)
                return View("_TenanNotFound", Int32.Parse(tenanId));

            ViewBag.GRN = grn;
            ViewBag.Tenan = tenan;
            IList<GRNItem> items = repo.FindItemsByGRNId(new Guid(grnId));
            foreach (var data in items)
            {
                string NewProduct = data.Items;
                Items ItemNewProduct = JsonConvert.DeserializeObject<Items>(NewProduct);
                if (data.Items.Length > 0)
                {
                    data.Nama = ItemNewProduct.NamaBArang;
                    data.Kode = ItemNewProduct.Kode;
                    data.Barcode = ItemNewProduct.Barcode;
                    data.HargaJual = ItemNewProduct.HargaJual;
                    data.StatusPrint = ItemNewProduct.StatusPrint;
                    data.CcyCode = ItemNewProduct.CcyName;
                }
            }   

            return View(items);
        }


        public JsonResult FindAllProductByCode(string kode, int tenanId)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            IList<ProductPrint> _product = repo.FindAllProductByCode(kode.ToLower(), tenanId);

            return Json(_product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConfirmVerifikasiPengirimanBarang(string grnId, string tenanId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            IList<GRNItem> items = repo.FindItemsByGRNId(new Guid(grnId));
            foreach (var item in items)
            {
                string product = item.Items;
                Items NewProduct = JsonConvert.DeserializeObject<Items>(product);
                if (item.Items.Length > 0)
                {
                    MasterData.Models.Ccy ccy = MasterDataRepository().FindAllCurrencies(0).Where(m => m.CcyId == NewProduct.CcyId).FirstOrDefault();
                    SendProductToTambahProduk(NewProduct, tenanId);

                    item.Nama = NewProduct.NamaBArang;
                    item.Kode = NewProduct.Kode;
                    item.Barcode = NewProduct.Barcode;
                    item.HargaJual = NewProduct.HargaJual;
                    item.StatusPrint = NewProduct.StatusPrint;
                    item.CcyCode = ccy.Kode;
                }
            }
            GRN grn = new AP1.Web.Models.PengirimanBarang(this.HttpContext)
                .ConfirmVerifikasiPengirimanBarang(grnId);
            Tenan tenan = MasterDataRepository().FindTenanById(Int32.Parse(tenanId));
            ViewBag.Tenan = tenan;
            ViewBag.GrnItem = items;
            return View("ConfirmVerifikasiPengirimanBarangResult", grn);
        }

        private void SendProductToTambahProduk(Items NewProduct, string tenanId)
        {

            MasterData.Models.PartGroup partGroup = MasterDataRepository().FindAllGroups(Int32.Parse(tenanId)).Where(m => m.GroupId == NewProduct.PartGroup).FirstOrDefault();
            if (partGroup == null )
                throw new ApplicationException("Partgroup dengan kode " + NewProduct.PartGroup + " tidak ditemukan dalam database.");

            MasterData.Models.Unit unit = MasterDataRepository().FindAllUnits(Int32.Parse(tenanId)).Where(m => m.UnitId == NewProduct.UnitId).FirstOrDefault();
            if (unit == null)
                throw new ApplicationException("Unit dengan kode " + NewProduct.UnitId + " tidak ditemukan dalam database.");

            MasterData.Models.Ccy ccy = MasterDataRepository().FindAllCurrencies(0).Where(m => m.CcyId == NewProduct.CcyId).FirstOrDefault();
            if (ccy == null)
                throw new ApplicationException("Mata uang " + NewProduct.CcyId + " tidak ditemukan dalam database.");


            BonaStoco.AP1.Web.Messages.TambahProductMessage msg = new BonaStoco.AP1.Web.Messages.TambahProductMessage()
            {
                TenanId = Int32.Parse(tenanId),
                Barcode = NewProduct.Barcode,
                Kode = NewProduct.Kode,
                Nama = NewProduct.NamaBArang,
                HargaBeli = NewProduct.HargaBeli,
                HargaJual = NewProduct.HargaJual,
                GroupId = NewProduct.PartGroup,
                CcyId = NewProduct.CcyId,
                CcyCode = ccy.Kode,
                UnitId = NewProduct.UnitId,
                ProductGuid = NewProduct.ProductGuid,
                StatusPrint = true,
                GroupGUID = partGroup.ModelGuid,
                UnitGUID = unit.ModelGuid
            };
            new RabbitHelper().SendMasterDataExchange<TambahProductMessage>(msg);
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
            item.Qty.ToString("N");
            return PartialView("_Edit",item);
        }
        public ViewResult Reject(string id, string grnId, int tenanId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            repo.DeleteItem(id);
            IList<GRNItem> Items = repo.FindItemsByGRNId(new Guid(grnId));
            Tenan tenan = new APMasterRepository().GetTenanByTenanId(tenanId.ToString());
            if (tenan == null)
                return View("_TenanNotFound", tenanId);
            GRN grn = repo.FindByGuid(new Guid(grnId));
            ViewBag.Tenan = tenan;
            ViewBag.GRN = grn;
            return View("DetailVerifikasiPengirimanBarang", Items);
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
                    item.Jumlah = item.ActualQty * item.Harga;
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
        public ActionResult DeleteVerifikasiPengirimanBarang(string grnId, string tenantId, string noTransaksi)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            repo.DeleteGrn(grnId);
            repo.DeleteGrnItem(grnId);
            ViewBag.Message = "Transaksi dengan no "+ noTransaksi +" Telah Berhasil Dihapus";
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View("Index",tenans); 
        }
    }
}