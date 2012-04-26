using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.AP1.Web.Messages;
using Spring.Context.Support;
using BonaStoco.AP1.PengirimanBarang.Models;
using Newtonsoft.Json;
namespace BonaStoco.AP1.Web.Models
{
    public class PengirimanBarang
    {
        AP1Entities ap1Db = new AP1Entities();
        HttpContextBase context;
        Items productGuid;
        public PengirimanBarang(HttpContextBase ctx)
        {
            this.context = ctx;           
        }
        public IList<GRNItemModel> GetItems(int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            string _discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator);
            return ap1Db.GRNItem.Where(item => item.UserId == context.User.Identity.Name &&
                                               item.TenanId == tenanId && item.Discriminator == _discriminator).ToList();
        }
        public void AddItem(GRNItemModel item)
        {
            ap1Db.GRNItem.Add(item);
            ap1Db.SaveChanges();
        }
        public GRNItemModel FindByBarcode(string barcode, int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            string _discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator);
            return ap1Db.GRNItem.Where(item => item.Barcode == barcode &&
                                               item.UserId == context.User.Identity.Name &&
                                               item.TenanId == tenanId && item.Discriminator == _discriminator).FirstOrDefault();
        }
        public GRNItemModel Add(Product product, DiscriminatorPengirimanBarang discriminator, int qty)
        {
            GRNItemModel item = null;
            item = FindByBarcode(product.Barcode, product.TenanId,discriminator);
            if (item != null)
            {
                item.Qty = qty;
                item.CalculateTotal();
            }
            else
            {
                Unit defaultUnit = MasterDataRepository.FindAllUnits(product.TenanId).Where(u => u.UnitId == product.UnitId).FirstOrDefault();
                item = new GRNItemModel()
                {
                    UserId = context.User.Identity.Name,
                    ProductId = product.ProductId,
                    Barcode = product.Barcode,
                    Code = product.Kode,
                    TenanId = product.TenanId,
                    NamaBarang = product.Nama,
                    Qty = qty,
                    Unit = product.UnitName,
                    Harga = product.HargaJual,
                    Jumlah = product.HargaJual * qty,
                    UnitId = defaultUnit == null ? Guid.Empty : defaultUnit.ModelGuid,
                    Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator)
                };
                ap1Db.GRNItem.Add(item);
            }
            ap1Db.SaveChanges();

            return item;
        }

        public GRNItemModel AddNewProduct(NewProductModel product, DiscriminatorPengirimanBarang discriminator, int tenanId, string items)
        {
            GRNItemModel item = null;
            item = FindByBarcode(product.Barcode, tenanId, discriminator);
            if (item != null)
            {
                item.Qty = product.Qty;
                item.CalculateTotal();
            }
            else
            {
                Unit defaultUnit = MasterDataRepository.FindAllUnits(tenanId).Where(u => u.UnitId == product.UnitId).FirstOrDefault();
                item = new GRNItemModel()
                {
                    UserId = context.User.Identity.Name,
                    //ProductId = 1,
                    Barcode = product.Barcode,
                    Code = product.Kode,
                    TenanId = tenanId,
                    NamaBarang = product.NamaBArang,
                    Qty = product.Qty,
                    Unit = product.UnitName,
                    Harga = product.HargaJual,
                    Jumlah = product.HargaJual * product.Qty,
                    UnitId = defaultUnit == null ? Guid.Empty : defaultUnit.ModelGuid,
                    Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator),
                    Items = items
                };
                ap1Db.GRNItem.Add(item);
            }
            ap1Db.SaveChanges();
            return item;
        }

        public GRNItemModel Add(Product product, DiscriminatorPengirimanBarang discriminator)
        { 
            GRNItemModel item = null;
            item = FindByBarcode(product.Barcode,product.TenanId,discriminator);
            if (item != null)
            {
                item.Qty++;
                item.CalculateTotal();
            }
            else
            {
                Unit defaultUnit = MasterDataRepository.FindAllUnits(product.TenanId).Where(u => u.UnitId == product.UnitId).FirstOrDefault();
                item = new GRNItemModel()
                {
                    UserId = context.User.Identity.Name,
                    ProductId = product.ProductId,
                    Barcode = product.Barcode,
                    Code = product.Kode,
                    TenanId = product.TenanId,
                    NamaBarang = product.Nama,
                    Qty = 1,
                    Unit = product.UnitName,
                    Harga = product.HargaBeli,
                    Jumlah = product.HargaBeli,
                    UnitId = defaultUnit == null ? Guid.Empty : defaultUnit.ModelGuid,
                    Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang),discriminator)
                };
                ap1Db.GRNItem.Add(item);
            }
            ap1Db.SaveChanges();

            return item;
        }
        public GRNItemModel FindById(int id, int tenanId)
        {
            return ap1Db.GRNItem.Where(item => item.Id == id &&
                                               item.UserId == context.User.Identity.Name &&
                                               item.TenanId == tenanId).FirstOrDefault();
        }
        public GRNItemModel Update(GRNItemModel item)
        {
            GRNItemModel savedItem = FindById(item.Id,item.TenanId);
            if (savedItem == null)
            {
                item.CalculateTotal();
                ap1Db.GRNItem.Add(item);
            }
            else
            {
                savedItem.Qty = item.Qty;
                savedItem.Harga = item.Harga;
                savedItem.CalculateTotal();
            }
            ap1Db.SaveChanges();

            return savedItem;
        }
        public string GetGRNTransactionNumber(DateTime transactionDate,int tenanId)
        {
            GRNAutoNumbering grnAutoNumbering = ap1Db.GRNAutoNumbering.Where(
                g => g.Year == transactionDate.Year && 
                     g.Month == transactionDate.Month &&
                     g.TenantId == tenanId).FirstOrDefault();

            if (grnAutoNumbering == null)
            {
                grnAutoNumbering = new GRNAutoNumbering()
                {
                     Index = 0,
                     Month = transactionDate.Month,
                     Year = transactionDate.Year,
                     TenantId = tenanId
                };
                ap1Db.GRNAutoNumbering.Add(grnAutoNumbering);
            }
            grnAutoNumbering.Next();
            ap1Db.SaveChanges();

            return grnAutoNumbering.GetNumberString();
        }
        public void Kirim(GRNModel grn,int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            Ccy ccy = MasterDataRepository.FindAllCurrencies(tenanId).Where(c => c.CcyId == grn.CcyId).FirstOrDefault();
            PengirimanBarangMessage msg = new PengirimanBarangMessage
            {
                Guid = Guid.NewGuid(),
                TenanId = tenanId,
                CcyCode = ccy == null ? "" : ccy.Kode,
                KodeTransaksi = grn.KodeTransaksi,
                TanggalTransaksi = grn.TanggalTransaksi,
                NamaPengirim = grn.NamaPengirim,
                Referensi = grn.Referensi,
                Keterangan = grn.Keterangan,
                Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator)
            };

            IList<Unit> units = MasterDataRepository.FindAllUnits(tenanId);
            
            IList<GRNItemMessage> itemMessages = new List<GRNItemMessage>();
            List<GRNItemModel> grnItems = (List<GRNItemModel>)GetItems(tenanId,discriminator);
            grnItems.ForEach(item =>
                {
                    Product product = MasterDataRepository.FindProductById(tenanId, item.ProductId);
                    
                    if (item.Items != null)
                    {
                        productGuid = JsonConvert.DeserializeObject<Items>(item.Items);
                    }
                    
                    itemMessages.Add(new GRNItemMessage
                    {
                        Guid = Guid.NewGuid(),
                        UnitGuid = item.UnitId,
                        ProductGuid = item.ProductId == 0 ? productGuid.ProductGuid : product.ModelGuid,
                        Qty = item.Qty,
                        Harga = item.Harga,
                        Jumlah = item.Jumlah,
                        Items = item.Items
                    });
                });
            msg.Items = itemMessages.ToArray();
            new RabbitHelper().SendPengirimanBarangExchange<PengirimanBarangMessage>(msg);

            grnItems.ForEach(item => ap1Db.GRNItem.Remove(item));
            ap1Db.SaveChanges();
        }

        IMasterDataRepository _repo;
        private IMasterDataRepository MasterDataRepository
        {
            get
            {
                if (_repo == null)
                    _repo = (IMasterDataRepository)ContextRegistry.GetContext().GetObject("MasterDataRepository");
                return _repo;
            }
        }
        IPengirimanBarangRepository _pengirimanBarangRepository;
        private IPengirimanBarangRepository PengirimanBarangRepository
        {
            get
            {
                if (_pengirimanBarangRepository == null)
                    _pengirimanBarangRepository = (IPengirimanBarangRepository)ContextRegistry.GetContext().GetObject("PengirimanBarangRepository");
                return _pengirimanBarangRepository;
            }
        }

        internal void Delete(GRNItemModel item)
        {
            GRNItemModel savedItem = ap1Db.GRNItem.Where(i => i.Id == item.Id).FirstOrDefault();
            if (savedItem != null)
            {
                ap1Db.GRNItem.Remove(savedItem);
                ap1Db.SaveChanges();
            }
        }
        public void DeleteAll(int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            string _discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator);
            IList<GRNItemModel> deleteItem = ap1Db.GRNItem.Where(item => item.UserId == context.User.Identity.Name &&
                                               item.TenanId == tenanId && item.Discriminator == _discriminator).ToList();
            if (deleteItem.Count != 0)
            {
                foreach(GRNItemModel item in deleteItem)
                {
                    ap1Db.GRNItem.Remove(item);
                }
                ap1Db.SaveChanges();
            }
        }
        internal GRN ConfirmVerifikasiPengirimanBarang(string grnId)
        {
            GRN grn = PengirimanBarangRepository.FindByGuid(new Guid(grnId));
            IList<GRNItem> items = PengirimanBarangRepository.FindItemsByGRNId(new Guid(grnId));

            VerifiedGRNMessage msg = new VerifiedGRNMessage
            {
                KodeTransaksi = grn.KodeTransaksi,
                TanggalTransaksi = grn.TanggalTransaksi,
                TenanId = grn.TenanId,
                NamaPengirim = grn.NamaPengirim,
                Keterangan = grn.Keterangan,
                Referensi = grn.Referensi,
                CcyCode = grn.CcyCode,
                Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), DiscriminatorPengirimanBarang.GRN)
            };
            msg.Guid = grn.Guid;

            IList<VerifiedGRNItemMessage> msgItems = new List<VerifiedGRNItemMessage>();
            foreach (GRNItem item in items)
            {
                VerifiedGRNItemMessage msgItem=new VerifiedGRNItemMessage
                {
                    ProductGuid = item.ProductGuid,
                    Qty = item.ActualQty,
                    Harga = item.Harga,
                    Jumlah = item.ActualQty*item.Harga,
                    UnitGuid = item.UnitGuid
                };
                msgItem.Guid = item.Guid;
                msgItems.Add(msgItem);
            }

            msg.Items = msgItems.ToArray();

            new RabbitHelper().SendVerifiedGRNMessage(msg);

            return grn;
        }

        internal GRN ConfirmReturnBarang(string grnId)
        {
            GRN grn = PengirimanBarangRepository.FindByGuid(new Guid(grnId));
            IList<GRNItem> items = PengirimanBarangRepository.FindItemsByGRNId(new Guid(grnId));

            VerifiedGRNMessage msg = new VerifiedGRNMessage
            {
                KodeTransaksi = grn.KodeTransaksi,
                TanggalTransaksi = grn.TanggalTransaksi,
                TenanId = grn.TenanId,
                NamaPengirim = grn.NamaPengirim,
                Keterangan = grn.Keterangan,
                Referensi = grn.Referensi,
                CcyCode = grn.CcyCode,
                Discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), DiscriminatorPengirimanBarang.RET)
            };
            msg.Guid = grn.Guid;

            IList<VerifiedGRNItemMessage> msgItems = new List<VerifiedGRNItemMessage>();
            foreach (GRNItem item in items)
            {
                VerifiedGRNItemMessage msgItem = new VerifiedGRNItemMessage
                {
                    ProductGuid = item.ProductGuid,
                    Qty = 0 - item.ActualQty,
                    Harga = item.Harga,
                    Jumlah = item.ActualQty * item.Harga,
                    UnitGuid = item.UnitGuid
                };
                msgItem.Guid = item.Guid;
                msgItems.Add(msgItem);
            }

            msg.Items = msgItems.ToArray();

            new RabbitHelper().SendVerifiedGRNMessage(msg);

            return grn;
        }




    }
}