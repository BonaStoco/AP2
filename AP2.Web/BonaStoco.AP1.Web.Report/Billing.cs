using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.Web.Report
{
     [NamedSqlQuery("FindBillingDetailByPeriodeandTenanId", "select b.period as Period, bl.billingid as BillingId, bl.categoryid as CategoryId,  bl.tenanid as TenanId, bl.bandaraid as BandaraId, bl.terminalid as TerminalId, bl.subterminalid as SubTerminalId, bl.date as Tanggal, dailysales as Jumlah from billing b inner join billingdetail bl on b.id = bl.billingid where b.period=@period and bl.tenanid=@tenanid")]

     [NamedSqlQuery("FindBillingDetailByPeriodeandBandaraId", "select b.period as Period, bl.billingid as BillingId, bl.categoryid as CategoryId,  bl.tenanid as TenanId, bl.bandaraid as BandaraId, bl.terminalid as TerminalId, bl.subterminalid as SubTerminalId, bl.date as Tanggal, dailysales as Jumlah from billing b inner join billingdetail bl on b.id = bl.billingid where b.period=@period and bl.tenanid=@tenanid and bl.bandaraid=@bandaraid")]

     [NamedSqlQuery("FindBillingDetailByPeriodeandTerminalId", "select b.period as Period, bl.billingid as BillingId, bl.categoryid as CategoryId,  bl.tenanid as TenanId, bl.bandaraid as BandaraId, bl.terminalid as TerminalId, bl.subterminalid as SubTerminalId, bl.date as Tanggal, dailysales as Jumlah from billing b inner join billingdetail bl on b.id = bl.billingid where b.period=@period and bl.tenanid=@tenanid and bl.bandaraid=@bandaraid and bl.terminalid=@terminalid")]

     [NamedSqlQuery("FindBillingDetailByPeriodeandSubTerminalId", "select b.period as Period, bl.billingid as BillingId, bl.categoryid as CategoryId,  bl.tenanid as TenanId, bl.bandaraid as BandaraId, bl.terminalid as TerminalId, bl.subterminalid as SubTerminalId, bl.date as Tanggal, dailysales as Jumlah from billing b inner join billingdetail bl on b.id = bl.billingid where b.period=@period and bl.tenanid=@tenanid and bl.bandaraid=@bandaraid and bl.terminalid=@terminalid and bl.subterminalid=@subterminalid")]

    public class Billing:IViewModel
    {
        public long BillingId {get;set;}
        public int CategoryId { get; set; }
        public int TenanId {get;set;}       
        public int BandaraId { get; set; }
        public int TerminalId { get; set; }
        public int SubTerminalId { get; set; }
        public string Tanggal {get;set;}
        public decimal Jumlah {get;set;}
        public string Period { get; set; }
       
    }
}
