using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using System.Globalization;

namespace BonaStoco.AP1.ReportFakturPajak.Controllers
{
    [Authorize]
    public class FakturPajakAP2Controller : Controller
    {
        const string IDR = "IDR";
        const string USD = "USD";
        //
        // GET: /APReport/
        IReportFakturPajakRepository reportFakturPajak = new ReportFakturPajakRepository();
        //IList<Tenan> tenans;
        public JsonResult FindReportPajak(string noTenan, int tahun, int bulan)
        {            
            FakturPajakAP2 fakturPajak = reportFakturPajak.reportFakturPajakAP2FindByPeriodeAndTenan(ConvertPeriod(tahun, bulan), Int32.Parse(noTenan));
            return Json(fakturPajak, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReportFakturPajak(string noTenan, string tahun, string bulan, string noFakturPajak)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            ViewBag.HomePage = new CompanyProfiles(this.HttpContext).HomePage;
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();        
            #region Role
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().GetAllTenan();
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandara(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }
            #endregion
            ViewBag.Tenants = tenans;
            Tenan tenan = getTenan(Int32.Parse(noTenan));
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            ViewBag.Period = string.Format("{0} {1}", dtf.GetMonthName(Int32.Parse(bulan)), tahun);
            reportFakturPajak.UpdateNoFakturPajakAP2ByTenanAndPeriode(ConvertPeriod(Int32.Parse(tahun), Int32.Parse(bulan)), Int32.Parse(noTenan), noFakturPajak);
            FakturPajakAP2 fakturPajak = reportFakturPajak.reportFakturPajakAP2FindByPeriodeAndTenan(ConvertPeriod(Int32.Parse(tahun), Int32.Parse(bulan)), Int32.Parse(noTenan));
            SetViewBagFooter(fakturPajak);
            
            return View("ReportFakturPajakAP2", fakturPajak);
        }


        private decimal CalculateTotalPenjualan(FakturPajakAP2 fakturPajak, ExchangeRate usdRate)
        {

            if (usdRate == null)
                throw new ApplicationException("Rate USD tanggal " + DateTime.Today.Date.ToString("dd / MM /yyyy") + " tidak ditemukan.");

            if (fakturPajak.CcyCode.ToUpper() == USD)
                return (fakturPajak.TotalPenjualan / usdRate.Rate) + fakturPajak.TotalPenjualanInUSD;

            return fakturPajak.TotalPenjualan + (fakturPajak.TotalPenjualanInUSD * usdRate.Rate);
        }

        private void SetViewBagFooter(FakturPajakAP2 fakturPajak)
        {
            ExchangeRate _USDRate = reportFakturPajak.FindRateUSD(DateTime.Today.Date);            
            decimal totalpenjualan = CalculateTotalPenjualan(fakturPajak, _USDRate);
            decimal totalkonsesi = TotalKonsesi(totalpenjualan, fakturPajak);
            decimal totalbagihasil = totalkonsesi * fakturPajak.Tarif;
            decimal totalpajakbagihasil = Math.Round(totalbagihasil * fakturPajak.Pajak, 2);

            decimal totaltagihan = totalpajakbagihasil + totalbagihasil;
            if (fakturPajak.CcyCode.ToUpper() == USD)
            {
                decimal totaltagihanIDR = (totalpajakbagihasil) * _USDRate.Rate;
                ViewBag.TotalTagihanIDR = "Rp " + totaltagihanIDR.ToString("N0");
                ViewBag.Rate = _USDRate.Rate;
            }
           
           
            ViewBag.TotalKonsesi = fakturPajak.CcyCode.ToUpper() == USD ? USD + " " + totalkonsesi.ToString("N2") :  totalkonsesi.ToString("N0");
            ViewBag.TotalPenjualan = fakturPajak.CcyCode.ToUpper() == USD ? USD + " " + totalpenjualan.ToString("N2") : totalpenjualan.ToString("N0");
            ViewBag.TotalBagiHasil = fakturPajak.CcyCode.ToUpper() == USD ? USD + " " + totalbagihasil.ToString("N2") : totalbagihasil.ToString("N0");
            ViewBag.TotalPajakBagiHasil = fakturPajak.CcyCode.ToUpper() == USD ? USD + " " + totalpajakbagihasil.ToString("N2") : totalpajakbagihasil.ToString("N0");           
            ViewBag.TotalTagihan = fakturPajak.CcyCode.ToUpper() == USD ? USD + " " + totaltagihan.ToString("N2") : IDR + " " + totaltagihan.ToString("N0");
            //if(fakturPajak.CcyCode.ToUpper() == USD)
            //{
            //    ViewBag.Rate = _USDRate.Rate;
            //    ViewBag.TotalTagihan = IDR + " " + (totaltagihan * _USDRate.Rate).ToString("N");
            //    ViewBag.Say = SetCcyTerbilang(totaltagihan, fakturPajak.CcyCode);
            //}
            //else
            //{
            //     ViewBag.Rate = 1;
            //     ViewBag.TotalTagihan = IDR + " " + (totalbagihasil).ToString("N");
            //     ViewBag.Say = SetCcyTerbilang(totaltagihan, fakturPajak.CcyCode);
            //}
           


        }

        public decimal TotalKonsesi(decimal totalPenjualan, FakturPajakAP2 fakturPajak)
        {

            if (totalPenjualan < fakturPajak.Target)
            {
                return fakturPajak.Target;
            }
            return totalPenjualan;
        }


        private string SetCcyTerbilang(decimal totaltagihan, string ccyCode)
        {
            string terbilang = string.Empty;
            if (ccyCode.ToUpper() == USD)
            {
                string totalTagihan = totaltagihan.ToString("N2");
                string[] split = totalTagihan.Split('.');
                int angka1 = Int32.Parse(split[0].ToString());
                int angka2 = Int32.Parse(split[1].ToString());
                terbilang = SayNumber.Terbilang(angka1) + " Dollar " + SayNumber.Terbilang(angka2) + " Sen";
            }
            else
            {

                terbilang = SayNumber.Terbilang(Convert.ToInt32(totaltagihan)) + " rupiah";
            }
            return string.Format("{0}", terbilang).ToUpper();
        }

           
     
      
        private Tenan getTenan(int tenantId)
        {
            Tenan tenan = null;
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

            if (APRoles.IsRoot(cp.RoleName))
                tenan = MasterDataRepository().FindTenanById(tenantId);
            else if (APRoles.IsBandara(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandara(tenantId, cp.Role.Bandara);
            else if (APRoles.IsTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminal(tenantId, cp.Role.Bandara, cp.Role.Terminal);
            else if (APRoles.IsSubTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminalAndSubTerminal(tenantId, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            return tenan;
        }
       
        public ActionResult ShowDialogReport()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            ViewBag.HomePage = new CompanyProfiles(this.HttpContext).HomePage;
            if (APRoles.IsRoot(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().GetAllTenan();
            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandara(cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                tenans = TenanAdvanceSearch().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }
            ViewBag.Tenants = tenans;
            return View();
        }
        private string ConvertPeriod(int tahun, int bulan)
        {
            string bln;
            if (bulan < 10)
            {
                bln = string.Format("{0}{1}", 0, bulan);
            }
            else
            {
                bln = bulan.ToString();
            }

            string period = string.Format("{0}{1}", tahun, bln);
            return period;
        }
        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
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
        private ITenanAdvancedSearchRepository TenanAdvanceSearch()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }
    }
}