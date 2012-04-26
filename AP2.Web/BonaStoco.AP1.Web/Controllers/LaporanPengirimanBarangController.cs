using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.PengirimanBarang.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.MasterData.Models;
using Newtonsoft.Json;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize]
    public class LaporanPengirimanBarangController : Controller
    {
        //
        // GET: /LaporanPengirimanBarang/

        public ActionResult Index()
        {
            IList<Tenan> tenans = MasterDataRepository().GetAllTenan();
            return View(tenans);
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository"); ;
        }
        private IPengirimanBarangRepository PengirimanBarangRepository()
        {
            return (IPengirimanBarangRepository)ContextRegistry.
                GetContext().GetObject("PengirimanBarangRepository");
        }

        public PartialViewResult LaporanPengirimanBarang(string dari, string sampai, string status)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
            IList<GRN> items = repo.FindByTransaksi(dari, sampai, status, cp.CompanyId, DiscriminatorPengirimanBarang.GRN);
            ViewBag.TenanId = cp.CompanyId;
            return PartialView("_LaporanPengirimanBarang",items);
        }

        public PartialViewResult DetailLaporanPengirimanBarang(string grnId)
        {
            IPengirimanBarangRepository repo = PengirimanBarangRepository();
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
            return PartialView("_DetailLaporanPengirimanBarang", items);
        }
        public ActionResult LaporanPengirimanBarangReport(Guid guid, int tenanId)
        {

            Tenan tenan = MasterDataRepository().FindTenanById(tenanId);
            GRN grn = PengirimanBarangRepository().FindByGuidAllstatus(guid);
            IList<GRNItem> grnItem = PengirimanBarangRepository().FindItemsByGRNId(guid);
            foreach (var data in grnItem)
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

           
            ViewBag.GRNItems = grnItem;
            ViewBag.Tenan = tenan;
            return View(grn);
        }
    }
}