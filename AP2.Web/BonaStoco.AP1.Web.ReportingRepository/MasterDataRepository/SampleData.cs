using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.Inf.Reporting;
namespace BonaStoco.AP1.MasterData.Repository
{
    public class SampleData
    {
        IReportingRepository reportingRepository;
        public SampleData(IReportingRepository reportingRepository)
        {
            this.reportingRepository = reportingRepository;
        }
        
        public void CreateSampleDate()
        {
            var groups = new List<PartGroup>
            {
                new PartGroup() { GroupId = 1, Kode = "001", Nama = "Group 001", TenanId = 2240 },
                new PartGroup() { GroupId = 2, Kode = "002", Nama = "Group 002", TenanId = 2240 },
                new PartGroup() { GroupId = 3, Kode = "003", Nama = "Group 003", TenanId = 2240 },
                new PartGroup() { GroupId = 4, Kode = "004", Nama = "Group 004", TenanId = 2240 },
                new PartGroup() { GroupId = 5, Kode = "005", Nama = "Group 005", TenanId = 2240 }
            };

            reportingRepository.CreateTable<PartGroup>();
            groups.ForEach(g => this.reportingRepository.Save<PartGroup>(g));

            var currencies = new List<Ccy>
            {
                new Ccy() { CcyId = 1, Kode = "IDR", Nama = "Rupiah", Rounding = 0, TenanId = 2240 },
                new Ccy() { CcyId = 2, Kode = "USD", Nama = "US Dollar", Rounding = 2, TenanId = 2240 }
            };

            reportingRepository.CreateTable<Ccy>();
            currencies.ForEach(c => this.reportingRepository.Save<Ccy>(c));

            var units = new List<Unit>
            {
                new Unit() { UnitId = 1, Kode = "PCS", Nama = "Pices", TenanId = 2240 },
                new Unit() { UnitId = 2, Kode = "BOX", Nama = "Box", TenanId = 2240 },
                new Unit() { UnitId = 3, Kode = "DUS", Nama = "Dus", TenanId = 2240 }
            };

            reportingRepository.CreateTable<Unit>();
            units.ForEach(u => this.reportingRepository.Save<Unit>(u));

            var products = new List<Product>
            {
                new Product() { ProductId = 1, Kode = "001", Barcode = "001", Nama = "Kusuka", HargaBeli = 5000, HargaJual = 6000, TenanId = 2240, CcyId = 1, GroupId = 1, UnitId = 1 }
            };

            reportingRepository.CreateTable<Product>();
            products.ForEach(p => this.reportingRepository.Save<Product>(p) );
        }
    }
}