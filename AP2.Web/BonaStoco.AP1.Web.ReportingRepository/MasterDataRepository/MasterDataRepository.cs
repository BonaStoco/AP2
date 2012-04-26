using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.Inf.DataMapper.Impl;
using BonaStoco.Inf.Reporting;
using Spring.Data.Generic;
using Spring.Context.Support;
namespace BonaStoco.AP1.MasterData.Repository
{
    public class MasterDataRepository : IMasterDataRepository
    {
        QueryObjectMapper qryObjectMapper;
        IReportingRepository reportingRepository;
        AdoTemplate _AdoTemplate;
        public MasterDataRepository(QueryObjectMapper qryObjectMapper, IReportingRepository reportingRepository)
        {
            this.qryObjectMapper = qryObjectMapper;
            this.reportingRepository = reportingRepository;
            _AdoTemplate = ContextRegistry.GetContext().GetObject("AdoTemplate") as AdoTemplate;
        }

        #region  Find Master Data
        public Product FindProductByCode(int tenantId, string code)
        {
            return qryObjectMapper.Map<Product>("FindByCode",
                new string[2] { "tenanid", "code" },
                new object[2] { tenantId, code }).FirstOrDefault();
        }
        public PartGroup GetPartGroupById(int groupId, int tenanId)
        {
            return qryObjectMapper.Map<PartGroup>("FindByTenanIdAndId", new string[2] { "groupid", "tenanid" }, new object[2] { groupId, tenanId }).FirstOrDefault();
        }

        public PartGroup GetPartGroupByModelGuid(Guid modelGuid)
        {
            return qryObjectMapper.Map<PartGroup>("FindByTenanIdAndGuiId", new string[1] { "modelguid" }, new object[1] { modelGuid }).FirstOrDefault();
        }


        public Unit GetUnitById(int unitId, int tenanId)
        {
            return qryObjectMapper.Map<Unit>("FindByTenanIdAndId", new string[2] { "unitId", "tenanid" }, new object[2] { unitId, tenanId }).FirstOrDefault();
        }

        public IList<RequestProduct> FindByStatusAndTenanId(int tenanId, DateTime dari, DateTime sampai)
        {
            return qryObjectMapper.Map<RequestProduct>("FindByStatusAndTenanId",
               new string[] { "tenanid", "dari", "sampai" },
               new object[] { tenanId, dari, sampai }).ToList();
        }
        public IList<PartGroup> FindAllGroups(int tenantId)
        {
            return qryObjectMapper.Map<PartGroup>("FindByTenanId",
                new string[1] {"tenanid"},
                new object[1] {tenantId});
        }

        public IList<Ccy> FindAllCurrencies(int tenantId)
        {
            return qryObjectMapper.Map<Ccy>("FindByTenanId",
                new string[1] { "tenanid" },
                new object[1] { tenantId });
        }
        public IList<Unit> FindAllUnits(int tenanId)
        {
            return qryObjectMapper.Map<Unit>("FindByTenanId",
                new string[1] { "tenanid" },
                new object[1] { tenanId });
        }

        public Unit FindAllUnitsByModelGuid(Guid id)
        {
            return qryObjectMapper.Map<Unit>("FindByModelGiud",
                new string[1] { "id" },
                new object[1] { id }).FirstOrDefault();
        }

        public IList<Product> FindAllProduct(int tenanId)
        {
            return qryObjectMapper.Map<Product>("FindByTenanId",
                new string[1] { "tenanid" },
                new object[1] { tenanId });
        }

        public IList<Product> FindProductByBarcodeOrCode(int tenanId, string code)
        {
            return qryObjectMapper.Map<Product>("FindByBarcodeOrCode",
                new string[2] { "tenanid", "code" },
                new object[2] { tenanId, code });
        }

        public IList<Product> FindProductByname(int tenanId, string nama)
        {
            var sql = string.Format(
            @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
            FROM 
            product p inner join 
            partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
            unit u on p.unitid = u.unitid 
            where p.tenanid={0} AND lower(p.nama) like '%{1}%", tenanId.ToString(), nama.ToLower());
            return qryObjectMapper.Map<Product>(sql);
        }

        public IList<Product> FindProductByGroupId(int tenanId, int groupId)
        {
            return qryObjectMapper.Map<Product>("FindByGroup",
                new string[2] { "tenanid", "groupid" },
                new object[2] { tenanId, groupId }).ToList();
        }


        public Product FindProductById(int tenanId, int productId)
        {
            List<Product> result = qryObjectMapper.Map<Product>("FindById",
                new string[2] { "tenanid", "productid" },
                new object[2] { tenanId, productId }).ToList();
            return result.Count() > 0 ? result[0] : null;
        }

        public IList<RequestProduct> FindAllProductPending()
        {
            return qryObjectMapper.Map<RequestProduct>().ToList();
        }

        public PendingProductRequestCount CountPendingRequestedProduct()
        {
            return qryObjectMapper.Map<PendingProductRequestCount>().FirstOrDefault();
        }

        public IList<RequestProduct> FindProductApprovedAndPendingByTenanId(int tenanId)
        {
            return qryObjectMapper.Map<RequestProduct>("FindProductApprovedAndPendingByTenanId", new string[] { "tenanid" }, new object[] { tenanId }).ToList();
        }

        public IList<RequestProduct> FindAllProductAproveByProductId(int productId)
        {
            return qryObjectMapper.Map<RequestProduct>("FindAllProductAproveByProductId", new string[] { "productid" }, new object[] { productId }).ToList();
        }


        public RequestProduct FindProductAproveByGuidId(string guidId)
        {
            return qryObjectMapper.Map<RequestProduct>("FindAllProductAproveByGuidId", new string[] { "guidid" }, new object[] { guidId }).FirstOrDefault();
        }
        #endregion

        #region Find Tenan
        public IList<Tenan> FindAllTenanByCategoryId(int categoryId)
        {
            return qryObjectMapper.Map<Tenan>("FindAllTenanByCategoryId",
                new string[1] { "categoryid" },
                new object[1] { categoryId }).ToList();
        }
        public Tenan FindTenanById(int tenanId)
        {
            return qryObjectMapper.Map<Tenan>("FindByTenanId",
                new string[1] { "tenanid" },
                new object[1] { tenanId }).ToList().FirstOrDefault();
        }

        public IList<Tenan> GetAllTenan()
        {
            string sql = "select * from tenan t left join mappingcompany c on t.locationid=c.locationid	left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid";
            return qryObjectMapper.Map<Tenan>(sql).ToList();
        }
        
        public Tenan FindAllTenan()
        {
            string sql = "SELECT * FROM tenan";
            return qryObjectMapper.Map<Tenan>(sql).FirstOrDefault();
        }

        public IList<Tenan> FindTenantByBandara(int bandaraId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantByBandara", new string[] { "bandaraid" }, new object[] { bandaraid }).ToList();
        }

        public IList<Tenan> FindTenantByBandaraAndTerminal(int bandaraId, int terminalId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantByBandaraAndTerminal", new string[] { "bandaraid", "terminalid" }, new object[] { bandaraid, terminalId }).ToList();
        }

        public IList<Tenan> FindTenantByBandaraAndTerminalAndSubTerminal(int bandaraId, int terminalId, int subTerminalId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantByBandaraAndTerminalAndSubTerminal", new string[] { "bandaraid", "terminalid", "subterminal" }, new object[] { bandaraid, terminalId, subTerminalId }).ToList();
        }

        public Tenan FindTenantIdByBandara(int tenanId, int bandaraId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantIdByBandara", new string[] { "tenanid", "bandaraid" }, new object[] { tenanId, bandaraid }).SingleOrDefault();
        }

        public Tenan FindTenantIdByBandaraAndTerminal(int tenanId, int bandaraId, int terminalId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantIdByBandaraAndTerminal", new string[] { "tenanid", "bandaraid", "terminalid" }, new object[] { tenanId, bandaraid, terminalId }).SingleOrDefault();
        }

        public Tenan FindTenantIdByBandaraAndTerminalAndSubTerminal(int tenanId, int bandaraId, int terminalId, int subTerminalId)
        {
            string bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<Tenan>("FindTenantIdByBandaraAndTerminalAndSubTerminal", new string[] { "tenanid", "bandaraid", "terminalid", "subterminal" }, new object[] { tenanId, bandaraid, terminalId, subTerminalId }).SingleOrDefault();
        }
        #endregion

        #region find bandara terminal
        public IList<Company> GetAllCompany()
        {
            string sql = "SELECT * FROM mappingcompany";
            return qryObjectMapper.Map<Company>(sql).ToList();
        }
        public MappingCompany FindCompanyName(string locationid)
        {
            string sql = "SELECT * FROM mappingcompany";
            return qryObjectMapper.Map<MappingCompany>(sql).FirstOrDefault();
        }
        public MappingTerminal FindTerminalName(string terminalid)
        {
            string sql = "SELECT * FROM mappingterminal";
            return qryObjectMapper.Map<MappingTerminal>(sql).FirstOrDefault();
        }
        public IList<MappingCompany> FindBandaraByCategoryId(int categoryId)
        {
            return qryObjectMapper.Map<MappingCompany>("FindBandaraByCategoryId", new string[] { "categoryid" }, new object[] { categoryId }).ToList();
        }
        public IList<MappingTerminal> FindTerminalByCategoryId(int categoryId)
        {
            return qryObjectMapper.Map<MappingTerminal>("FindTerminalByCategoryId", new string[] { "categoryid"}, new object[] { categoryId}).ToList();
        }
        public IList<MappingSubTerminal> FindSubTerminalByCategoryId(int categoryId)
        {
            return qryObjectMapper.Map<MappingSubTerminal>("FindSubTerminalByCategoryId", new string[] { "categoryid" }, new object[] { categoryId }).ToList();
        }
        public IList<MappingTerminal> FindTerminalByCategoryIdAndLocationId(int categoryId, int locationId)
        {
            return qryObjectMapper.Map<MappingTerminal>("FindTerminalByCategoryIdAndLocationId", new string[] { "categoryid", "locationId" }, new object[] { categoryId, locationId }).ToList();
        }
        public IList<MappingSubTerminal> FindSubTerminalByCategoryIdAndTerminalId(int categoryId, int terminalId)
        {
            return qryObjectMapper.Map<MappingSubTerminal>("FindSubTerminalByCategoryIdAndTerminalId", new string[] { "categoryid", "terminalid" }, new object[] { categoryId, terminalId }).ToList();
        }
        public IList<MappingSubTerminal> FindSubTerminalByCategoryIdAndLocationId(int categoryId, int locationId)
        {
            return qryObjectMapper.Map<MappingSubTerminal>("FindSubTerminalByCategoryIdAndLocationId", new string[] { "categoryid", "locationid" }, new object[] { categoryId, locationId }).ToList();
        }
        public MappingSubTerminal FindSubTerminalById(int subTerminalId)
        {
            return qryObjectMapper.Map<MappingSubTerminal>("FindSubTerminalById", new string[] { "subterminalid" }, new object[] { subTerminalId }).FirstOrDefault();
        }
        public IList<MappingTerminal> FindTerminalById(int terminalId)
        {
            return qryObjectMapper.Map<MappingTerminal>("FindTerminalById", new string[] { "terminalId" }, new object[] { terminalId }).ToList();
        }
        public IList<MappingCompany> FindBandaraById(int locationId)
        {
            return qryObjectMapper.Map<MappingCompany>("FindBandaraById", new string[] { "locationId" }, new object[] { locationId }).ToList();
        }
        #endregion
        public Ccy GetCcyById(int ccyId)
        {
            return qryObjectMapper.Map<Ccy>("FindById", new string[1] { "ccyid" }, new object[1] { ccyId }).FirstOrDefault();
        }
        public IList<AirCraft> FindAirCraft()
        {
            return qryObjectMapper.Map<AirCraft>().ToList();
        }
        public IList<Customer> FindCustomer()
        {
            return qryObjectMapper.Map<Customer>().ToList();
        }
        public IList<Destinetion> FindDestinetion()
        {
            return qryObjectMapper.Map<Destinetion>().ToList();
        }
        public IList<Comodity> FindComodity()
        {
            return qryObjectMapper.Map<Comodity>().ToList();
        }

        public IList<BandaraAdvanceSearch> FindBandara()
        {
            return qryObjectMapper.Map<BandaraAdvanceSearch>().ToList();
        }
        public BandaraAdvanceSearch FindTenanActivityByIdBandara(int id)
        {
            return qryObjectMapper.Map<BandaraAdvanceSearch>("FindBandaraById", new string[] { "id" }, new object[] { id }).FirstOrDefault();
        }

        #region Advanced Search Product
        public IList<Product> FindSearchProductByName(int tenanId, string name)
        {
            string prodName = "%" +name.ToLower() + "%";
            return qryObjectMapper.Map<Product>("FindSearchProductByName", new string[2] { "tenanid", "name" }, new object[2] { tenanId, prodName }).ToList();
        }
        #endregion

        public IList<TenanMonitoring> FindTenanActive(string data)
        {
            return qryObjectMapper.Map<TenanMonitoring>("FindTenanActive", new string[] { "data" }, new object[] { data }).ToList();
        }

        public FindBandaraForMonitoringTenan FindBandaraForMonitoringTenan()
        {
            return qryObjectMapper.Map<FindBandaraForMonitoringTenan>().FirstOrDefault();
        }

        public IList<TenanType> FindTenanType()
        {
            return qryObjectMapper.Map<TenanType>().ToList();
        }

        public IList<ProductType> FindProductType()
        {
            return qryObjectMapper.Map<ProductType>().ToList();
        }

        public TenanType FindTenanTypeById(int tenanTypeId)
        {
            return qryObjectMapper.Map<TenanType>("TenanTypeById", new string[1] { "tenantypeid" }, new object[1] { tenanTypeId }).FirstOrDefault();
        }

        public FindBandaraForMonitoringTenanAP2 FindBandaraForMonitoringAP2Tenan()
        {
            return qryObjectMapper.Map<FindBandaraForMonitoringTenanAP2>().FirstOrDefault();
        }
      

        public IList<FindCcyCode> FindCcyCode()
        {
            return qryObjectMapper.Map<FindCcyCode>().ToList();
        }

        public ProductType FindProductTypeById(int productTypeId)
        {
            return qryObjectMapper.Map<ProductType>("findproducttype", new string[1] { "producttypeid" }, new object[1] { productTypeId }).FirstOrDefault();
        }

        public TenanType FindTenanTypeByName(string name)
        {
            return qryObjectMapper.Map<TenanType>("FindTenanTypeByName", new string[1] { "name" }, new object[1] { name }).FirstOrDefault();
        }
        public ProductType FindProductTypeByName(string name)
        {
            return qryObjectMapper.Map<ProductType>("FindProductTypeByName", new string[1] { "name" }, new object[1] { name }).FirstOrDefault();
        }
        public FindCcyCode FindCcyCodeByName(string ccy)
        {
            return qryObjectMapper.Map<FindCcyCode>("FindCcyCodeByName", new string[1] { "ccyCode" }, new object[1] { ccy }).FirstOrDefault();
        }
        

        public IList<ProductPrint> FindAllProductByCode(string kode, int tenanId)
        {
            return qryObjectMapper.Map<ProductPrint>("FindAllProductByCode",
                new string[2] { "kode", "tenanId" },
                new object[2] { kode, tenanId }).ToList();
        }

        public void RejectAllRequestProduct(RequestProduct requestProduct)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "update requestproduct set status='2' where modelguid='" + requestProduct.ModelGuid+"'");
        }

        public IList<RequestProduct> FindAllProductPendingByTenanId(int tenanId)
        {
            return qryObjectMapper.Map<RequestProduct>("FindAllProductPendingByTenanId", new string[1] { "tenanid" }, new object[1] { tenanId }).ToList();

        }

        public IList<ProductHeader> FindAllProductPendingByGroupTenan()
        {
            return qryObjectMapper.Map<ProductHeader>().ToList();

        }
        public IList<Product> FindProductFilteredByMappedPrice(int tenanId, IList<MappingPriceList> mappingPrice)
        {
            string query = getFilteringQuery(mappingPrice,tenanId);
            return qryObjectMapper.Map<Product>(query);
        }

        private string getFilteringQuery(IList<MappingPriceList> mappingPrice, int tenanId)
        {
            return @"SELECT p.*, g.nama as groupname, c.nama as ccyname, u.nama as unitname
          FROM 
          product p inner join 
           partgroup g on p.groupid = g.groupid inner join 
            ccy c on p.ccyid = c.ccyid inner join 
             unit u on p.unitid = u.unitid 
          where p.tenanid= " + tenanId + " " + getCategory(mappingPrice);
        }

        private string getCategory(IList<MappingPriceList> mappingPrice)
        {
            string category = "and p.productid not in (0,";
            foreach (MappingPriceList map in mappingPrice)
            {
                category += map.ProductId + ",";
            }
            category = category.Substring(0,category.Length - 1) + ")";
            return category;
        }
    }
}