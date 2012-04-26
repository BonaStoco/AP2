using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [SqlQuery(
      @"SELECT 
        (SELECT count(*) FROM tenan WHERE categoryid in (3,4) AND locationid = 2 AND tenantstatus = 1) as TotalTenan,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE categoryid in (3,4) AND tenanid in (select tenanid from tenan where tenantstatus=1)) as TotalTenanAktif,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date AND categoryid in (3,4)) as TotalTenanAktifHariIni,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date - 1 AND categoryid in (3,4)) as TotalTenanAktifKemarin,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInUSD,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInUSD,
        (SELECT sum(yearlytotalsalepercompany) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInIDR,
        (SELECT sum(totalsalepercompanyinusd) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInUSD")]
    [NamedSqlQuery("SummaryAP2",
      @"SELECT 
        (SELECT count(*) FROM tenan WHERE categoryid in (3,4) AND locationid = 1 AND tenantstatus = 1) as TotalTenan,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE categoryid in (3,4) AND tenanid in (select tenanid from tenan where tenantstatus=1)) as TotalTenanAktif,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date AND categoryid in (3,4)) as TotalTenanAktifHariIni,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date - 1 AND categoryid in (3,4)) as TotalTenanAktifKemarin,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInUSD,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInUSD,
        (SELECT sum(yearlytotalsalepercompany) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInIDR,
        (SELECT sum(totalsalepercompanyinusd) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInUSD")]
    
    [NamedSqlQuery("SummaryUmum",
      @"SELECT 
        (SELECT count(*) FROM tenan WHERE categoryid in (3,4) AND locationid = 1 AND tenantstatus = 1) as TotalTenan,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE categoryid in (3,4) AND tenanid in (select tenanid from tenan where tenantstatus=1)) as TotalTenanAktif,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date AND categoryid in (3,4)) as TotalTenanAktifHariIni,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date - 1 AND categoryid in (3,4)) as TotalTenanAktifKemarin,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date AND categoryid in (3,4) ) as TotalTransaksiHariInUSD,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInIDR,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) ) as TotalTransaksiKemarinInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanInUSD,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInIDR,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) ) as TotalTransaksiBulanKemarinInUSD,
        (SELECT sum(yearlytotalsalepercompany) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInIDR,
        (SELECT sum(totalsalepercompanyinusd) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) ) as TotalTransaksiTahunInUSD")]
    public class SummaryHome : IViewModel
    {
        public Int64 TotalTenan { get; set; }
        public Int64 TotalTenanAktif { get; set; }
        public Int64 TotalTenanAktifHariIni { get; set; }
        public Int64 TotalTenanAktifKemarin { get; set; }
        public decimal TotalTransaksiHariInIDR { get; set; }
        public decimal TotalTransaksiHariInUSD { get; set; }
        public decimal TotalTransaksiKemarinInIDR { get; set; }
        public decimal TotalTransaksiKemarinInUSD { get; set; }
        public decimal TotalTransaksiBulanInIDR { get; set; }
        public decimal TotalTransaksiBulanInUSD { get; set; }
        public decimal TotalTransaksiBulanKemarinInIDR { get; set; }
        public decimal TotalTransaksiBulanKemarinInUSD { get; set; }
        public decimal TotalTransaksiTahunInIDR { get; set; }
        public decimal TotalTransaksiTahunInUSD { get; set; }
    }
}