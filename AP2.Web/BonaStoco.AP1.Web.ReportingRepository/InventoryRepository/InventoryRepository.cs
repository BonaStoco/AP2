using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Inventory.Models;
using BonaStoco.Inf.DataMapper;
using Spring.Context.Support;
using BonaStoco.AP1.CouchDB.Config;
using LoveSeat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class InventoryRepository : IInventoryRepository
    {
        IQueryObjectMapper QueryObjectMapper { get; set; }
        //CouchDBConfig _config;
        //CouchClient _client;
        InventoryMongoRepository mongoRepo;
        public InventoryRepository()
        {
            QueryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as IQueryObjectMapper;
            mongoRepo = new InventoryMongoRepository();
            //_config = new CouchDBConfig();
            //_client = new CouchClient(_config.ServerName, _config.ServerPort, _config.UserName, _config.Password, false, AuthenticationType.Cookie);
        }
        public IList<InventoryPartGroup> FindPartGroupByTenanId(long tenanId)
        {
            Int32 _tenanId = Int32.Parse(tenanId.ToString());
            IList<InventoryPartGroup> PGids = QueryObjectMapper.Map<InventoryPartGroup>("FindPartGroupByTenanId", new string[1] { "tenanid" }, new object[1] { _tenanId });
            return PGids;
        }

        public IList<InventoryProduct> FindProductByGroupAndTenanId(long tenanId, long partgroupId, int start, int limit)
        {
            try
            {
                IList<ProductId> product = QueryObjectMapper.Map<ProductId>("FindProductIdByTenanAndGroup", new string[4] { "tenanid", "groupid", "offset", "limit" },
                                                new object[4] { tenanId, partgroupId, start, limit });

                IList<InventoryProduct> pr = new List<InventoryProduct>();
                if (product.Count == 0)
                    return pr;
                //CouchDatabase db = _client.GetDatabase("inventory");
                foreach (ProductId id in product)
                {
                    object[] keys = new object[2];
                    keys[0] = tenanId;
                    keys[1] = id.ProductGuid.ToString();
                    ViewOptions options = new ViewOptions();
                    options.Group = true;
                    options.Reduce = true;
                    options.Key.Add(keys);
                    StockCardDetail scDetail = mongoRepo.GetCurrentBalance(int.Parse(tenanId.ToString()), id.ProductGuid.ToString());
                    InventoryProduct ip;
                    if (scDetail == null)
                    {
                        ip = new InventoryProduct()
                        {
                            Balance = 0,
                            Barcode = id.Barcode,
                            Kode = id.Kode,
                            Nama = id.Nama
                        };
                    }
                    else
                    {
                        ip = new InventoryProduct()
                        {
                            Balance = scDetail.Balance,
                            Barcode = id.Barcode,
                            Kode = id.Kode,
                            Nama = id.Nama
                        };
                    }
                    pr.Add(ip);
                }

                return pr;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public IList<InventoryProduct> FindProductByGroupAndTenanId(long tenanId, long partgroupId)
        {
            try
            {
                IList<ProductId> product = QueryObjectMapper.Map<ProductId>("FindProductIdStockByTenanAndGroup", new string[2] { "tenanid", "groupid" },
                                                new object[2] { tenanId, partgroupId });

                IList<InventoryProduct> pr = new List<InventoryProduct>();
                if (product.Count == 0)
                    return pr;
                //CouchDatabase db = _client.GetDatabase("inventory");
                foreach (ProductId id in product)
                {
                    object[] keys = new object[2];
                    keys[0] = tenanId;
                    keys[1] = id.ProductGuid.ToString();
                    ViewOptions options = new ViewOptions();
                    options.Group = true;
                    options.Reduce = true;
                    options.Key.Add(keys);
                    StockCardDetail scDetail = mongoRepo.GetCurrentBalance(int.Parse(tenanId.ToString()), id.ProductGuid.ToString());
                    InventoryProduct ip;
                    if (scDetail == null)
                    {
                        ip = new InventoryProduct()
                        {
                            Balance = 0,
                            Barcode = id.Barcode,
                            Kode = id.Kode,
                            Nama = id.Nama
                        };
                    }
                    else
                    {
                        ip = new InventoryProduct()
                        {
                            Balance = scDetail.Balance,
                            Barcode = id.Barcode,
                            Kode = id.Kode,
                            Nama = id.Nama
                        };
                    }
                    pr.Add(ip);
                }

                return pr;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public int FindPageNumber(long tenanId, long pgId)
        {
            PageNumbering pn = QueryObjectMapper.Map<PageNumbering>("FindPageNumberByTenanAndGroup", new string[2] { "tenanid", "groupid" }, new object[2] { tenanId, pgId }).FirstOrDefault();
            if (pn == null)
                return 1;
            double pagenumber = Math.Floor(pn.RecordCount / 20);
            if (pn.RecordCount % 20 == 0)
            {
                if (pn.RecordCount < 20)
                    return 1;
                else
                    return int.Parse(pagenumber.ToString());
            }
            else
                return int.Parse((pagenumber + 1).ToString());
        }
    }
}
