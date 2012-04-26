using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindDataToDashboardTenan", @"SELECT tenanid , 
                    (SELECT sum(sellingpertransaction) as TotalTransaksiBulan  from tenantdailysalesmonitoring where extract (month from date) = extract (month from current_date) AND extract (year from date) = extract (year from current_date) AND tenanid = @tenanid)as TotalTransaksiBulanIdr,
                    (SELECT sum(sellingpertransactioninusd) as TotalTransaksiBulan  from tenantdailysalesmonitoring where extract (month from date) = extract (month from current_date) AND extract (year from date) = extract (year from current_date) AND tenanid = @tenanid)as TotalTransaksiBulanUsd,
                    (SELECT sum(sellingpertransaction) as TotalTransaksiBulan  from tenantdailysalesmonitoring where extract (month from date) = extract (month from current_date - interval '1 month') AND extract (year from date) = extract (year from current_date) AND tenanid = @tenanid)as TotalTransaksiBulanKemarinIdr,
                    (SELECT sum(sellingpertransactioninusd) as TotalTransaksiBulan  from tenantdailysalesmonitoring where extract (month from date) = extract (month from current_date - interval '1 month') AND extract (year from date) = extract (year from current_date) AND tenanid = @tenanid)as TotalTransaksiBulanKemarinUsd,
                    (SELECT sum(sellingpertransaction) as TotalTransaksiHari from tenantdailysalesmonitoring where date = current_date AND tenanid = @tenanid)as TotalTransaksiHariIdr,
                    (SELECT sum(sellingpertransactioninusd) as TotalTransaksiHari from tenantdailysalesmonitoring where date = current_date AND tenanid = @tenanid)as TotalTransaksiHariUsd,
                    (SELECT sum(sellingpertransaction) as TotalTransaksiHari from tenantdailysalesmonitoring where date = current_date - 1 AND tenanid = @tenanid)as TotalTransaksiKemarinIdr,
                    (SELECT sum(sellingpertransactioninusd) as TotalTransaksiHari from tenantdailysalesmonitoring where date = current_date - 1 AND tenanid = @tenanid)as TotalTransaksiKemarinUsd,
                    (SELECT sum(sellingpertransaction) as TotalTransaksiTahun from tenantdailysalesmonitoring where extract(year from date)= extract(year from current_date) AND tenanid = @tenanid) as TotalTransaksiTahunIdr,
                    (SELECT sum(sellingpertransactioninusd) as TotalTransaksiTahun from tenantdailysalesmonitoring where extract(year from date)= extract(year from current_date) AND tenanid = @tenanid) as TotalTransaksiTahunUsd
	                                              from tenan where tenanid=@tenanid")]

    public class SummaryHomeTenan : IViewModel
    {
        public int TenanId { get; set; }
        public decimal TotalTransaksiHariIdr { get; set; }
        public decimal TotalTransaksiHariUsd { get; set; }
        public decimal TotalTransaksiKemarinIdr { get; set; }
        public decimal TotalTransaksiKemarinUsd { get; set; }
        public decimal TotalTransaksiBulanIdr { get; set; }
        public decimal TotalTransaksiBulanUsd { get; set; }
        public decimal TotalTransaksiBulanKemarinIdr { get; set; }
        public decimal TotalTransaksiBulanKemarinUsd { get; set; }
        public decimal TotalTransaksiTahunIdr { get; set; }
        public decimal TotalTransaksiTahunUsd { get; set; }
    }
}
