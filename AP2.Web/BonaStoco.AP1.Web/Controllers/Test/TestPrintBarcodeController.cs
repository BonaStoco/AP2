using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BonaStoco.AP1.Web.Controllers
{
    //[Authorize(Roles="Tester")]
    public class TestPrintBarcodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}