using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;


namespace BonaStoco.AP1.Web.Controllers
{
    public class ReportStockController : Controller
    {
        //
        // GET: /ReportStock/
        IAPStockRepository _stockRepo= new APStockRepository();

        [Authorize(Roles = APRoles.AP_ROLES)]
        public ActionResult ReportStock()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }
       
        [Authorize(Roles=APRoles.TENANT_ROLES)]
        public ViewResult ReportStockTenan()
        {
            CompanyProfiles dataTenan = new CompanyProfiles(this.HttpContext);
            return View(dataTenan);
        }

        [Authorize]
        public JsonResult FindGroupNameByTenanId(int id)
        {
            var list = new InventoryRepository().FindPartGroupByTenanId(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult FindCompanyByTenanId(int id)
        {
           string[] tenanMessage= GetTenanWebService(id);
           return Json(tenanMessage, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult FindStockCardByGroupName(string id)
        {
            string _id = id;
            string[] idmessage = _id.Split(',');
            IList<StockCard> stockCard = _stockRepo.FindStockCardByGroupName(idmessage[0],idmessage[1]);
            return Json(stockCard, JsonRequestBehavior.AllowGet);
        }

        [Authorize]        
        public JsonResult FindStockCardByTenanId(int id)
        {
            IList<StockCard> stockCard = _stockRepo.FindStockCardByTenanId(id);
            return Json(stockCard, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult FindStockCardDetailByCode(string tenantId,string code)
        {
            int _tenantId = int.Parse(tenantId);
            Product prod= MasterDataRepository().FindProductByCode(_tenantId,code);
            string productId = prod.ModelGuid.ToString();
            IList<BonaStoco.AP1.Inventory.Models.StockCardDetail> stockDetail = new InventoryMongoRepository().FindStockCardItemByDate(_tenantId, productId,  DateTime.UtcNow.Date).ToList();
           return Json(stockDetail, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductIdStockByTenanAndGroup(string tenantid, string groupid)
        {
            var list = new InventoryRepository().FindProductByGroupAndTenanId(Int32.Parse(tenantid), Int32.Parse(groupid));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private string[] GetTenanWebService(int txtTenanId)
        {
            var tenan = new BonaStoco.AP1.Web.ReportingRepository.tenanws.BonastocoServices().gettenant(
                new BonaStoco.AP1.Web.ReportingRepository.tenanws.askTenant() { tenantid = txtTenanId.ToString(), token = "" });
            string[] tenanMessage = tenan.message.Split(new char[] { ';', '=' });
            return new string[2] { txtTenanId.ToString(), tenanMessage[1] };
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
    }
}