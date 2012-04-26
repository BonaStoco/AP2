using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Web;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.Report.Controllers
{
    [Authorize(Roles = APRoles.AP_ROLES)]
    public class AP2ReportController : Controller
    {
        const string IDR = "IDR";
        const string USD = "USD";

        BonaStoco.AP1.Web.ReportingRepository.IAP2ReportRepository _apRepo = null;
        public AP2ReportController()
        {
            _apRepo = new AP2ReportRepository();
        }

        public ActionResult Index()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanAdvancedSearch> tenans = GetTenanByRole(cp);
            return View();

        }

        private IList<TenanAdvancedSearch> GetTenanByRole(CompanyProfiles cp)
        {
            IList<TenanAdvancedSearch> tenans = new List<TenanAdvancedSearch>();
            if (APRoles.IsRoot(cp.RoleName))
                tenans = AdvancedSearchRepository().GetAllTenan();
            else if (APRoles.IsBandara(cp.RoleName))
                tenans = AdvancedSearchRepository().FindTenantByBandara(cp.Role.Bandara);
            else if (APRoles.IsTerminal(cp.RoleName))
                tenans = AdvancedSearchRepository().FindTenantByBandaraAndTerminal(cp.Role.Bandara, cp.Role.Terminal);
            else if (APRoles.IsSubTerminal(cp.RoleName))
                tenans = AdvancedSearchRepository().FindTenantByBandaraAndTerminalAndSubTerminal(cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);

            ViewBag.Tenants = tenans;
            return tenans;
        }


        public ActionResult FakturAP2(string Tenant, string tahun, string bulan, string noFaktur, string ttd, string nip, string pts)
        {
            _apRepo.UpdateBillingByTenan(Int32.Parse(Tenant), ConvertPeriod(tahun, bulan), ttd, nip, noFaktur);
            FakturAP2 faktur = _apRepo.FindFakturAPByPeriodeandTenanId(ConvertPeriod(tahun, bulan), Tenant);
            if (pts != null && pts.Equals("PTS"))
            {
                ViewBag.Pts = "PTS";
            }
            //IList<DetailFakturAP2> bills = FindBillingDetail(faktur.Period, faktur.TenanId.ToString());
            //if (bills.Count == 0)
            //{
            //    CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            //    IList<TenanAdvancedSearch> tenans = GetTenanByRole(cp);

            //    ViewBag.Message = "Data Tidak Ditemukan !!";
            //    return View("Index");

            //}
            Tenan tenan = getTenan(Int32.Parse(Tenant));
            SetViewHeaderFaktur(bulan, tahun, tenan);
            SetViewBagFooter(faktur);
            return View(faktur);

        }

        private decimal CalculateTotalPenjualan(FakturAP2 fakturAP)
        {
            ExchangeRate _USDRate = _apRepo.FindRateUSD(DateTime.Today.Date);
            if (_USDRate == null)
                throw new ApplicationException("Rate USD tanggal " + DateTime.Today.Date.ToString("dd / MM /yyyy") + " tidak ditemukan.");

            if (fakturAP.CcyCode.ToUpper() == USD)
                return (fakturAP.TotalPenjualan / _USDRate.Rate) + fakturAP.TotalPenjualanInUSD;

            return fakturAP.TotalPenjualan + (fakturAP.TotalPenjualanInUSD * _USDRate.Rate);
        }

        private void SetViewBagFooter(FakturAP2 fakturAP)
        {
            decimal totalpenjualan = CalculateTotalPenjualan(fakturAP);
            decimal totalkonsesi = TotalKonsesi(totalpenjualan, fakturAP);
            decimal totalbagihasil = totalkonsesi * fakturAP.Tarif;
            decimal totalpajakbagihasil = totalbagihasil * fakturAP.Pajak;
            decimal totaltagihan = totalpajakbagihasil + totalbagihasil;

            ViewBag.Target = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + fakturAP.Target.ToString("N2") : fakturAP.Target.ToString("N0"); 
            ViewBag.TotalKonsesi = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + totalkonsesi.ToString("N2") : totalkonsesi.ToString("N0");
            ViewBag.TotalPenjualan = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + totalpenjualan.ToString("N2") : totalpenjualan.ToString("N0");
            ViewBag.TotalBagiHasil = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + totalbagihasil.ToString("N2") : totalbagihasil.ToString("N0");
            ViewBag.TotalPajakBagiHasil = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + totalpajakbagihasil.ToString("N2") : totalpajakbagihasil.ToString("N0");
            ViewBag.TotalTagihan = fakturAP.CcyCode.ToUpper() == USD ? USD + " " + totaltagihan.ToString("N2") : totaltagihan.ToString("N0");
            ViewBag.Say = SetCcyTerbilang(totaltagihan, fakturAP.CcyCode);
            


        }

        public decimal TotalKonsesi(decimal totalPenjualan, FakturAP2 fakturAP)
        {

            if (totalPenjualan < fakturAP.Target)
            {
                return fakturAP.Target;
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


        private void SetViewHeaderFaktur(string bulan, string tahun, Tenan tenan)
        {
            ProductType productType = MasterDataRepository().FindProductTypeById(tenan.ProductTypeId);

            if (productType != null)
                ViewBag.JenisJasa = productType.ProductTypeName;
            else
                ViewBag.JenisJasa = "KONSESI";
            ViewBag.Tahun = tahun;
            ViewBag.Bulan = bulan;
            ViewBag.Tenant = tenan.TenanId;
            ViewBag.TenanName = tenan.TenanName;
            ViewBag.Address = tenan.Alamat;

        }

        private IList<DetailFakturAP2> FindBillingDetail(string period, string tenanId)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<DetailFakturAP2> bill = new List<DetailFakturAP2>();
            if (APRoles.IsRoot(cp.RoleName))
            {
                bill = _apRepo.FindBillingDetailByPeriodeandTenanId(period, Int32.Parse(tenanId));

            }
            else if (APRoles.IsBandara(cp.RoleName))
            {
                bill = _apRepo.FindBillingDetailByPeriodeandBandaraId(period, Int32.Parse(tenanId), cp.Role.Bandara);
            }
            else if (APRoles.IsTerminal(cp.RoleName))
            {
                bill = _apRepo.FindBillingDetailByPeriodeandTerminalId(period, Int32.Parse(tenanId), cp.Role.Bandara, cp.Role.Terminal);
            }
            else if (APRoles.IsSubTerminal(cp.RoleName))
            {
                bill = _apRepo.FindBillingDetailByPeriodeandSubTerminalId(period, Int32.Parse(tenanId), cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);
            }
            return bill;
        }

        public JsonResult FindTenantNameByTenanId(int id)
        {
            Tenan tenan = getTenan(id);
            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
        }

        private Tenan getTenan(int id)
        {
            Tenan tenan = null;
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);

            if (APRoles.IsRoot(cp.RoleName))
                tenan = MasterDataRepository().FindTenanById(id);
            else if (APRoles.IsBandara(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandara(id, cp.Role.Bandara);
            else if (APRoles.IsTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminal(id, cp.Role.Bandara, cp.Role.Terminal);
            else if (APRoles.IsSubTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminalAndSubTerminal(id, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);

            return tenan;
        }

        public JsonResult FakturbyTenanAndPeriod(string Id, string tahun, string bulan)
        {

            FakturAP2 fakturAP2 = _apRepo.FindFakturAPByPeriodeandTenanId(ConvertPeriod(tahun, bulan), Id);
            return Json(fakturAP2, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DetailFakturbyTenanAndPeriod(string Id, string tahun, string bulan)
        {
            IList<DetailFakturAP2> detailFakturAP = _apRepo.FindDetailFakturByPeriodeandTenanId(ConvertPeriod(tahun, bulan), Int32.Parse(Id));


            return Json(detailFakturAP, JsonRequestBehavior.AllowGet);
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


        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

        private ITenanAdvancedSearchRepository AdvancedSearchRepository()
        {
            return (ITenanAdvancedSearchRepository)ContextRegistry.
                GetContext().GetObject("TenanAdvancedSearchRepository");
        }

    }
}
