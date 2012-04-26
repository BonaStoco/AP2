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
using BonaStoco.Inf.ExceptionUtils;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    public class PengirimanBarangController : Controller
    {
        HttpPostedFileBase uploadedFileToImport;
        IMasterDataRepository repo = null;
        ImportProductResponse response;

        public PengirimanBarangController()
        {
            response =new ImportProductResponse() { HasError = false, ErrorMessages = new List<String>() };
        }
        private IMasterDataRepository MasterDataRepository
        {
            get
            {
                if (repo == null)
                    repo = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
                return repo;
            }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListPengirimanBarang()
        {
            IList<GRNItemModel> grnItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(GetTenanId(),DiscriminatorPengirimanBarang.GRN);
            return Json(grnItems, JsonRequestBehavior.AllowGet);
        }

        private int GetTenanId()
        {
            return new CompanyProfiles(this.HttpContext).CompanyId;
        }

        public JsonResult CariBarangUntukGRNBerdasarkanKode(string code)
        {
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(
                GetTenanId(), code);
            return Json(products,JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddItemToListPengiriman(string code, int qty)
        {
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(
                GetTenanId(), code);
            GRNItemModel item = null;
            if (products.Count > 0)
            {
                MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
                item = pb.Add(products[0], DiscriminatorPengirimanBarang.GRN,qty);
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateNewProduct(String NewProduct)
        {
            NewProductModel Product = JsonConvert.DeserializeObject<NewProductModel>(NewProduct);
            Product.ProductGuid = Guid.NewGuid();
            string Items = JsonConvert.SerializeObject(Product);
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel GRNItem = pb.AddNewProduct(Product, DiscriminatorPengirimanBarang.GRN,GetTenanId(),Items);
            return Json(GRNItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int id)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel item = pb.FindById(id, GetTenanId());
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateItem(GRNItemModel item)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            GRNItemModel editItem = pb.FindById(item.Id, cp.CompanyId);
            editItem.Qty = item.Qty;
            GRNItemModel savedItem = pb.Update(editItem);
            return Json(editItem);
        }

        [HttpPost]
        public JsonResult DeleteItem(GRNItemModel item)
        {
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            pb.Delete(item);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAllItem()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
            pb.DeleteAll(cp.CompanyId,DiscriminatorPengirimanBarang.GRN);
            return Json("Delete Berhasil", JsonRequestBehavior.AllowGet);
        }
        public ActionResult KirimBarang()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            GRNModel grn = new GRNModel()
            {
                TenantId = cp.CompanyId,
                NamaTenan = cp.CompanyName,
                TanggalTransaksi = DateTime.Today
            };
            IList<Ccy> ccyList = MasterDataRepository.FindAllCurrencies(cp.CompanyId);
            ViewBag.CcyId = new SelectList(ccyList, "CcyId", "Nama");
            IList<GRNItemModel> items = new MDL.PengirimanBarang(this.HttpContext).GetItems(cp.CompanyId, DiscriminatorPengirimanBarang.GRN);
            ViewBag.GRNItems = items.OrderBy(m => m.NamaBarang).ToList();

            return View(grn);
        }

        [HttpPost]
        public ActionResult KirimBarang(GRNModel grn)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            Tenan tenan = MasterDataRepository.FindTenanById(cp.CompanyId);
            ViewBag.GRNItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(grn.TenantId, DiscriminatorPengirimanBarang.GRN);
            grn.KodeTransaksi = new MDL.PengirimanBarang(this.HttpContext).GetGRNTransactionNumber(grn.TanggalTransaksi, grn.TenantId);

            if (ModelState.IsValid)
            {
                IList<GRNItemModel> items = new MDL.PengirimanBarang(this.HttpContext).GetItems(grn.TenantId,DiscriminatorPengirimanBarang.GRN);

                ViewBag.GRNItems = items.OrderBy(m => m.NamaBarang).ToList();
                ViewBag.AlamatTenan = tenan.Alamat;
                new MDL.PengirimanBarang(this.HttpContext).Kirim(grn,cp.CompanyId,DiscriminatorPengirimanBarang.GRN);
                return View("_BarangTerkirim", grn);
            }
            IList<Ccy> ccyList = MasterDataRepository.FindAllCurrencies(cp.CompanyId);
            ViewBag.CcyId = new SelectList(ccyList, "CcyId", "Nama");
            ViewBag.GRNItems = new MDL.PengirimanBarang(this.HttpContext).GetItems(cp.CompanyId, DiscriminatorPengirimanBarang.GRN);
            return View(grn);
        }

        #region Import Pengiriman Barang
        public ActionResult ImportPengirimanBarang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportPengirimanBarang(HttpPostedFileBase file)
        {
            IList<GRNItemModel> item = new List<GRNItemModel>();
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                DeleteAllItem();
                using(StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');
                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        string row = rows[rowIndex];

                        if (row == string.Empty)
                            continue;

                        item.Add(ImportPengirimanBarang(cp, row));
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
            return View("Index",response);
        }

        private GRNItemModel ImportPengirimanBarang(CompanyProfiles cp, string row)
        {
            GRNItemModel item = null;
            try
            {
                string[] pengirimanBarangArr = row.Split(',');
                string code = pengirimanBarangArr[0].Trim();
                string nama = pengirimanBarangArr[1].Trim();
                string qty = pengirimanBarangArr[2].Trim();
                Product product = MasterDataRepository.FindProductByCode(cp.CompanyId, code);
                if (product == null)
                {
                    throw new ApplicationException("Kode barang " + code + " tidak ditemukan dalam database.");
                }
                MDL.PengirimanBarang pb = new MDL.PengirimanBarang(this.HttpContext);
                item = pb.Add(product, DiscriminatorPengirimanBarang.GRN, Int32.Parse(qty));
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
            return item;
        }
        private void FailIfContentTypeNotCSV()
        {
            if (uploadedFileToImport.ContentType == "text/csv")
                return;
            if (uploadedFileToImport.ContentType == "application/vnd.ms-excel")
                return;
            if (uploadedFileToImport.ContentType == "application/octet-stream")
                return;

            throw new ApplicationException("Format file harus dalam CSV");
        }
        #endregion
        public ActionResult PengirimanBarangDariAP1()
        {
            return View();
        }

        private IPengirimanBarangRepository PengirimanBarangRepository()
        {
            return (IPengirimanBarangRepository)ContextRegistry.GetContext().GetObject("PengirimanBarangRepository");
        }

        public JsonResult GetPartGroup()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<GetToComboBoxPartGroup> partGroup = PengirimanBarangRepository().FindByPartGroup(cp.CompanyId);
            return Json(partGroup, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCcy()
        {
            IList<GetToComboBoxCcy> Ccy = PengirimanBarangRepository().FindByCcy();
            return Json(Ccy,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnit()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<GetToComboBoxUnit> Unit = PengirimanBarangRepository().FindByUnit(cp.CompanyId);
            return Json(Unit, JsonRequestBehavior.AllowGet);
        }
    }
}
