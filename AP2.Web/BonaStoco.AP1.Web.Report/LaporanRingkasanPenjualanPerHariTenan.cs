using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
    [NamedSqlQuery("FindDataRingkasanPerHariTenan", @"select i.transactiondate as TransactionDate, i.tenanid as TenanId,
                                                        tenan.tenanname as TenanName, i.companylocationid as CompanyLocationId, 
                                                        i.totalsalepertenan as TransactionIDR,
                                                        i.totalsalespertenantinusd as TransactionUSD,
                                                        (select sum(i.totalsalepertenan) from tenantdailysales i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai) as TotalIDR,
                                                        (select sum(i.totalsalespertenantinusd) from tenantdailysales i inner join tenan on i.tenanid = tenan.tenanid where i.tenanid = @tenanid AND i.date between @dari AND @sampai) as TotalUSD

                                                from 
                                                    tenantdailysales i inner join
                                                    tenan on i.tenanid = tenan.tenanid
                                                where 
                                                        i.tenanid = @tenanid AND i.date between @dari AND @sampai")]

    public class LaporanRingkasanPenjualanPerHariTenan : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string TransactionDate { get; set; }
        public int CompanyLocationId { get; set; }
        public decimal TransactionIDR { get; set; }
        public decimal TransactionUSD { get; set; }
        public decimal TotalIDR { get; set; }
        public decimal TotalUSD { get; set; }
    }

    //Buat Repository dan interface
}
