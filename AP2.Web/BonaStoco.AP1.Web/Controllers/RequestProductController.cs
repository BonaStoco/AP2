using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.Web.Models;
using Newtonsoft.Json;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class RequestProductController : Controller
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
        //
        // GET: /VerifikasiBarang/

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTenans()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().GetAllTenan();
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandara(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }

            return Json(tenans, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataProduct()
        {

            return Json(new object { }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPartMaster()
        {
            IList<RequestProduct> apPart = MasterDataRepository.FindAllProductPending();
            return Json(apPart, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindAllProductPendingByGroupTenan()
        {
            IList<ProductHeader> apParts = MasterDataRepository.FindAllProductPendingByGroupTenan();
            return Json(apParts, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindPartPendingByTenanId(string tenantId)
        {
            IList<RequestProduct> apParts = MasterDataRepository.FindAllProductPendingByTenanId(Int32.Parse(tenantId));
             return Json(apParts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StatusAprove(string guidId)
        {
            RequestProduct apPart = MasterDataRepository.FindProductAproveByGuidId(guidId);

            ApproveProductMessage msg = new ApproveProductMessage()
            {
                 ProductGuid=apPart.ModelGuid,
                  ProductId= apPart.ProductId,
                   TenanId= apPart.TenanId
            };
            new RabbitHelper().SendApproveMasterData(msg);
            return Json(apPart, JsonRequestBehavior.AllowGet);
        }


        public JsonResult StatusReject(string guidId)
        {
            RequestProduct apPart = MasterDataRepository.FindProductAproveByGuidId(guidId);

            RejectProductMessage msg = new RejectProductMessage()
            {
                ProductGuid = apPart.ModelGuid,
                ProductId = apPart.ProductId,
                TenanId = apPart.TenanId
            };
            new RabbitHelper().SendRejectMasterData(msg);
            return Json(apPart, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductPending()
        {
            IList<RequestProduct> pendingProduct = MasterDataRepository.FindAllProductPending();
            int countPendingProduct = pendingProduct.Count;
            return Json(countPendingProduct, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountPendingRequestedProduct()
        {
            PendingProductRequestCount pendingProductRequest = MasterDataRepository.CountPendingRequestedProduct();
            return Json(pendingProductRequest.Count, JsonRequestBehavior.AllowGet);
        }

        public void RejectAllProduct()
        {
            IList<RequestProduct> pendingProduct = MasterDataRepository.FindAllProductPending();
            foreach (RequestProduct reqProd in pendingProduct)
            {
               // RequestProduct requestProduct = JsonConvert.DeserializeObject<RequestProduct>(reqProd);
                MasterDataRepository.RejectAllRequestProduct(reqProd);
            }           
          
        }

        public void RejectAllProductbyTenanId(int tenantId)
        {
            IList<RequestProduct> pendingProduct = MasterDataRepository.FindAllProductPendingByTenanId(tenantId);
            foreach (RequestProduct reqProd in pendingProduct)
            {
                // RequestProduct requestProduct = JsonConvert.DeserializeObject<RequestProduct>(reqProd);
                MasterDataRepository.RejectAllRequestProduct(reqProd);
            }

        }
        public void ApproveAllProductbyTenanId(int tenantId)
        {
            try
            {
                IList<RequestProduct> pendingProduct = MasterDataRepository.FindAllProductPendingByTenanId(tenantId);
                foreach (RequestProduct reqProd in pendingProduct)
                {
                    ApproveProductMessage msg = new ApproveProductMessage()
                    {
                        ProductGuid = reqProd.ModelGuid,
                        ProductId = reqProd.ProductId,
                        TenanId = reqProd.TenanId
                    };
                    new RabbitHelper().SendApproveMasterData(msg);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public void ApproveAllProduct()
        {
            try
            {
                IList<RequestProduct> pendingProduct = MasterDataRepository.FindAllProductPending();
                foreach (RequestProduct reqProd in pendingProduct)
                {
                    ApproveProductMessage msg = new ApproveProductMessage()
                    {
                        ProductGuid = reqProd.ModelGuid,
                        ProductId = reqProd.ProductId,
                        TenanId = reqProd.TenanId
                    };
                    new RabbitHelper().SendApproveMasterData(msg);
                }                
            }
            catch (Exception ex)
            {
                
                 throw ex;
            }         
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
                
    }
}