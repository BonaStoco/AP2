using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("GetSummaryTerminal",
      @"SELECT 
        (SELECT count(*) FROM tenan WHERE categoryid in (3,4) AND locationid = @locationid AND terminalid = @terminalid) as TotalTenan,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTenanAktif,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date - 1 AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTenanAktifKemarin,
        (SELECT count(DISTINCT(tenanid)) FROM tenantdailysalesmonitoring WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTenanAktifHariIni,
        (SELECT sum(totalsalepercompany) FROM terminaldailysales WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiHari,
        (SELECT sum(totalsalespercompanyinusd) FROM terminaldailysales WHERE date = current_date AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiHariUsd,
        (SELECT sum(totalsalepercompany) FROM terminaldailysales WHERE date = current_date-1 AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiKemarin,
        (SELECT sum(totalsalespercompanyinusd) FROM terminaldailysales WHERE date = current_date-1 AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiKemarinUsd,
        (SELECT sum(totalsalepercompany) FROM terminalmonthlysales WHERE monthperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiBulan,
        (SELECT sum(totalsalepercompanyinusd) FROM terminalmonthlysales WHERE monthperiode = to_char(now(),'YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiBulanUsd,
        (SELECT sum(totalsalepercompany) FROM subterminalmonthlysales WHERE monthperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiBulanKemarin,
        (SELECT sum(totalsalespercompanyinusd) FROM subterminalmonthlysales WHERE monthperiode = to_char(now()- interval '1 month','YYYYMM') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiBulanKemarinUsd,
        (SELECT sum(totalsalepercompany)FROM terminalyearlysales WHERE yearperiode = to_char(now(),'YYYY') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiTahun,
        (SELECT sum(totalsalepercompanyinusd)FROM terminalyearlysales WHERE yearperiode = to_char(now(),'YYYY') AND categoryid in (3,4) AND companylocationid = @locationid AND terminalid = @terminalid) as TotalTransaksiTahunUsd")]
    public class SummaryHomeTerminal : IViewModel
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
        public decimal TotalTransaksiBulanKemarinUsd { get; set; }
        public decimal TotalTransaksiTahun { get; set; }
        public decimal TotalTransaksiTahunUsd { get; set; }
    }
}
