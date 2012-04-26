using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BonaStoco.AP1.Web.Models;
using System.Security.Cryptography;
using System.Text;
using BonaStoco.Inf.ExceptionUtils;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.MasterData.Repository;
using Spring.Context.Support;
namespace BonaStoco.AP1.Web.Controllers
{
    public class SynchronizationPartController : Controller
    {
        //
       
        [Authorize(Roles = APRoles.AP_ROLES)]
        public ActionResult Index()
        {
            return View("Index");
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
            Tenan tenan = MasterDataRepository().FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SyncronizePart(string tenanid)
        {
            try
            {              
                    SyncronizeProductTenanIdMessage msg = new SyncronizeProductTenanIdMessage()
                    {
                         
                        TenanId = Int32.Parse(tenanid.ToString())
                    };

                    new RabbitHelper().SendSyncronizationPart<SyncronizeProductTenanIdMessage>(msg);

                    // return Json(new {message="Success" }, JsonRequestBehavior.AllowGet);
                    return View("TenanSyncronizeFinished");
                
            }
            catch (Exception ex)
            {
                return View("Index?Message="+ex.Message);
               
            }
              
        }
        public JsonResult FindTenanByName(string key)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenanByName(key);
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndName(key, cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraTerminalAndName(key, cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvSearchRepository().FindTenantByBandaraAndTerminalAndSubTerminalAndName(key, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }

            return Json(tenans, JsonRequestBehavior.AllowGet);
        }
        private ITenanAdvancedSearchRepository TenanAdvSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }
      
    }
}