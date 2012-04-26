using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles=APRoles.TENANT_ROLES)]
    public class UploadSalesController : Controller
    {
        UploadSalesResponse response = new UploadSalesResponse();
        HttpPostedFileBase uploadedFile;

        //
        // GET: /UploadSales/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                uploadedFile = file;
                FailIfContentTypeNotCSV();
                if (UploadedFileContainsData())
                    SendDataSalesToRabbit();
                else
                    GenerateFileEmptyError();
            }
            catch(Exception ex)
            {
                GenerateUnExpectedError(ex);
            }

            return View("Result",response);
        }

        private void FailIfContentTypeNotCSV()
        {
            if (uploadedFile.ContentType == "text/csv")
                return;
            if (uploadedFile.ContentType == "application/vnd.ms-excel")
                return;
            if (uploadedFile.ContentType == "application/octet-stream")
                return;

            throw new ApplicationException("Format file harus dalam CSV");
        }
        private bool UploadedFileContainsData()
        {
            return uploadedFile.ContentLength > 0;
        }
        private void SendDataSalesToRabbit()
        {
            StreamReader sr = new StreamReader(uploadedFile.InputStream);
            string content = sr.ReadToEnd();
            
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

            //IList<ShoppingCartCheckedOut> scList = content.ToShoppingCartCheckedOut(cp.CompanyId, 0);

            //RabbitHelper rabbit = new RabbitHelper();
            //foreach (ShoppingCartCheckedOut sc in scList)
            //    rabbit.SendPOSTransactionMessage(sc);

            UploadSalesMessage msg = content.ToUploadSalesMessagee();
            msg.CompanyReserved = cp.CompanyReserved;
            msg.TenanId = cp.CompanyId;
            msg.LocationId = GetLocationId(cp.CompanyId);
            new RabbitHelper().SendUploadSalesMessage(msg);

            string filePath = string.Format("{0}{1}{2}-{3}-{4}",
                Server.MapPath("~/App_Data/Uploads"),
                Path.DirectorySeparatorChar,
                cp.CompanyId.ToString(),
                Guid.NewGuid(),
                uploadedFile.FileName);
            uploadedFile.SaveAs(filePath);
        }
        private void GenerateFileEmptyError()
        {
            response.HasError = true;
            response.ErrorMessage = "File kosong ( tidak ada data )";
        }
        private void GenerateUnExpectedError(Exception ex)
        {
            response.HasError = true;
            response.ErrorMessage = ex.Message;
        }
        private int GetLocationId(int tenanId)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(tenanId);
            return tenan.LocationId;
        }
        public IMasterDataRepository MasterDataRepository
        {
            get
            {
                return (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
            }
        }
    }
}