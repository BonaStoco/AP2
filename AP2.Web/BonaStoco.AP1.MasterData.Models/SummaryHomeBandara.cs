using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("GetSummaryBandara",
      @"SELECT 
        (SELECT count(*) FROM tenan WHERE categoryid in (3,4) AND locationid = @locationid) as TotalTenan,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE categoryid in (3,4) AND companylocationid = @locationid) as TotalTenanAktif,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date - 1 AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTenanAktifKemarin,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTenanAktifHariIni,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiHari,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiHariUsd,
        (SELECT sum(totalsalepercompany) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiKemarin,
        (SELECT sum(totalsalespercompanyinusd) FROM dailysales WHERE date = current_date-1 AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiKemarinUsd,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4)  AND companylocationid = @locationid) as TotalTransaksiBulan,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4)  AND companylocationid = @locationid) as TotalTransaksiBulanUsd,
        (SELECT sum(monthlytotalsalepercompany) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiBulanKemarin,
        (SELECT sum(monthlytotalsalepercompanyinusd) FROM monthlysales WHERE monthlyperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiBulanKemarinUSD,
        (SELECT sum(yearlytotalsalepercompany) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiTahun,
        (SELECT sum(totalsalepercompanyinusd) FROM yearlysales WHERE yearlyperiode = to_char(now(),'YYYY') AND categoryid in (3,4) AND companylocationid = @locationid) as TotalTransaksiTahunUsd")]
    public class SummaryHomeBandara : IViewModel
    {
        public Int64 TotalTenan { get; set; }
        public Int64 TotalTenanAktif { get; set; }
        public Int64 TotalTenanAktifKemarin { get; set; }
        public Int64 TotalTenanAktifHariIni { get; set; }
        public decimal TotalTransaksiHari { get; set; }
        public decimal TotalTransaksiHariUsd { get; set; }
        public decimal TotalTransaksiKemarin { get; set; }
        public decimal TotalTransaksiKemarinUsd { get; set; }
        public decimal TotalTransaksiBulan { get; set; }
        public decimal TotalTransaksiBulanUsd { get; set; }
        public decimal TotalTransaksiBulanKemarin { get; set; }
        public decimal TotalTransaksiBulanKemarinUSD { get; set; }
        public decimal TotalTransaksiTahun { get; set; }
        public decimal TotalTransaksiTahunUsd { get; set; }
    }
}
