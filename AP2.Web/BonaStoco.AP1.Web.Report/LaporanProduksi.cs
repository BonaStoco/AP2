using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindAllByCategory", @"SELECT t.tenanid as MRCHCode, 
                                        tt.tenantypename, 
                                        mc.namecompany as Bandara, 
                                        mt.terminalname as Terminal, 
                                        ms.subterminalname as SubTerminal,
                                        t.gate, 
                                        t.tenanname as NamaPerusahaan, 
                                        b.nofaktur, 
                                        '' as Tanggal,
                                        b.period as Bulan, 
                                        pt.producttypename as JenisProduk, 
                                        b.penjualan as Omset, 
                                        t.tarif as PersenKonsesi, 
                                        b.bagihasil as NominalKonsesi, 
                                        b.nofakturpajak as NoPajak, 
                                        '' as ManagementFee 
                                            FROM tenan as t
                                                inner join tenantype tt on tt.tenantypeid=t.tenantypeid
                                                inner join mappingcompany mc on mc.locationid=t.locationid
                                                inner join mappingterminal mt on mt.terminalid=t.terminalid
                                                left join mappingsubterminal ms on ms.subterminalid=t.subterminalid			
                                                inner join billing b on b.tenanid=t.tenanid
                                                inner join producttype pt on pt.producttypeid=t.producttypeid WHERE b.period=@period and t.categoryid IN (3,4)
                                                ORDER BY t.tenantypeid, t.terminalid, t.gate")]
    [Serializable]
    public class LaporanProduksi:IViewModel
    {
        public int MRCHCode { get; set; }
        public string NamaPerusahaan { get; set; }
        public string Gate { get; set; }
        public string TenanTypeName { get; set; }
        public string NoFaktur { get; set; }
        public string Tanggal { get; set; }
        public string Bulan { get; set; }
        public string JenisProduk { get; set; }
        public decimal OMSET { get; set; }
        public decimal PersenKonsesi { get; set; }
        public decimal NominalKonsesi { get; set; }
        public string NoPajak{ get; set; }
        public string ManagementFee { get; set; }
        public string Terminal { get; set; }
        public string Bandara { get; set; }
        public string SubTerminal { get; set; }
    }
}
