using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.Inventory.Models;
using BonaStoco.Inf.DataMapper;
using Spring.Context.Support;

namespace BonaStoco.AP1.Web.RepositoryTest
{
    [TestFixture]
    public class When_find_partgroup_by_tenanid
    {
        long tenanId;
        long pgId;
        InventoryRepository invRepo;
        [TestFixtureSetUp]
        public void Setup()
        {
            tenanId = 4178;
            pgId = 697;
            invRepo = new InventoryRepository();
        }
        [Test]
        public void Should_return_partgroup_collection_from_ap1()
        {
            IList<InventoryPartGroup> invPG = invRepo.FindPartGroupByTenanId(tenanId);
            Assert.IsTrue(invPG.Count > 0);
        }
        [Test]
        public void Should_return_part_collection_from_couchdb()
        {
            IList<InventoryProduct> product = invRepo.FindProductByGroupAndTenanId(tenanId,pgId,0,20);
            Assert.IsTrue(product.Count > 0);
        }
        [Test]
        public void Count_page_number()
        {
            int pageNumber = invRepo.FindPageNumber(tenanId, pgId);
            Assert.IsNotNull(pageNumber);
        }
    }
}
