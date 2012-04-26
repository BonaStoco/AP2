using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.Web.Messages;

namespace BonaStoco.AP1.Web.Controllers
{
    [Authorize(Roles=APRoles.TENANT_ROLES)]
    public class CompanyProfileController : Controller
    {
      
        IMasterDataRepository repo = null;
        private IMasterDataRepository MasterDataRepository
        {
            get
            {
                if (repo == null)
                    repo = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
                return repo;
            }
        }

        public ActionResult Index()
        {           
            Tenan _tenan = MasterDataRepository.FindTenanById(new CompanyProfiles(this.HttpContext).CompanyId);
            return View(_tenan);
        }

        public ViewResult EditProfile(string tenantId)
        {
            Tenan _tenan = MasterDataRepository.FindTenanById(Int32.Parse(tenantId));
            return View(_tenan);
        }

        [HttpPost]
        public ViewResult EditProfile(Tenan tenan)
        {
            Tenan _tenan = MasterDataRepository.FindTenanById(tenan.TenanId);
            if (ModelState.IsValid)
            {
                TenanEditedMessage msg = new TenanEditedMessage()
                {
                    TenanId = tenan.TenanId,
                    TenanName = _tenan.TenanName,
                    Alamat = tenan.Alamat,
                    Nppkp = tenan.Nppkp,
                    Npwp = tenan.Npwp,
                    CategoryId = _tenan.CategoryId,
                    LocationId = _tenan.LocationId,
                    SubTerminalId = _tenan.SubTerminalId,
                    TerminalId = _tenan.TerminalId,
                    Tarif = _tenan.Tarif,
                    TanggalBergabung = _tenan.TanggalBergabung,
                    TenanTypeId = _tenan.TenanTypeId,
                    ProductTypeId = _tenan.ProductTypeId,
                    Gate = _tenan.Gate,
                     CcyCode= _tenan.CcyCode,
                     FormulaKonsesi= _tenan.FormulaKonsesi,
                     Target= _tenan.Target,
                     HeadOffice= _tenan.HeadOffice    
                };

                new RabbitHelper().SendTenanEditedMessage(msg);

                return View("EditTenanSelesai");
            }            

            return View(tenan);
        }      
    }
}
