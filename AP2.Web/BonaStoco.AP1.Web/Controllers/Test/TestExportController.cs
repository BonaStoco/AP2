using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.Controllers.Test
{
    public class TestExportController : Controller
    {
        //
        // GET: /TestExport/
        public FileContentResult  ExportFormatNewProduct()
        {
            IList<Product> products = MasterDataRepository.FindAllProduct(4083);
            if (products.Count == 0)
                return null;
            string csv = "Partgroup,Unit,Barcode,Code,Nama,HargaJual,CcyCode,Status Print\n";
            foreach (Product prod in products)
            {
                BonaStoco.AP1.MasterData.Models.PartGroup group = MasterDataRepository.FindAllGroups(4083)
                        .Where(g => g.GroupId == prod.GroupId).FirstOrDefault();
                Unit unit = MasterDataRepository.FindAllUnits(4083)
                    .Where(u => u.UnitId == prod.UnitId).FirstOrDefault();
                Ccy ccy = MasterDataRepository.FindAllCurrencies(4083)
                    .Where(c => c.CcyId == prod.CcyId).FirstOrDefault();
                csv += group.Kode + ',';
                csv += unit.Kode + ',';
                csv += prod.Barcode + ',';
                csv += prod.Kode + ',';
                csv += prod.Nama + ',';
                csv += prod.HargaJual.ToString() + ',';
                csv += ccy.Kode + ',';
                csv += prod.StatusPrint + "\n";
            }
            
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Tenant-.csv");
        }
        public FileContentResult ExportFormatEditProduct()
        {
            IList<Product> products = MasterDataRepository.FindAllProduct(4083);
            if (products.Count == 0)
                return null;
            string csv = "Code,Nama,HargaJual,Barcode\n";
            foreach (Product prod in products)
            {
                csv += prod.Kode + ',';
                csv += prod.Nama + ',';
                csv += prod.HargaJual.ToString() + ',';
                csv += prod.Barcode + '\n';
            }

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Tenant-ByGroup().csv");
        }

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
    }
}
