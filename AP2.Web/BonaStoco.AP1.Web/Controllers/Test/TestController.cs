using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.MasterData.Repository;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.Web.Models;
using System.IO;
using BonaStoco.AP1.PengirimanBarang.Models;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.Context;
using RabbitMQ.Client;
using BonaStoco.Inf.DataMapper;
using BonaStoco.Inf.ExceptionUtils;
namespace BonaStoco.AP1.Web.Controllers
{
    //[Authorize(Roles="Tester")]
    public class TestController : Controller
    {
        //
        // GET: /Test/
        RabbitHelper rabbitHelper;

        public TestController()
        {
            rabbitHelper = new RabbitHelper();
            response = new ImportProductResponse() { HasError = false, ErrorMessages = new List<String>() };
        }

        public ActionResult Index()
        {
            return View();
        }

        public string SayHello(string id)
        {
            return "Hello, World";
            //BonastocoServices ws = new BonaStoco.AP1.Web.tenantws.BonastocoServices();
            //serverResponse response = ws.gettenant(new askTenant { tenantid = id });
            //return string.Format("Message: {0}, Status: {1}", response.message, response.status);
        }

        public JsonResult TestJson()
        {
            return Json(new Foo { Id = 2240, Name = "PT. ABC" });
        }

        public JsonResult LoadProducts()
        {
            IMasterDataRepository mrep = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
            return Json(mrep.FindAllProduct(2240), JsonRequestBehavior.AllowGet);
        }

        public string RegisterProduct()
        {
            RegisterProductMessage msg = new RegisterProductMessage()
            {
                Barcode = "111",
                CcyCode = "abc",
                CcyId = 123,
                GroupId = 1,
                HargaBeli = 6500,
                HargaJual = 6400,
                Kode = "12345",
                Nama = "Vinsa",
                ProductGuid = Guid.NewGuid(),
                ProductId = 1313,
                StatusPrint = true,
                TenanId = 4083,
                UnitId = 1234,
                UnitGUID = Guid.NewGuid(),
                GroupGUID = Guid.NewGuid()
            };
            new RabbitHelper().SendRegisterMasterData(msg);
            return "Tess berhasil.";
        }
        public string ApproveProduct()
        {
            ApproveProductMessage msg = new ApproveProductMessage()
            {
                TenanId = 4083,
                ProductId = 1313,
                ProductGuid = Guid.NewGuid()
            };
            new RabbitHelper().SendApproveMasterData(msg);
            return "Test Berhasil.";
        }

        public ActionResult RegisterTenant()
        {
            return View();
        }

        [HttpPost]
        public string RegisterAllTenant()
        {
            try
            {
                List<string> failed = new List<string>();
                IApplicationContext context;
                string MASTER_DATA_EXCHANGE = "masterData.topic";
                string PENGIRIMAN_BARANG_EXCHANGE = "pengirimanBarang.topic";
                string EXCHANGE_RATE_EXCHANGE = "exchangeRate.topic";
                string EXCHANGE_RATE_ROUTING_KEY = "alltenant";

                string filename = string.Format("{0}{1}{2}",
                    Server.MapPath("~/App_Data"),
                    Path.DirectorySeparatorChar,
                    "listtenant.csv");

                StreamReader sr = new StreamReader(filename);
                context = ContextRegistry.GetContext();
                RabbitTemplate template = context.GetObject("RabbitTemplate") as RabbitTemplate;
                string[] tenantList = sr.ReadToEnd().Split('\r','\n');
                foreach (string tenant in tenantList)
                {
                    if (tenant == string.Empty) 
                        continue;

                    BonaStocoWSLogon wsLogon = new BonaStocoWSLogon();
                    string username = tenant.Split(',')[5];
                    wsLogon.Login(username, "123456");
                    if (!wsLogon.IsAuthenticated)
                        failed.Add(tenant + " -> " + wsLogon.ErrorMessage);
                    template.Execute<object>(delegate(IModel model)
                    {
                        string queueName = GetQueueName(wsLogon.Response.companyid);
                        model.QueueDeclare(queueName, true);
                        model.QueueBind(queueName, MASTER_DATA_EXCHANGE, queueName, false, null);
                        model.QueueBind(queueName, PENGIRIMAN_BARANG_EXCHANGE, queueName, false, null);
                        model.QueueBind(queueName, EXCHANGE_RATE_EXCHANGE, EXCHANGE_RATE_ROUTING_KEY, false, null);
                        return null;
                    });
                }

                if (failed.Count > 0)
                {
                    string result = "<p>Failed:</p>";
                    foreach (string s in failed)
                    {
                        result += s + "<br/><br/>";
                    }
                    return result;
                }

                return "Successfull.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string GetQueueName(int companyId)
        {
            return string.Format("ap1.{0}", companyId.ToString());
        }

        public ViewResult ImportProductRecovery()
        {
            IList<RecoverTenanModel> tenants = GetRecoverTenantList();
            return View(tenants);
        }

        private IList<RecoverTenanModel> GetRecoverTenantList()
        {
            string filename = string.Format("{0}{1}{2}",
                        Server.MapPath("~/App_Data"),
                        Path.DirectorySeparatorChar,
                        "RecoverTenant.csv");

            StreamReader sr = new StreamReader(filename);
            string[] tenantsArr = sr.ReadToEnd().Split('\r', '\n');
            IList<RecoverTenanModel> tenants = new List<RecoverTenanModel>();
            foreach (string tenant in tenantsArr)
            {
                if (tenant == string.Empty)
                    continue;

                string[] tenantArr = tenant.Split(',');
                tenants.Add(new RecoverTenanModel { Code = tenantArr[0], Name = tenantArr[1] });
            }
            return tenants;
        }

        public JsonResult FindTenantsToRecover()
        {
            IList<RecoverTenanModel> result = GetRecoverTenantList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecoverTenan(int tenantId)
        {
            RecoverImportProductResponse response = new RecoverImportProductResponse { HasError = false, Message = "", TenantId = tenantId };
            
            try
            {
                IList<Product> products = MasterDataRepository.FindAllProduct(tenantId);
                if (products != null)
                {
                    foreach (Product prod in products)
                    {
                        SendMessageRecovery(prod, tenantId);
                    }
                }
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = ex.GetInnermostException().Message;
            }
            
            return Json(response);
        }

        public void SendMessageRecovery(Product product, int tenantId)
        {
            MasterData.Models.PartGroup partGroup = MasterDataRepository.FindAllGroups(tenantId).Where(m => m.GroupId == product.GroupId).FirstOrDefault();
            MasterData.Models.Unit unit = MasterDataRepository.FindAllUnits(tenantId).Where(m => m.UnitId == product.UnitId).FirstOrDefault();
            MasterData.Models.Ccy ccy = MasterDataRepository.FindAllCurrencies(0).Where(m => m.CcyId == product.CcyId).FirstOrDefault();

            BonaStoco.AP1.Web.Messages.TambahProductMessage msg = new BonaStoco.AP1.Web.Messages.TambahProductMessage()
            {
                TenanId = product.TenanId,
                Barcode = product.Barcode,
                Kode = product.Kode,
                Nama = product.Nama,
                HargaBeli = 1,
                HargaJual = product.HargaJual,
                GroupGUID = partGroup.ModelGuid,
                CcyCode = ccy.Kode,
                UnitGUID = unit.ModelGuid,
                ProductGuid = product.ModelGuid,
                StatusPrint = product.StatusPrint,
                GroupId = partGroup.GroupId,
                UnitId = unit.UnitId,
                CcyId = ccy.CcyId
            };

            rabbitHelper.SendImportProductMessageToTenant(tenantId, msg);
        }


        ImportProductResponse reponse=new ImportProductResponse(){ HasError = false, ErrorMessages = new List<String>()};
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
        #region Import Pengiriman Barang
        public ActionResult ImportPengirimanBarang()
        {
            return View();
        }

        HttpPostedFileBase uploadedFileToImport;
        ImportProductResponse response;

        [HttpPost]
        public ActionResult ImportPengirimanBarang(HttpPostedFileBase file)
        {
            IList<GRNItemModel> item = new List<GRNItemModel>();
            try
            {
                this.uploadedFileToImport = file;
                FailIfContentTypeNotCSV();
                CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                cp.CompanyId = 4083;
                using (StreamReader sr = new StreamReader(uploadedFileToImport.InputStream))
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
            return View();
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
                BonaStoco.AP1.Web.Models.PengirimanBarang pb = new BonaStoco.AP1.Web.Models.PengirimanBarang(this.HttpContext);
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
    }

    class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}