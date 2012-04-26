using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Models;

namespace BonaStoco.AP1.Web.Controllers.ReportSale
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class PerubahanBarangController : Controller
    {
        //
        // GET: /PerubahanBarang/

        IAPMasterRepository _repo = new APMasterRepository();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FindTenanByDate()
        {
            IList<TenanProduct> tenan = _repo.FindTenanByDate(DateTime.Now);
            return Json(tenan,JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenanByWeek()
        {
            DateTime date = DateTime.Now;
            int endDay = (int)date.DayOfWeek;
            var startdate = date.AddDays(endDay * -1);
            IList<TenanProduct> tenan = _repo.FindAllTenanByWeek(startdate, date);
            return Json(tenan, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenanByMonth()
        {
            IList<TenanProduct> tenan = _repo.FindTenanByMonth();
            return Json(tenan, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindAllTenan()
        {
            IList<TenanProduct> tenan = _repo.FindAllTenan();
            return Json(tenan, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductChangeByTenanAndDate(int tenanid)
        {
            IList<ProductChange> productChange = _repo.FindProductChangeByTenanAndDate(tenanid);
            return Json(productChange, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductChangeByTenanAndWeek(int tenanid)
        {
            DateTime date = DateTime.Now;
            int endDay = (int)date.DayOfWeek;
            var startdate = date.AddDays(endDay * -1);
            IList<ProductChange> productChange = _repo.FindProductChangeByTenanAndWeek(tenanid, startdate, date);
            return Json(productChange, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindProductChangeByTenanAndMonth(int tenanid)
        {
            IList<ProductChange> productChange = _repo.FindProductChangeByTenanAndMonth(tenanid);
            return Json(productChange, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindAllProductChange(int tenanid, int totalRow, int currPage)
        {
            IList<ProductChange> productChange = _repo.FindAllProductChange(tenanid, totalRow, currPage);
            return Json(productChange, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountAllProductChangeByTenan(int tenanid)
        {
            IList<ProductChange> productChange = _repo.CountAllProductChangeByTenan(tenanid);
            return Json(productChange, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindAllProductByCode(string id, int tenanid)
        {
            IList<ProductPrint> productprint = _repo.FindAllProductByCode(id, tenanid);
            return Json(productprint, JsonRequestBehavior.AllowGet);
        }
    }
}
