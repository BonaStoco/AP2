using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Inventory.Models;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.Controllers
{
    public class ReportInventoryController : Controller
    {
        //
        // GET: /ReportInventory/
        [Authorize(Roles = APRoles.AP_ROLES)]
        public ViewResult ReportInventoryAP()
        {
            IList<TenanAdvancedSearch> tenans = TenanAdvSearchRepository().GetAllTenan();
            return View(tenans);
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ViewResult ReportInventoryTenan()
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
        public JsonResult FindPartNameByGroupId(dynamic tenantid, dynamic groupid, dynamic starts, dynamic limits)
        {
            var list = new InventoryRepository().FindProductByGroupAndTenanId(Int32.Parse(tenantid[0]), Int32.Parse(groupid[0]), Int32.Parse(starts[0]), Int32.Parse(limits[0]));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountPagination(dynamic tenantid, dynamic groupid)
        {
            var list = new InventoryRepository().FindPageNumber(Int32.Parse(tenantid[0]), Int32.Parse(groupid[0]));
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }

        [Authorize]
        public JsonResult FindCompanyByTenanId(int id)
        {
            string[] tenanMessage = GetTenanWebService(id);
            return Json(tenanMessage, JsonRequestBehavior.AllowGet);
        }
        private string[] GetTenanWebService(int txtTenanId)
        {
            var tenan = new BonaStoco.AP1.Web.ReportingRepository.tenanws.BonastocoServices().gettenant(
                new BonaStoco.AP1.Web.ReportingRepository.tenanws.askTenant() { tenantid = txtTenanId.ToString(), token = "" });
            string[] tenanMessage = tenan.message.Split(new char[] { ';', '=' });
            return new string[2] { txtTenanId.ToString(), tenanMessage[1] };
        }
    }
}
