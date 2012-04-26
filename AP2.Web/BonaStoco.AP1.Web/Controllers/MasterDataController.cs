using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.Inf.ExceptionUtils;
using System.IO;

namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]

    public class MasterDataController : Controller
    {
        HttpPostedFileBase uploadedFileToImport;
        ImportProductResponse response;
        RabbitHelper rabbitHelper;
        IList<Product> products = new List<Product>();
        IList<BonaStoco.AP1.MasterData.Models.PartGroup> partGroups = new List<BonaStoco.AP1.MasterData.Models.PartGroup>();

        public MasterDataController()
        {
            response = new ImportProductResponse() { HasError = false, ErrorMessages = new List<String>() };
            rabbitHelper = new RabbitHelper();
        }

        private void LoadData()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            ViewBag.GroupId = new SelectList(MasterDataRepository.FindAllGroups(cp.CompanyId), "GroupId", "Nama");
            ViewBag.CcyId = new SelectList(MasterDataRepository.FindAllCurrencies(cp.CompanyId), "CcyId", "Nama");
            ViewBag.UnitId = new SelectList(MasterDataRepository.FindAllUnits(cp.CompanyId), "UnitId", "Nama");
        }
        private void LoadDataForEditing(Product p)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            ViewBag.GroupId = new SelectList(MasterDataRepository.FindAllGroups(cp.CompanyId), "GroupId", "Nama", p.GroupId);
            ViewBag.CcyId = new SelectList(MasterDataRepository.FindAllCurrencies(cp.CompanyId), "CcyId", "Nama", p.CcyId);
            ViewBag.UnitId = new SelectList(MasterDataRepository.FindAllUnits(cp.CompanyId), "UnitId", "Nama", p.UnitId);
        }
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ActionResult Index(string code)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            Product products = MasterDataRepository.FindProductByCode(cp.CompanyId, code);
            return View(products);
        }
        public JsonResult InitialSearchProduct()
        {
            return Json(getProducts(), JsonRequestBehavior.AllowGet);
        }

        private IList<Product> getProducts()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            return MasterDataRepository.FindSearchProductByName(cp.CompanyId, "a");
        }

        private IList<Product> GetProductsByName(string name)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            return products = MasterDataRepository.FindSearchProductByName(cp.CompanyId, name);
        }

        public JsonResult InitialSearchGRNProduct()
        {
            IList<Product> products = getProducts();
            products = products.Where(x => x.StatusProduct == true).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProductByName(string name)
        {
            return Json(GetProductsByName(name), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult SearchProductGrnByName(string name)
        {
            IList<Product> products = GetProductsByName(name);
            products = products.Where(p => p.StatusProduct == true).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TambahProduct()
        {
            LoadData();
            return View(new Product());
        }

        [HttpPost]
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ActionResult TambahProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                    IList<RequestProduct> listRequestProduct = MasterDataRepository.FindProductApprovedAndPendingByTenanId(cp.CompanyId);
                    IList<Product> listProduct = MasterDataRepository.FindAllProduct(cp.CompanyId);
                    if (IsProductExist(listRequestProduct, listProduct, product))
                    {
                        ViewBag.IsExist = true;
                        LoadData();
                        return View(product);
                    }

                    RegisterProductMessage msg = new RegisterProductMessage
                    {
                        TenanId = cp.CompanyId,
                        ProductId = product.ProductId,
                        Barcode = product.Barcode,
                        Kode = product.Kode,
                        Nama = product.Nama,
                        HargaBeli = product.HargaBeli,
                        HargaJual = product.HargaJual,
                        GroupId = product.GroupId,
                        UnitId = product.UnitId,
                        CcyId = product.CcyId,
                        ProductGuid = Guid.NewGuid(),
                        StatusPrint = product.StatusPrint,
                        StatusProduct = true
                    };

                    BonaStoco.AP1.MasterData.Models.PartGroup group = MasterDataRepository.FindAllGroups(cp.CompanyId)
                        .Where(g => g.GroupId == product.GroupId).FirstOrDefault();

                    Unit unit = MasterDataRepository.FindAllUnits(cp.CompanyId)
                        .Where(u => u.UnitId == product.UnitId).FirstOrDefault();

                    Ccy ccy = MasterDataRepository.FindAllCurrencies(cp.CompanyId)
                        .Where(c => c.CcyId == product.CcyId).FirstOrDefault();

                    msg.GroupGUID = group.ModelGuid;
                    msg.UnitGUID = unit.ModelGuid;
                    msg.CcyCode = ccy.Kode;
                    new RabbitHelper().SendRegisterMasterData(msg);
                    return View("TambahProductSelesai");
                }

                LoadData();
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                LoadData();
                return View(product);
            }
        }

        private bool IsProductExist(IList<RequestProduct> listRequestProduct, IList<Product> listProduct, Product newProduct)
        {
            foreach (RequestProduct _product in listRequestProduct)
            {
                if (newProduct.Kode.Trim().ToLower().Equals(_product.Kode.ToLower()) ||
                    newProduct.Barcode.Trim().ToLower().Equals(_product.Barcode.ToLower()))
                    return true;
            }
            foreach (Product _product in listProduct)
            {
                if (newProduct.Kode.Trim().ToLower().Equals(_product.Kode.ToLower()) ||
                    newProduct.Barcode.Trim().ToLower().Equals(_product.Barcode.ToLower()))
                    return true;
            }
            return false;
        }

        private bool IsBarcodeExist(IList<RequestProduct> listRequestProduct, IList<Product> listProduct, Product newProduct)
        {
            foreach (RequestProduct _product in listRequestProduct)
            {
                if (newProduct.Barcode.Trim().ToLower().Equals(_product.Barcode.ToLower()))
                    return true;
            }
            foreach (Product _product in listProduct)
            {
                if (newProduct.Barcode.Trim().ToLower().Equals(_product.Barcode.ToLower()))
                    return true;
            }
            return false;
        }

        public ActionResult Edit(int tenanId, int productId)
        {
            Product product = MasterDataRepository.FindProductById(tenanId, productId);
            if (product != null)
                LoadDataForEditing(product);

            return View(product);
        }

        public JsonResult FindAllProductByCode(string kode, int tenanId)
        {
            IList<ProductPrint> _product = MasterDataRepository.FindAllProductByCode(kode.ToLower(), tenanId);

            return Json(_product, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                    ProductEditedMessage msg = new ProductEditedMessage
                    {
                        TenanId = product.TenanId,
                        ProductId = product.ProductId,
                        Barcode = product.Barcode,
                        Kode = product.Kode,
                        Nama = product.Nama,
                        HargaBeli = product.HargaBeli,
                        HargaJual = product.HargaJual,
                        GroupId = product.GroupId,
                        UnitId = product.UnitId,
                        CcyId = product.CcyId,
                        ProductGuid = product.ModelGuid,
                        StatusPrint = product.StatusPrint,
                        StatusProduct = product.StatusProduct
                    };

                    BonaStoco.AP1.MasterData.Models.PartGroup group = MasterDataRepository.FindAllGroups(cp.CompanyId)
                        .Where(g => g.GroupId == product.GroupId).FirstOrDefault();

                    Unit unit = MasterDataRepository.FindAllUnits(cp.CompanyId)
                        .Where(u => u.UnitId == product.UnitId).FirstOrDefault();

                    Ccy ccy = MasterDataRepository.FindAllCurrencies(cp.CompanyId)
                        .Where(c => c.CcyId == product.CcyId).FirstOrDefault();

                    msg.GroupGUID = group.ModelGuid;
                    msg.UnitGUID = unit.ModelGuid;
                    msg.CcyCode = ccy.Kode;

                    new RabbitHelper().SendMasterDataExchange<ProductEditedMessage>(msg);

                    return View("EditProductSelesai");
                }

                LoadDataForEditing(product);
                return View(product);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                LoadDataForEditing(product);
                return View(product);
            }
        }

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
        public PartialViewResult SearchProduct(string code)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(cp.CompanyId, code);
            return PartialView("_SearchProductResult", products);
        }
        public JsonResult CariBarangByID(string code)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(cp.CompanyId, code);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        #region Import Data
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ActionResult ImportProduct()
        {
            return View();
        }
        public ActionResult ImportPartgroup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadProductForImport(HttpPostedFileBase file)
        {
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                LoadProductByTenan(cp.CompanyId);

                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');

                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        string row = rows[rowIndex];

                        if (row == string.Empty)
                            continue;

                        if (IsProductAlreadyExist(row))
                            continue;

                        ImportProduct(cp, row);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }

            return View("ImportProductResult", response);
        }

        [HttpPost]
        public ActionResult UploadPartGroupForImport(HttpPostedFileBase file)
        {
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                LoadPartGroupByTenan(cp.CompanyId);

                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');

                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        string row = rows[rowIndex];
                        if (row == string.Empty)
                            continue;

                        if (IsGroupAlreadyExist(row))
                            continue;

                        ImportPartGroup(cp, row);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
            return View("ImportPartGroupResult", response);
        }

        private bool IsGroupAlreadyExist(string row)
        {
            string[] groupArr = row.Split(',');
            string code = groupArr[0].ToLower().Trim();
            BonaStoco.AP1.MasterData.Models.PartGroup existingGroup = partGroups.Where(m => m.Kode.ToLower().Trim() == code).FirstOrDefault();
            return existingGroup != null;
        }

        private void LoadPartGroupByTenan(int tenanId)
        {
            partGroups = MasterDataRepository.FindAllGroups(tenanId).ToList();
        }

        public JsonResult LoadPartGroupByTenan()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<BonaStoco.AP1.MasterData.Models.PartGroup> partgroups = MasterDataRepository.FindAllGroups(cp.CompanyId).ToList();
            return Json(partgroups, JsonRequestBehavior.AllowGet);
        }

        private bool IsProductAlreadyExist(string row)
        {
            string[] productArr = row.Split(',');
            string barcode = productArr[2].ToLower().Trim();
            string code = productArr[3].ToLower().Trim();
            Product existingProduct = products.Where(m =>
            {
                return m.Barcode.ToLower().Trim() == barcode ||
                       m.Kode.ToLower().Trim() == code;
            }).FirstOrDefault();
            return existingProduct != null;
        }

        private void LoadProductByTenan(int tenanid)
        {
            products = MasterDataRepository.FindAllProduct(tenanid).ToList();
        }

        private void ImportPartGroup(CompanyProfiles cp, string row)
        {
            try
            {
                string[] partGroupArr = row.Split(',');
                string groupCode = partGroupArr[0].Trim();
                string groupName = partGroupArr[1].Trim();

                TambahPartGroupMessage msg = null;
                msg = new TambahPartGroupMessage()
                {
                    TenanId = cp.CompanyId,
                    Kode = groupCode,
                    Nama = groupName,
                    ModelGuid = Guid.NewGuid()
                };
                new RabbitHelper().SendMasterDataExchange<TambahPartGroupMessage>(msg);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
        }
        private void ImportProduct(CompanyProfiles cp, string row)
        {
            try
            {
                string[] productArr = row.Split(',');
                string groupCode = productArr[0].ToLower().Trim();
                string unitCode = productArr[1].ToLower().Trim();
                string ccyCode = productArr[6].ToLower().Trim();

                MasterData.Models.PartGroup partGroup = MasterDataRepository.FindAllGroups(cp.CompanyId).Where(m => m.Kode.ToLower() == groupCode).FirstOrDefault();
                if (partGroup == null)
                    throw new ApplicationException("Partgroup dengan kode " + groupCode + " tidak ditemukan dalam database.");

                MasterData.Models.Unit unit = MasterDataRepository.FindAllUnits(cp.CompanyId).Where(m => m.Kode.ToLower() == unitCode).FirstOrDefault();
                if (unit == null)
                    throw new ApplicationException("Unit dengan kode " + unitCode + " tidak ditemukan dalam database.");

                MasterData.Models.Ccy ccy = MasterDataRepository.FindAllCurrencies(0).Where(m => m.Kode.ToLower() == ccyCode).FirstOrDefault();
                if (ccy == null)
                    throw new ApplicationException("Mata uang " + ccyCode + " tidak ditemukan dalam database.");

                BonaStoco.AP1.Web.Messages.RegisterProductMessage msg = new BonaStoco.AP1.Web.Messages.RegisterProductMessage()
                {
                    TenanId = cp.CompanyId,
                    Barcode = productArr[2].Trim(),
                    Kode = productArr[3].Trim(),
                    Nama = productArr[4].Trim(),
                    HargaBeli = 1,
                    HargaJual = Decimal.Parse(productArr[5].Trim()),
                    GroupGUID = partGroup.ModelGuid,
                    CcyCode = productArr[6].Trim(),
                    UnitGUID = unit.ModelGuid,
                    ProductGuid = Guid.NewGuid(),
                    StatusPrint = true,
                    GroupId = partGroup.GroupId,
                    UnitId = unit.UnitId,
                    CcyId = ccy.CcyId,
                    StatusProduct = true
                };
                rabbitHelper.SendRegisterMasterData(msg);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
        }
        private bool UploadedFileToImportContainsData()
        {
            return uploadedFileToImport.ContentLength > 0;
        }
        private void FailIfContentTypeNotCSV()
        {
            if (uploadedFileToImport.ContentType == "text/csv")
                return;
            if (uploadedFileToImport.ContentType == "text/plain")
                return;
            if (uploadedFileToImport.ContentType == "application/vnd.ms-excel")
                return;
            if (uploadedFileToImport.ContentType == "application/octet-stream")
                return;

            throw new ApplicationException("Format file harus dalam CSV");
        }

        #endregion

        #region Import Edit Barang

        public ActionResult ImportEditBarang()
        {
            return View();
        }
        public ActionResult UploadEditProductForImport(HttpPostedFileBase file)
        {
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');

                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        string row = rows[rowIndex];
                        string code = row.Split(',')[0].Trim();

                        if (row == string.Empty)
                            continue;
                        Product product = MasterDataRepository.FindProductByCode(cp.CompanyId, code);
                        if (product == null)
                        {
                            response.HasError = true;
                            response.ErrorMessages.Add("Barang dengan kode " + code + " tidak ditemukan.");
                        }
                        else
                            ImportEditProduct(cp, row, product);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
            return View("ImportEditBarangResult", response);
        }

        private void ImportEditProduct(CompanyProfiles cp, string row, Product product)
        {
            try
            {
                string[] editProductArr = row.Split(',');
                string nama = editProductArr[1];
                string hargaJual = editProductArr[2];
                string barcode = editProductArr[3];

                ProductEditedMessage msg = new ProductEditedMessage
                {
                    TenanId = product.TenanId,
                    ProductId = product.ProductId,
                    Barcode = barcode,
                    Kode = product.Kode,
                    Nama = nama,
                    HargaBeli = product.HargaBeli,
                    HargaJual = Decimal.Parse(hargaJual),
                    GroupId = product.GroupId,
                    UnitId = product.UnitId,
                    CcyId = product.CcyId,
                    ProductGuid = product.ModelGuid,
                    StatusPrint = product.StatusPrint
                };

                BonaStoco.AP1.MasterData.Models.PartGroup group = MasterDataRepository.FindAllGroups(cp.CompanyId)
                        .Where(g => g.GroupId == product.GroupId).FirstOrDefault();
                if (group == null)
                    throw new ApplicationException("Group untuk barang dengan kode " + product.Kode + " tidak ditemukan.");

                Unit unit = MasterDataRepository.FindAllUnits(cp.CompanyId)
                    .Where(u => u.UnitId == product.UnitId).FirstOrDefault();
                if (unit == null)
                    throw new ApplicationException("Unit untuk barang dengan kode " + product.Kode + " tidak ditemukan.");

                Ccy ccy = MasterDataRepository.FindAllCurrencies(cp.CompanyId)
                    .Where(c => c.CcyId == product.CcyId).FirstOrDefault();
                if (ccy == null)
                    throw new ApplicationException("Mata uang untuk barang dengan kode " + product.Kode + " tidak ditemukan.");

                msg.GroupGUID = group.ModelGuid;
                msg.UnitGUID = unit.ModelGuid;
                msg.CcyCode = ccy.Kode;
                new RabbitHelper().SendMasterDataExchange<ProductEditedMessage>(msg);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
        }

        #endregion

        #region Export Barang
        public ActionResult ExportProduct()
        {
            return View();
        }
        public FileContentResult ExportFormatNewProduct()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = MasterDataRepository.FindAllProduct(cp.CompanyId);
            if (products.Count == 0)
                return null;
            string csv = "Partgroup,Unit,Barcode,Code,Nama,HargaJual,CcyCode,Status Print\r\n";
            foreach (Product prod in products)
            {
                BonaStoco.AP1.MasterData.Models.PartGroup group = MasterDataRepository.FindAllGroups(cp.CompanyId)
                        .Where(g => g.GroupId == prod.GroupId).FirstOrDefault();
                Unit unit = MasterDataRepository.FindAllUnits(cp.CompanyId)
                    .Where(u => u.UnitId == prod.UnitId).FirstOrDefault();
                Ccy ccy = MasterDataRepository.FindAllCurrencies(cp.CompanyId)
                    .Where(c => c.CcyId == prod.CcyId).FirstOrDefault();
                csv += group.Kode + ',';
                csv += unit.Kode + ',';
                csv += prod.Barcode + ",";
                csv += prod.Kode + ",";
                csv += prod.Nama + ',';
                csv += prod.HargaJual.ToString() + ",";
                csv += ccy.Kode + ',';
                csv += prod.StatusPrint + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Product Tenan-" + cp.CompanyName + ".txt");
        }
        public ActionResult ExportEditProduct()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            ViewBag.GroupId = new SelectList(MasterDataRepository.FindAllGroups(cp.CompanyId), "GroupId", "Nama");
            return View();
        }
        public FileContentResult ExportFormatEditProduct(string groupId)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = new List<Product>();
            BonaStoco.AP1.MasterData.Models.PartGroup partGroup = new BonaStoco.AP1.MasterData.Models.PartGroup();
            if (groupId.Equals("all"))
            {
                products = MasterDataRepository.FindAllProduct(cp.CompanyId);
                partGroup.Nama = "All Group";
            }
            else
            {
                products = MasterDataRepository.FindProductByGroupId(cp.CompanyId, Int32.Parse(groupId));
                partGroup = MasterDataRepository.GetPartGroupById(Int32.Parse(groupId), cp.CompanyId);
            }

            if (products.Count == 0)
                return null;

            string csv = "Code,Nama,HargaJual,Barcode\r\n";
            foreach (Product prod in products)
            {
                csv += prod.Kode + ',';
                csv += prod.Nama + ',';
                csv += prod.HargaJual.ToString() + ',';
                csv += prod.Barcode + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Edit Product Tenant-" + cp.CompanyName + " With Group(" + partGroup.Nama + ").txt");
        }
        #endregion
        #region Import Data Dari AP
        [Authorize(Roles = APRoles.AP_ROLES)]
        public ViewResult APImportData()
        {
            return View("APImportProduct");
        }
        public ActionResult APUploadProductForImport(string tenanId, HttpPostedFileBase file)
        {
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                LoadProductByTenan(Int32.Parse(tenanId));

                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');

                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        string row = rows[rowIndex];

                        if (row == string.Empty)
                            continue;

                        if (IsProductAlreadyExist(row))
                            continue;

                        APImportProduct(Int32.Parse(tenanId), row);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }

            return View("APImportProductResult", response);
        }
        private void APImportProduct(int tenanId, string row)
        {
            try
            {
                string[] productArr = row.Split(',');
                string groupCode = productArr[0].ToLower().Trim();
                string unitCode = productArr[1].ToLower().Trim();
                string ccyCode = productArr[6].ToLower().Trim();

                MasterData.Models.PartGroup partGroup = MasterDataRepository.FindAllGroups(tenanId).Where(m => m.Kode.ToLower() == groupCode).FirstOrDefault();
                if (partGroup == null)
                    throw new ApplicationException("Partgroup dengan kode " + groupCode + " tidak ditemukan dalam database.");

                MasterData.Models.Unit unit = MasterDataRepository.FindAllUnits(tenanId).Where(m => m.Kode.ToLower() == unitCode).FirstOrDefault();
                if (unit == null)
                    throw new ApplicationException("Unit dengan kode " + unitCode + " tidak ditemukan dalam database.");

                MasterData.Models.Ccy ccy = MasterDataRepository.FindAllCurrencies(0).Where(m => m.Kode.ToLower() == ccyCode).FirstOrDefault();
                if (ccy == null)
                    throw new ApplicationException("Mata uang " + ccyCode + " tidak ditemukan dalam database.");

                BonaStoco.AP1.Web.Messages.TambahProductMessage msg = new BonaStoco.AP1.Web.Messages.TambahProductMessage()
                {
                    TenanId = tenanId,
                    Barcode = productArr[2].Trim(),
                    Kode = productArr[3].Trim(),
                    Nama = productArr[4].Trim(),
                    HargaBeli = 1,
                    HargaJual = Decimal.Parse(productArr[5].Trim()),
                    GroupGUID = partGroup.ModelGuid,
                    CcyCode = productArr[6].Trim(),
                    UnitGUID = unit.ModelGuid,
                    ProductGuid = Guid.NewGuid(),
                    StatusPrint = true,
                    GroupId = partGroup.GroupId,
                    UnitId = unit.UnitId,
                    CcyId = ccy.CcyId,
                    StatusProduct = true
                };
                rabbitHelper.SendMasterDataExchange<TambahProductMessage>(msg);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
        }
        #endregion
    }
}