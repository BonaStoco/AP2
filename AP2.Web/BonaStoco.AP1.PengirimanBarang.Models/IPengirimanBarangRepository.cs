using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
namespace BonaStoco.AP1.PengirimanBarang.Models
{
    public interface IPengirimanBarangRepository
    {
        IList<GRN> FindPendingGRNByTenanId(int tenanId, DiscriminatorPengirimanBarang discriminator);
        IList<GRN> FindByTransaksi(string dari, string sampai, string status, int tenanId, DiscriminatorPengirimanBarang discriminator);
        IList<GRNItem> FindItemsByGRNId(Guid grnId);
        GRNItem FindItemByItemGuid(string guid);
        GRN FindByGuid(Guid guid);
        GRN FindByGuidAllstatus(Guid guid);
        IList<GRN> GetGRNFForDaftarPengiriman(int tenanId, string tanggalAwal, string tanggalAkhir, string status, DiscriminatorPengirimanBarang discriminator);
        IList<GRN> FindAllPendingGRNByDiscriminator(DiscriminatorPengirimanBarang discriminator);
        IList<ProductPrint> FindAllProductByCode(string kode, int tenanId);
        IList<GetToComboBoxPartGroup> FindByPartGroup(int tenanid);
        IList<GetToComboBoxCcy> FindByCcy();
        IList<GetToComboBoxUnit> FindByUnit(int tenanid);
        GRNItem DeleteItem(string id);
        GRNItem DeleteGrnItem(string grnId);
        GRN DeleteGrn(string guid);
    }
}