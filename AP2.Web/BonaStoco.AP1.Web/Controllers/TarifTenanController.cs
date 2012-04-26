using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.Web.Models;
using System.IO;
using BonaStoco.Inf.ExceptionUtils;
namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES + "," + APRoles.Umum_ROLES)]
    public class TarifTenanController : Controller
    {
        public TarifTenanController()
        {
            rabbitHelper = new RabbitHelper();
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

        //
        // GET: /CompanyProfileAP1/

        public ActionResult Index()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            if (APRoles.IsRoot(cp.RoleName))
            {
                IList<Tenan> ap = MasterDataRepository().FindAllTenanByCategoryId(cp.Role.Category);
                return View("../TarifTenan/index", ap);
            }
            else if(APRoles.IsBandara(cp.RoleName))
            {
                IList<Tenan> bandara = MasterDataRepository().FindTenantByBandara(cp.Role.Bandara);
                return View("../TarifTenan/Bandara", bandara);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                IList<Tenan> subterminal = MasterDataRepository().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
                return View("../TarifTenan/SubTerminal", subterminal);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                IList<Tenan> terminal = MasterDataRepository().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
                return View("../TarifTenan/Terminal", terminal);
            }
          
            return View();
        }
        
        private void LoadDataForEditing(Tenan t)
        {
            ViewBag.TenanTypeId = new SelectList(MasterDataRepository().FindTenanType(), "TenanTypeId", "TenanTypeName", t.TenanTypeId);
            ViewBag.ProductTypeId = new SelectList(MasterDataRepository().FindProductType(), "ProductTypeId", "ProductTypeName", t.ProductTypeId);
            ViewBag.CcyCode = new SelectList(MasterDataRepository().FindCcyCode(), "CcyCode", "CcyCode", t.CcyCode);
        }   

        public ActionResult Edit(int id)
        {
            Tenan tenan = MasterDataRepository().FindTenanById(id);
            if (tenan != null)
                LoadDataForEditing(tenan);

            return View(tenan);
        }

        [HttpPost]
        public ActionResult Edit(Tenan tenan)
        {
            try
            {
                
                    TenanEditedMessage msg = new TenanEditedMessage()
                    {
                        TenanId = tenan.TenanId,
                        TenanName = tenan.TenanName,
                        Alamat = tenan.Alamat,
                        LocationId = tenan.LocationId,
                        Nppkp = tenan.Nppkp,
                        Npwp = tenan.Npwp,
                        TerminalId = tenan.TerminalId,
                        SubTerminalId = tenan.SubTerminalId,
                        TanggalBergabung = tenan.TanggalBergabung,
                        Tarif = tenan.Tarif,
                        CategoryId = tenan.CategoryId,
                        TenanTypeId = tenan.TenanTypeId,
                        ProductTypeId = tenan.ProductTypeId,
                        Gate = tenan.Gate,
                        Target = tenan.Target,
                        CcyCode = tenan.CcyCode,
                        FormulaKonsesi = tenan.FormulaKonsesi.ToLower()
                        
                    };

                    new RabbitHelper().SendTenanEditedMessage(msg);
                    return View("update");
               
            }
            catch (Exception)
            {
                
                throw;
            }
                
        }

        #region Update tenant

        [Authorize(Roles = APRoles.AP_ROLES)]
        public ViewResult ImportEditTenant()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = APRoles.AP_ROLES)]
        public ActionResult ImportEditTenant(HttpPostedFileBase file)
        {
            try
            {
                response = new ImportProductResponse() { HasError = false, ErrorMessages = new List<String>() };
                uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
                {
                    string content = sr.ReadToEnd().Trim();
                    string[] rows = content.Split('\r', '\n');

                    for (int rowIndex = 1; rowIndex < rows.Length; rowIndex++)
                    {
                        if (rows[rowIndex] == string.Empty)
                            continue;

                        string[] tenanDataRow = rows[rowIndex].Split(',');

                        if (IsTenanRegistered(Int32.Parse(tenanDataRow[0])))
                            UpdateTenant(tenanDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.ErrorMessages.Add(ex.GetInnermostException().Message);
            }
            return View("ImportEditTenantResult", response);
        }
        public void UpdateTenant(string[] tenanDataRow)
        {
            int tenanId = Int32.Parse(tenanDataRow[0]);
            string tenanName = tenanDataRow[1];
            string tenanAddress = tenanDataRow[2];
            string npwp = tenanDataRow[3];
            decimal tarif = Decimal.Parse(tenanDataRow[4].Replace("%", ""));
            string tenanType = tenanDataRow[5];
            string productType = tenanDataRow[6];
            string ccy = tenanDataRow[7];
            string gate = tenanDataRow[8];
            int tenanTypeId = 0;
            int productTypeId = 0;
            string ccyCode = "IDR";

            tenanAddress = tenanAddress.Replace(',', ' ');
            tenanAddress = tenanAddress.Replace("\r\n", "-");
            Tenan tenan = MasterDataRepository().FindTenanById(tenanId);
            TenanType _tenanType = MasterDataRepository().FindTenanTypeByName(tenanType.ToLower());
            ProductType _productType = MasterDataRepository().FindProductTypeByName(productType.ToLower());
            FindCcyCode _ccy = MasterDataRepository().FindCcyCodeByName(ccy.ToLower());
            if (_tenanType != null)
                tenanTypeId = _tenanType.TenanTypeId;
            if (_productType != null)
                productTypeId = _productType.ProductTypeId;
            if (_ccy != null)
                ccyCode = _ccy.CcyCode;
            TenanEditedMessage editTenanMessage = new TenanEditedMessage
            {
                TenanId = tenanId,
                TenanName = tenanName,
                Alamat = tenanAddress,
                Npwp = npwp,
                Tarif = tarif,

                CategoryId = tenan.CategoryId,
                LocationId = tenan.LocationId,
                TerminalId = tenan.TerminalId,
                SubTerminalId = tenan.SubTerminalId,
                Gate = gate,

                ProductTypeId = productTypeId,
                TanggalBergabung = tenan.TanggalBergabung,
                TenanTypeId = tenanTypeId,
                Nppkp = tenan.Nppkp,
                CcyCode = ccyCode
            };
            rabbitHelper.SendTenanEditedMessage(editTenanMessage);
        }
        private bool IsTenanRegistered(int tenanId)
        {
            Tenan registeredTenan = MasterDataRepository().FindTenanById(tenanId);
            return registeredTenan != null;
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

        HttpPostedFileBase uploadedFileToImport;
        ImportProductResponse response;
        IMasterDataRepository masterDataRepo;
        RabbitHelper rabbitHelper;

        #endregion
    }
}