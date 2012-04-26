using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Report;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class LaporanProduksiController : Controller
    {
        //
        // GET: /LaporanProduksi/
        IAPMasterRepository _apRepo = new APMasterRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LaporanProduksi(string tahun, string bulan, string tglCetak)
        {
            ViewBag.Tanggal = tglCetak;
            IList<LaporanProduksi> lpProduksi = _apRepo.FindAllByCategory(ConvertPeriod(tahun, bulan));
            //var omsetGate = lpProduksi.GroupBy(i => i.Gate).Sum(gl => gl.Sum(l => l.OMSET));            
            return View(lpProduksi);
        }

        private string ConvertPeriod(string tahun, string bulan)
        {
            string bln;
            if (int.Parse(bulan) < 10)
            {
                bln = string.Format("{0}{1}", 0, bulan);
            }
            else
            {
                bln = bulan;
            }

            string period = string.Format("{0}{1}", tahun, bln);
            return period;
        }

    }
}
