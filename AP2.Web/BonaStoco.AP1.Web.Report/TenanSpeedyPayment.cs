using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindBillingTenantByYear", @"SELECT bt.*, t.tenanname, to_char(t.tanggalbergabung,'Mon yyyy') as BulanBergabung, to_char(t.tanggalkeluar,'Mon yyyy') as BulanKeluar 
                                                        FROM billingtenant bt 
                                                            inner join tenan t on bt.tenanid=t.tenanid where bt.tahun=@tahun ORDER BY t.tenanname")]
    [NamedSqlQuery("FindBillingTenantByYearAndCategory", @"SELECT bt.*, t.tenanname, to_char(t.tanggalbergabung,'Mon yyyy') as BulanBergabung, to_char(t.tanggalkeluar,'Mon yyyy') as BulanKeluar 
                                                        FROM billingtenant bt 
                                                            inner join tenan t on bt.tenanid=t.tenanid 
                                                            left join mappingcompany c on t.locationid=c.locationid 
                                                            left join mappingterminal ter on ter.terminalid=t.terminalid 
                                                            left join mappingsubterminal sub on sub.subterminalid=t.subterminalid  
                                                        WHERE bt.tahun=@tahun and t.categoryid IN (3,4)  ORDER BY t.tenanname")]
    [NamedSqlQuery("FindBillingTenantByYearAndBandara", @"SELECT bt.*, t.tenanname, to_char(t.tanggalbergabung,'Mon yyyy') as BulanBergabung, to_char(t.tanggalkeluar,'Mon yyyy') as BulanKeluar 
                                                        FROM billingtenant bt 
                                                            inner join tenan t on bt.tenanid=t.tenanid 
                                                            left join mappingcompany c on t.locationid=c.locationid 
                                                            left join mappingterminal ter on ter.terminalid=t.terminalid 
                                                            left join mappingsubterminal sub on sub.subterminalid=t.subterminalid
                                                        WHERE bt.tahun=@tahun and t.locationid=@bandaraid and t.categoryid IN (3,4)  ORDER BY t.tenanname")]
    [NamedSqlQuery("FindBillingTenantByYearAndBandaraAndTerminal", @"SELECT bt.*, t.tenanname, to_char(t.tanggalbergabung,'Mon yyyy') as BulanBergabung, to_char(t.tanggalkeluar,'Mon yyyy') as BulanKeluar 
                                                        FROM billingtenant bt 
                                                            inner join tenan t on bt.tenanid=t.tenanid 
                                                            left join mappingcompany c on t.locationid=c.locationid  
                                                            left join mappingterminal ter on ter.terminalid=t.terminalid 
                                                            left join mappingsubterminal sub on sub.subterminalid=t.subterminalid  
                                                        WHERE bt.tahun=@tahun and t.locationid=@bandaraid and t.terminalid=@terminalid and t.categoryid IN (3,4)  ORDER BY t.tenanname")]
    [NamedSqlQuery("FindBillingTenantByYearAndBandaraAndTerminalAndSubTerminal", @"SELECT bt.*, t.tenanname, to_char(t.tanggalbergabung,'Mon yyyy') as BulanBergabung, to_char(t.tanggalkeluar,'Mon yyyy') as BulanKeluar 
                                                        FROM billingtenant bt 
                                                            inner join tenan t on bt.tenanid=t.tenanid 
                                                            left join mappingcompany c on t.locationid=c.locationid  
                                                            left join mappingterminal ter on ter.terminalid=t.terminalid 
                                                            left join mappingsubterminal sub on sub.subterminalid=t.subterminalid  
                                                        WHERE bt.tahun=@tahun and t.locationid=@bandaraid and t.terminalid=@terminalid and t.subterminalid=@subterminalid and t.categoryid IN (3,4)  ORDER BY t.tenanname")]

    public class TenanSpeedyPayment : IViewModel
    {
        public long Id { get; set; }
        public long TenanId { get; set; }
        public bool Januari { get; set; }
        public bool Februari { get; set; }
        public bool Maret { get; set; }
        public bool April { get; set; }
        public bool Mei { get; set; }
        public bool Juni { get; set; }
        public bool Juli { get; set; }
        public bool Agustus { get; set; }
        public bool September { get; set; }
        public bool Oktober { get; set; }
        public bool November { get; set; }
        public bool Desember { get; set; }
        public string Tahun { get; set; }
        public string TenanName { get; set; }
        public string BulanBergabung { get; set; }
        public string BulanKeluar { get; set; }
    }

}
