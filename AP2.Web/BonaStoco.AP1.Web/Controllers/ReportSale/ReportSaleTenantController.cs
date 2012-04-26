using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers.ReportSale
{
    public class ReportSaleTenantController : Controller
    {
        IAPMasterRepository _repo = new APMasterRepository();
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public ViewResult DetailPenjualanTenant()
        {
            return View("../ReportSale/DetailPenjualanTenant");
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult ListDetailPenjualanTenan(string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanDetailPenjualanPerTenanView> detail = new List<LaporanDetailPenjualanPerTenanView>();
            ILaporanDetailPenjualanPerTenanRepository repo = new LaporanDetailPenjualanPerTenanRepository();
            detail = repo.FindPenjualanByTenanIdAndDate(dari, sampai, cp.CompanyId);
            return Json(detail.OrderBy(no => no.TransactionNo), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult ListDetailPenjualanPerHariTenan(string dari, string sampai)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<LaporanRingkasanPenjualanPerHariTenan> detail = new List<LaporanRingkasanPenjualanPerHariTenan>();
            ILaporanRingkasanPenjualanPerHariTenanRepository repo = new LaporanRingkasanPenjualanPerHariTenanRepository();
            detail = repo.FindPenjualanByTenanIdAndDateDays(dari, sampai, cp.CompanyId);
            return Json(detail, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult FindProductDetailByTransactionNumberTenan(string transactionNo)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<SalesProductDetail> ProductDetails = _repo.FindSalesDetailProductByTransactionNo(transactionNo, cp.CompanyId);
            return Json(ProductDetails.OrderBy(kode => kode.KodeProduk), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = APRoles.TENANT_ROLES)]
        public JsonResult FindSummaryPenjualanTenan(string tanggal)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<SalesSummaryProduct> summary = _repo.FindSalesSummaryByTenantAndDate(cp.CompanyId.ToString(), DateTime.Parse(tanggal));
            return Json(summary.OrderBy(kode => kode.KodeProduk), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSessionIdByDate(string tanggal)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<SessionSummaryPerKasir> session = _repo.FindSessionPerkasirByTenantAndDate(cp.CompanyId, DateTime.Parse(tanggal));
            return Json(session, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FindSummaryPerkasirByDate(string tanggal, int sessionId)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<SummaryPerKasir> summary = _repo.FindSummaryPerkasirByDateAndTenan(cp.CompanyId, DateTime.Parse(tanggal), sessionId);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }
    }
}
