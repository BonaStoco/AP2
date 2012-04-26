using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery(@"select DISTINCT(b.tenanid)as tenanid, a.tenanname, a.alamat, a.gate
                                        from tenan a inner join
                                             tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                        where b.date = current_date - 1 AND b.categoryid in (3,4)")]
    [NamedSqlQuery("DetailTenanAktifKemarinInBandara", @"select DISTINCT(b.tenanid)as tenanid, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.date = current_date - 1 AND b.companylocationid = @locationid AND b.categoryid in (3,4)")]

    [NamedSqlQuery("DetailTenanAktifKemarinInTerminal", @"select DISTINCT(b.tenanid)as tenanid, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.date = current_date - 1 AND b.companylocationid = @locationid AND b.categoryid in (3,4) AND b.terminalid = @terminalid")]

    [NamedSqlQuery("DetailTenanAktifKemarinInSubTerminal", @"select DISTINCT(b.tenanid)as tenanid, a.tenanname, a.alamat, a.gate
                                                from tenan a inner join
                                                      tenantdailysalesmonitoring b on a.tenanid = b.tenanid
                                                where b.date = current_date - 1 AND b.companylocationid = @locationid AND b.categoryid in (3,4) AND b.terminalid = @terminalid AND b.subterminalid = @subterminalid")]


    public class DetailTenanAktifKemarin : IViewModel
    {
        public int TenanId { get; set; }
        public string Alamat { get; set; }
        public string TenanName { get; set; }
        public string Gate { get; set; }
    }
}
