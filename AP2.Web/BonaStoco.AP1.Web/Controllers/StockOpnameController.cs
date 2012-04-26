using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.Models;
using Msg = BonaStoco.AP1.Web.Messages;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Messages;
using BonaStoco.AP.StockOpname.Models;
using BonaStoco.AP.StockOpname.Repository;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Report;


namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class StockOpnameController : Controller
    {
        //
        // GET: /StockOpname/
        IMasterDataRepository repo = null;
        string tenanId;
        BonaStoco.AP.StockOpname.Repository.IStockOpnameRepository stockOpnameRepo = null;
        //IStockOpnameReportRepository opnameRepo = null;
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult OpnamePart(string tenanId)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(Int32.Parse(tenanId));
            ViewBag.tenanId = tenanId;
            ViewBag.tenanName = tenan.TenanName;
            this.tenanId = tenanId;
            return View("TenanOpname");
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
        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = MasterDataRepository.FindTenanById(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
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
        public JsonResult LoadDataStockOpnameByCurrenDate(string tenanId)
        {

            string username = this.User.Identity.Name;
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(-6);
            IList<OpnameModel> opmModel = StockOpnameRepository.FindAllStockOpnameByTenanId(Int32.Parse(tenanId));

            return Json(opmModel, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult FindStockOpnameByTenanIdAndStatus(string tenanId)
        //{
        //  //  OpnameModel opm = StockOpnameRepository.FindStockOpnameByTenanIdAndStatus(tenanId);
        //    return Json(opmHeader, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetOpnameByStatusOpen(string tenanId)
        {
            OpnameModel opmModel = StockOpnameRepository.GetStockOpnameTenanIandStatus(Int32.Parse(tenanId));
            return Json(opmModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OpenStockOpname(string tenanId)
        {
            try
            {


                OpnameModel opm = StockOpnameRepository.GetStockOpnameTenanIandStatus(Int32.Parse(tenanId));
                Guid id;
                if (opm == null)
                {
                    OpenOpnameCommand cmd = new OpenOpnameCommand()
                    {
                        TenanId = Int32.Parse(tenanId),
                        _id = Guid.NewGuid(),
                        Username = this.User.Identity.Name
                    };
                    id = cmd._id;
                    new RabbitHelper().SendStockOpnameExchange<OpenOpnameCommand>(cmd);
                }
                else
                {
                    id = opm.Id;

                }


                return Json(new { ok = true, opnameid = id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, errormessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockOpenOpnameId(string id)
        {
            OpnameModel opmHeader = StockOpnameRepository.GetStockOpnameByGuid(new Guid(id));

            return Json(opmHeader, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddStockOpnameItem(string tenanId, dynamic qty, dynamic partGroupId, dynamic partId, dynamic partGuid, dynamic opnameId)
        {
            try
            {
                Product prod = MasterDataRepository.FindProductById(Int32.Parse(tenanId), Int32.Parse(partId[0]));

                AddOpnameItemCommand msg = new AddOpnameItemCommand()
                {

                    Id = Guid.NewGuid(),
                    OpnameId = new Guid(opnameId[0]),
                    PartGroupId = Int32.Parse(partGroupId[0]),
                    PartId = Int32.Parse(partId[0]),
                    PartGuid = new Guid(partGuid[0]),
                    Qty = Int32.Parse(qty[0]),
                    Barcode = prod.Barcode,
                    PartCode = prod.Kode,
                    PartGroupName = prod.GroupName,
                    PartName = prod.Nama,
                    TenanId = Int32.Parse(tenanId)
                };

                new RabbitHelper().SendStockOpnameExchange(msg);

                return Json(new { ok = true, itemid = msg.Id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, errormessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStockOpnameItemById(string id)
        {
            OpnameItemModel opnItem = StockOpnameRepository.GetStockOpnameItemGuid(new Guid(id));
            return Json(opnItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateItemQty(dynamic qty, dynamic id, dynamic headerId)
        {
            try
            {
                OpnameModel headerOpname = StockOpnameRepository.GetStockOpnameByGuid(new Guid(headerId[0]));
                EditQtyOpnameItemCommand msg = new EditQtyOpnameItemCommand()
                {
                    Id = new Guid(id[0]),
                    OpnameId = headerOpname.Id,
                    OpnameNumber = headerOpname.OpnameNumber,
                    Qty = Int32.Parse(qty[0]),
                    TenanId = headerOpname.TenanId
                };
                new RabbitHelper().SendStockOpnameExchange(msg);
                return Json(msg.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Delete(dynamic id)
        {
            try
            {
                Guid _id = new Guid(id[0]);
                StockOpnameRepository.Delete(_id);
                return Json(_id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteItem(dynamic id, dynamic opnameId)
        {
            OpnameModel opm = StockOpnameRepository.GetStockOpnameByGuid(new Guid(opnameId[0]));
            OpnameItemModel opmItem = StockOpnameRepository.GetStockOpnameItemGuid(new Guid(id[0]));
            try
            {

                DeleteOpnameItemCommand cmd = new DeleteOpnameItemCommand()
                {
                    Id = opmItem.Id,
                    OpnameId = opmItem.OpnameId,
                    OpnameNumber = opmItem.OpnameNumber,
                    TenanId = opmItem.TenanId
                };
                new RabbitHelper().SendStockOpnameExchange(cmd);
                Guid Id = cmd.Id;
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CreateStockOpnameId()
        {
            try
            {
                var id = Guid.NewGuid();
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetUserName()
        {
            string username = this.User.Identity.Name;
            return Json(username, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAllProductByCodeOrBarcode(string tenanId,string code)
        //{
        //    IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(
        //        Int32.Parse(tenanId), code);
        //    return Json(products, JsonRequestBehavior.AllowGet);
        //}

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
        private BonaStoco.AP.StockOpname.Repository.IStockOpnameRepository StockOpnameRepository
        {
            get
            {
                if (stockOpnameRepo == null)
                    stockOpnameRepo = new StockOpnamePostgreRepository();
                return stockOpnameRepo;
            }

        }
        private IStockOpnameReportRepository OpnameReportRepository()
        {
            return new StockOpnameReportRepository();
        }
        private int GetTenanId()
        {
            return new CompanyProfiles(this.HttpContext).CompanyId;
        }

        public JsonResult InitialSearchProduct(string tenanId)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = MasterDataRepository.FindSearchProductByName(Int32.Parse(tenanId), "a");
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProductByName(string tenanId, string name)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<Product> products = MasterDataRepository.FindSearchProductByName(Int32.Parse(tenanId), name);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductByCodeOrBarcode(string tenanId, string code)
        {
            IList<Product> products = MasterDataRepository.FindProductByBarcodeOrCode(
              Int32.Parse(tenanId), code);
            //   var invProducts = new InventoryRepository().FindProductByGroupAndTenanId(Int32.Parse(tenanId), products[0].GroupId, 0, 20);

            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindStockOpnameById(string id)
        {
            OpnameModel opnameHeader = StockOpnameRepository.GetStockOpnameByGuid(new Guid(id));
            IList<OpnameItemModel> opnameItem = StockOpnameRepository.FindAllStockOpnameItemByOpnameId(new Guid(id));
            var opname = new
            {
                Id = opnameHeader.Id,
                TenanId = opnameHeader.TenanId,
                OpenDate = opnameHeader.OpenDate,
                OpnameNumber = opnameHeader.OpnameNumber,
                CloseDate = opnameHeader.CloseDate,
                Username = opnameHeader.Username,
                Status = opnameHeader.Status,
                Items = opnameItem
            };
            return Json(opname, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CloseStockOpname(dynamic opnameid, dynamic tenanid, dynamic opnamenumber, dynamic approval1, dynamic approval2, dynamic approval3, dynamic opnamenote)
        {
            CloseStockOpnameCommand cmd = new CloseStockOpnameCommand()
            {
                _id = new Guid(opnameid[0]),
                TenanId = Int32.Parse(tenanid[0]),
                Username = this.User.Identity.Name,
                OpnameNumber = opnamenumber[0],
                ApprovalOne = approval1[0],
                ApprovalTwo = approval2[0],
                ApprovalThree = approval3[0],
                OpnameNote = opnamenote[0]
            };

            new RabbitHelper().SendStockOpnameExchange<CloseStockOpnameCommand>(cmd);
            return Json(cmd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockOpnameReport(string _id)
        {
            StockOpnameReport[] opnameReport = OpnameReportRepository().FindAllById(new Guid(_id));
            return View(opnameReport);
        }

        public JsonResult CoutOpnameItemById(string id)
        {
            IList<OpnameItemModel> opnameItem = StockOpnameRepository.FindAllStockOpnameItemByOpnameId(new Guid(id));
            return Json(opnameItem, JsonRequestBehavior.AllowGet);
        }
    }
}
