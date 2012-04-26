using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery(@"select DISTINCT(b.tenanid)as tenanid, tenanidlama, a.tenanname, a.alamat, a.gate
                                        from tenan a inner join
                                             tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                        where b.categoryid in (3,4)")]
    [NamedSqlQuery("FindTenanAktifByName", @"select DISTINCT(b.tenanid)as tenanid, tenanidlama, a.tenanname, a.alamat, a.gate
                                        from tenan a inner join
                                             tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                        where b.categoryid in (3,4) 
                                        AND LOWER(a.tenanname) like @key")]
    [NamedSqlQuery("DetailTenanAktifInBandara", @"select DISTINCT(b.tenanid)as tenanid, tenanidlama, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.companylocationid = @locationid AND b.categoryid in (3,4)")]

    [NamedSqlQuery("DetailTenanAktifInTerminal", @"select DISTINCT(b.tenanid)as tenanid, tenanidlama, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.companylocationid = @locationid AND b.categoryid in (3,4) AND b.terminalid = @terminalid")]

    [NamedSqlQuery("DetailTenanAktifInSubTerminal", @"select DISTINCT(b.tenanid)as tenanid, tenanidlama, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.companylocationid = @locationid AND b.categoryid in (3,4) AND b.terminalid = @terminalid AND b.subterminalid = @subterminalid")]

    public class DetailTenanAktif : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanIdLama { get; set; }
        public string Alamat { get; set; }
        public string TenanName { get; set; }
        public string Gate { get; set; }
    }
}
