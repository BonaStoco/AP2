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
    public class APReportController : Controller
    {
        IAPMasterRepository _apRepo = null;
        const string IDR = "IDR";
        const string USD = "USD";
        public APReportController()
        {
            _apRepo = new APMasterRepository();
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

        public ActionResult ReportPreview(string Tenant, string bulan, string tahun, string ttd, string nip, string noFaktur, string pts)
        {

            string button = this.Request.Form["Button"];
            string period = ConvertPeriod(tahun, bulan);
            Tenan tenan = getTenan(Int32.Parse(Tenant));
            SetViewBagHeader(bulan, tahun, tenan);
            ViewPTS(pts); 
            FakturAP fakturAP= null;
            if (button == "1")
            {
                fakturAP = _apRepo.FindFakturAPByPeriodeandTenanId(period, Tenant);
                FormulaField formula = FormulaBagiHasil.ExecuteFormula(fakturAP.FormulaKonsesi, fakturAP.Konsesi, fakturAP.Tarif);
                SetViewBagFormula(formula);
                SetViewBagFooter(fakturAP);
                return View("ReportView", fakturAP);
            }
            else
            {

                fakturAP = UpdateBillingAndPreview(Tenant, period, ttd, nip, noFaktur, pts);
                SetViewBagFooter(fakturAP);
                return View("ReportView", fakturAP);
            }          
           
        }

        private FakturAP UpdateBillingAndPreview(string tenanId, string period, string ttd, string nip, string noFaktur, string pts)
        {
        FakturAP fakturAP = _apRepo.FindFakturAPByPeriodeandTenanId(period,tenanId);
        string formulaKonsesi = fakturAP.FormulaKonsesi;
        Tenan tenan = MasterDataRepository().FindTenanById(Int32.Parse(tenanId));

        decimal target = tenan.Target;
        decimal tarif = fakturAP.Tarif;
        decimal totalpenjualan = CalculateTotalPenjualan(fakturAP);
        decimal totalkonsesi = TotalKonsesi(totalpenjualan, tenan);
        FormulaField formula= FormulaBagiHasil.ExecuteFormula(formulaKonsesi, totalkonsesi, tarif);
        SetViewBagFormula(formula);
        decimal totalbagihasil = formula.BagiHasil;
        decimal totalpajakbagihasil = totalbagihasil * fakturAP.Pajak;
        decimal totaltagihan = totalpajakbagihasil + totalbagihasil;
        UpdateFakturAP updateFakturAP = new UpdateFakturAP()
        {
            TenanId = long.Parse(tenanId),
            Period = period,
            Ttd = ttd,
            Nip = nip,
            NoFaktur = noFaktur,
            BagiHasil = totalbagihasil,
            Konsesi = totalkonsesi,
            PajakBagiHasil = totalpajakbagihasil,
            Penjualan = totalpenjualan,
            Tagihan = totaltagihan,
            Tarif= tarif,
            FormulaKonsesi = formulaKonsesi,
            Target = target
             
        };
         _apRepo.UpdateBilling(updateFakturAP);
         FakturAP _fakturAP = _apRepo.FindFakturAPByPeriodeandTenanId(period, tenanId);
         return _fakturAP;
                
        }               

        private void ViewPTS(string pts)
        {
             if (pts != null && pts.Equals("PTS"))
            {
                ViewBag.Pts = "PTS";
            }

        }
                
        private decimal CalculateTotalPenjualan(FakturAP fakturAP)
        {
            ExchangeRate _USDRate = _apRepo.FindRateUSD(DateTime.Today.Date);
            if(_USDRate == null)
                throw new ApplicationException("Rate USD tanggal "+ DateTime.Today.Date.ToString("dd / MM /yyyy") + " tidak ditemukan.");

            if (fakturAP.CcyCode.ToUpper() == USD)
                return (fakturAP.TotalPenjualan / _USDRate.Rate) + fakturAP.TotalPenjualanInUSD;

            return fakturAP.TotalPenjualan + (fakturAP.TotalPenjualanInUSD * _USDRate.Rate);
        }

        private void SetViewBagFormula(FormulaField formula)
        {
            ViewBag.FormulaKonsesi = formula.FormulaName;
            ViewBag.FormulaProcess = formula.FormulaProcess;
        }
        private void SetViewBagFooter(FakturAP fakturAP)
        {
            //string formula = fakturAP.FormulaKonsesi;
            //decimal tarif = fakturAP.Tarif;
            //decimal totalpenjualan = CalculateTotalPenjualan(fakturAP);
            //decimal totalkonsesi = TotalKonsesi(totalpenjualan, fakturAP);
            //decimal totalbagihasil = FormulaBagiHasil.ExecuteFormula(formula, totalkonsesi, tarif);
            //decimal totalpajakbagihasil = totalbagihasil * fakturAP.Pajak;
            //decimal totaltagihan = totalpajakbagihasil + totalbagihasil;

            decimal displayTotalPenjualan = GetDisplayTotalPenjualan(fakturAP.Penjualan, fakturAP);
            ViewBag.TotalKonsesi = fakturAP.CcyCode.ToUpper() == USD ? "$" + " " + fakturAP.Konsesi.ToString("N2") : "Rp." + fakturAP.Konsesi.ToString("N0");
            ViewBag.TotalPenjualan = fakturAP.CcyCode.ToUpper() == USD ? "$" + " " + displayTotalPenjualan.ToString("N2") : "Rp." + displayTotalPenjualan.ToString("N0");
            ViewBag.TotalBagiHasil = fakturAP.CcyCode.ToUpper() == USD ? "$" + " " + fakturAP.BagiHasil.ToString("N2") : "Rp." + fakturAP.BagiHasil.ToString("N0");
            ViewBag.TotalPajakBagiHasil = fakturAP.CcyCode.ToUpper() == USD ? "$" + " " + fakturAP.PajakBagiHasil.ToString("N2") : "Rp." + fakturAP.PajakBagiHasil.ToString("N0");
            ViewBag.TotalTagihan = fakturAP.CcyCode.ToUpper() == USD ? "$" + " " + fakturAP.Tagihan.ToString("N2") : "Rp." + fakturAP.Tagihan.ToString("N0");
            ViewBag.Say = SetCcyTerbilang(fakturAP.Tagihan, fakturAP.CcyCode);
                    
        }

        private decimal GetDisplayTotalPenjualan(decimal totalPenjualan, FakturAP faktur)
        {
            return totalPenjualan < faktur.Target ? faktur.Target : totalPenjualan;
        }

        public decimal TotalKonsesi(decimal totalPenjualan, Tenan tenan)
        {
            if (totalPenjualan < tenan.Target)
            {
                return tenan.Target;
            }
            return totalPenjualan;
        }


        private string SetCcyTerbilang(decimal totaltagihan, string ccyCode)
        {
            string terbilang = string.Empty;
            if(ccyCode.ToUpper()== USD)
            {
                string totalTagihan = totaltagihan.ToString("N2");
                string[] split = totalTagihan.Split('.');
                int angka1 = Int32.Parse(split[0].ToString().Replace(",","").Trim());
                int angka2 = Int32.Parse(split[1].ToString());
                terbilang = SayNumber.Terbilang(angka1) + " Dollar " + SayNumber.Terbilang(angka2) + " Sen";    
            }
            else
            {
                decimal _totalTagihan = Math.Ceiling(totaltagihan);
                terbilang = SayNumber.Terbilang(Convert.ToInt32(_totalTagihan)) + " rupiah";
                
            }
           return string.Format("{0}", terbilang).ToUpper();
        }

        //private void SetViewBagFooter(FakturAP fakturAP, IList<DetailFakturAP> detailFakturAP, Tenan tenan, string ccyCode)
        //{
        //    ViewBag.Pajak = fakturAP.Pajak;
        //    ViewBag.Ttd = fakturAP.Ttd;
        //    ViewBag.Nip = fakturAP.Nip;
        //    ViewBag.BillingId = fakturAP.NoFaktur;      
        //}

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

        private void SetViewBagHeader(string bulan, string tahun, Tenan tenan)
        {
            //TenanType tenanType = MasterDataRepository().FindTenanTypeById(tenan.TenanTypeId);
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

        public JsonResult FakturbyTenanAndPeriod(string Id, string tahun, string bulan)
        {
            FakturAP fakturAP = _apRepo.FindFakturAPByPeriodeandTenanId(ConvertPeriod(tahun, bulan), Id);
            return Json(fakturAP, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailFakturbyTenanAndPeriod(string Id, string tahun, string bulan)
        {
            IList<DetailFakturAP> detailFakturAP = _apRepo.FindDetailFakturByPeriodeandTenanId(ConvertPeriod(tahun, bulan), Int32.Parse(Id));
           

            return Json(detailFakturAP, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FindTenantNameByTenanId(int id)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            Tenan tenan = null;
            if (APRoles.IsRoot(cp.RoleName))
                tenan = MasterDataRepository().FindTenanById(id);
            else if (APRoles.IsBandara(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandara(id, cp.Role.Bandara);
            else if (APRoles.IsTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminal(id, cp.Role.Bandara, cp.Role.Terminal);
            else if (APRoles.IsSubTerminal(cp.RoleName))
                tenan = MasterDataRepository().FindTenantIdByBandaraAndTerminalAndSubTerminal(id, cp.Role.Bandara, cp.Role.Terminal, cp.Role.SubTerminal);

            if (tenan == null)
            {
                return Json("Tenant Tidak Ditemukan", JsonRequestBehavior.AllowGet);
            }
            return Json(tenan.TenanName, JsonRequestBehavior.AllowGet);
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