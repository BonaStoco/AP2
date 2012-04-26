using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.PengirimanBarang.Models;
using BonaStoco.Inf.DataMapper.Impl;
namespace BonaStoco.AP1.PengirimanBarang.Repository
{
    public class PengirimanBarangRepository : IPengirimanBarangRepository
    {
        QueryObjectMapper qryObjectMapper;

        public PengirimanBarangRepository(QueryObjectMapper qryObjectMapper)
        {
            this.qryObjectMapper = qryObjectMapper;
        }

        public IList<GRN> FindPendingGRNByTenanId(int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            return qryObjectMapper.Map<GRN>("FindPendingGRNByTenanId",
               new string[2] { "tenanid", "discriminator" },
               new object[2] { tenanId, Enum.GetName(typeof(DiscriminatorPengirimanBarang),discriminator) });
        }

        public IList<GRNItem> FindItemsByGRNId(Guid grnId)
        {
            return qryObjectMapper.Map<GRNItem>("FindByGRNId",
               new string[1] { "grnid" },
               new object[1] {grnId });
        }
        public GRNItem FindItemByItemGuid(string guid)
        {
            return qryObjectMapper.Map<GRNItem>("FindByGuId",
               new string[1] { "guid" },
               new object[1] { guid }).FirstOrDefault();
        }

        public GRN FindByGuid(Guid guid)
        {
            return qryObjectMapper.Map<GRN>("FindByGuid",
               new string[1] { "guid" },
               new object[1] { guid }).FirstOrDefault();
        }
        public GRN FindByGuidAllstatus(Guid guid)
        {
            return qryObjectMapper.Map<GRN>("FindByGuidAllstatus",
               new string[1] { "guid" },
               new object[1] { guid }).FirstOrDefault();
        }
        public IList<GRN> FindByTransaksi(string dari, string sampai, string status, int tenanId, DiscriminatorPengirimanBarang discriminator)
        {
            string _discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator);
            string query = @"select grn.*, 
                 tenan.tenanname,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
                    from
                    grn inner join tenan on grn.tenanid = tenan.tenanid
                    where tanggaltransaksi between ";
            if (status.Equals("all"))
            {
                query += string.Format("\'{0}\' AND \'{1}\' AND grn.tenanid = {2} AND discriminator = \'{3}\' ;", dari, sampai, tenanId,_discriminator);
            }
            else
            {
                query += string.Format("\'{0}\' AND \'{1}\'AND status = {2} AND grn.tenanid = {3}  AND discriminator = \'{4}\';", dari, sampai, status, tenanId, _discriminator);
            }

            return qryObjectMapper.Map<GRN>(query).ToList();
        }
        public IList<GRN> GetGRNFForDaftarPengiriman(int tenanId, string tanggalAwal, string tanggalAkhir, string status, DiscriminatorPengirimanBarang discriminator)
        {
            string _discriminator = Enum.GetName(typeof(DiscriminatorPengirimanBarang), discriminator);
            string query = @"select grn.*, 
                 tenan.tenanname,
                 case when grn.status = 0 then 'Pending' else 'Sudah di verifikasi' end as statusstring
          from
          grn inner join tenan on grn.tenanid = tenan.tenanid
          where tanggaltransaksi between ";
            if (status.Equals("all"))
            {
                query += string.Format("\'{0}\' AND \'{1}\' AND grn.tenanid = {2} AND discriminator = \'{3}\'", tanggalAwal, tanggalAkhir, tenanId, _discriminator);
            }
            else
            {
                query += string.Format("\'{0}\' AND \'{1}\' AND grn.tenanid = {2} AND status = {3} AND discriminator = \'{4}\'", tanggalAwal, tanggalAkhir, tenanId, status, _discriminator);
            }

            return qryObjectMapper.Map<GRN>(query).ToList(); 
        }
    }
}