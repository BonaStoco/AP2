using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Models;
namespace BonaStoco.AP1.ReportFakturPajak.Controllers
{
    [Authorize(Roles=APRoles.AP_ROLES)]
    public class CalenderViewController : Controller
    {
        //
        // GET: /CalenderView/
        //decimal total;
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSalesByDate(string id)
        {
            var list=new KalenderViewRepository().ReposetoryKalenderView(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}