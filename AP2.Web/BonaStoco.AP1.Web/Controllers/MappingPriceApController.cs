using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.ReportingRepository.MappingPriceRepository;

namespace BonaStoco.AP1.Web.Controllers
{
    [HandleError]
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class MappingPriceApController : Controller
    {
        IMasterDataRepository repo = null;
        IMappingPriceRepository repoMapping = new MappingPriceRepository();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult TenanLounge(string tenanId)
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTenans()
        {
            IList<TenanLounge> tenans = new List<TenanLounge>();
            tenans = TenanAdvSearchRepository().FindTenanLounge();
            return Json(tenans, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(id);
            if (tenan.MappingPrice == false)
            {
                return Json("Tenant yang anda masukan bukan tenan lounge", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenanByName(string key)
        {
            IList<TenanLounge> tenans = new List<TenanLounge>();
            tenans = TenanAdvSearchRepository().FindTenanLoungeByName(key);
            return Json(tenans, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListProduct(string tenanId)
        {
            IList<Product> product = new List<Product>();
            product = MasterDataRepository.FindProductFilteredByMappedPrice(Int32.Parse(tenanId), getMappingPriceByTenanId(tenanId));//MasterDataRepository.FindAllProduct(Int32.Parse(tenanId));
            return Json(product, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetProductById(string tenanId, string id)
        { 
            Product product = new Product();
            product = MasterDataRepository.FindProductById(Int32.Parse(tenanId), Int32.Parse(id));
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddToMappingPrice(string tenanId, string productId, string harga, string productguid)
        {
            MappingPrice mappingprice = new MappingPrice()
            {
                Id = Guid.NewGuid(),
                ProductGuid = Guid.Parse(productguid),
                Price = decimal.Parse(harga),
                ProductId = Int32.Parse(productId),
                TenanId = Int32.Parse(tenanId)
            };
            repoMapping.AddItem(mappingprice);
            return Json(getMappingPriceByTenanId(tenanId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListMappingPrice(string tenanId)
        {
            return Json(getMappingPriceByTenanId(tenanId), JsonRequestBehavior.AllowGet);
        }

        private IList<MappingPriceList> getMappingPriceByTenanId(string tenanId)
        {
            IList<MappingPriceList> mappingpriceList = new List<MappingPriceList>();
            return repoMapping.GetMappingPriceByTenanId(Int32.Parse(tenanId));
        }

        public JsonResult GetMappingPriceById(string id)
        {
            MappingPriceList mappingPrice = new MappingPriceList();
            mappingPrice = repoMapping.GetMappingByGuidId(id);
            return Json(mappingPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateToMappingPrice(string id, string tenanId, string productId, string harga, string productguidUpdate)
        {
            MappingPrice updateMappingPrice = new MappingPrice()
            {
                Id = Guid.Parse(id),
                ProductGuid = Guid.Parse(productguidUpdate),
                TenanId = Int32.Parse(tenanId),
                ProductId = Int32.Parse(productId),
                Price = decimal.Parse(harga)
            };
            repoMapping.UpdateItem(updateMappingPrice);
            MappingPriceList mappingPrice = new MappingPriceList();
            mappingPrice = repoMapping.GetMappingByGuidId(id);
            return Json(mappingPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMappingPrice(string id)
        {
            repoMapping.Delete(id);
            MappingPriceList mappingPrice = new MappingPriceList();
            mappingPrice = repoMapping.GetMappingByGuidId(id);
            if (mappingPrice != null)
                DeleteMappingPrice(id);
            return Json("Ok", JsonRequestBehavior.AllowGet);
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

        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }

    }
}
