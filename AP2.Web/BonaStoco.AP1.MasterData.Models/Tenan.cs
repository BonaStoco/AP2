using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindByTenanId", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid	left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid  left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid Where t.categoryid IN (3,4) and t.tenanid=@tenanid and t.tenantstatus=1")]
    [NamedSqlQuery("FindAllTenanByCategoryId", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.categoryid IN (3,4) and t.tenantstatus=1 ORDER BY t.tenanid")]
    [NamedSqlQuery("FindTenantByBandara", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.locationid=@bandaraid and t.categoryid IN (3,4) and t.tenantstatus=1 ORDER BY t.tenanid")]
    [NamedSqlQuery("FindTenantByBandaraAndTerminal", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.locationid=@bandaraid and t.terminalid=@terminalid and t.categoryid IN (3,4) and t.tenantstatus=1 ORDER BY t.tenanid")]
    [NamedSqlQuery("FindTenantByBandaraAndTerminalAndSubTerminal", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.locationid=@bandaraid and t.terminalid=@terminalid and t.subterminalid=@subterminal and t.categoryid IN (3,4) and t.tenantstatus=1 ORDER BY t.tenanid")]
    [NamedSqlQuery("FindTenantIdByBandara", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.categoryid IN (3,4) and t.tenanid=@tenanid and t.locationid=@bandaraid and t.tenantstatus=1")]
    [NamedSqlQuery("FindTenantIdByBandaraAndTerminal", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.categoryid IN (3,4) and t.tenanid=@tenanid and t.locationid=@bandaraid and t.terminalid=@terminalid and t.tenantstatus=1")]
    [NamedSqlQuery("FindTenantIdByBandaraAndTerminalAndSubTerminal", "select * from tenan t left join mappingcompany c on t.locationid=c.locationid  left join mappingterminal ter on ter.terminalid=t.terminalid left join mappingsubterminal sub on sub.subterminalid=t.subterminalid left join tenantype tt on tt.tenantypeid=t.tenantypeid left join producttype pt on pt.producttypeid=t.producttypeid WHERE t.categoryid IN (3,4) and t.tenanid='4083' and t.locationid=@bandaraid and t.terminalid=@terminalid and t.subterminalid=@subterminal and t.tenantstatus=1")]

    public class Tenan : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Alamat { get; set; }
        public string Npwp { get; set; }
        public string Nppkp { get; set; }
        public int LocationId { get; set; }
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string NameCompany { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime TanggalBergabung { get; set; }
        public decimal Tarif { get; set; }
        public int SubTerminalId { get; set; }
        public string SubTerminalName { get; set; }
        public int CategoryId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int TenanTypeId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int ProductTypeId { get; set; }
        public string Gate { get; set; }
        public string TenanTypeName { get; set; }
        public string ProductTypeName { get; set; }
        public decimal Target { get; set; }
        public string CcyCode { get; set; }
        public int HeadOffice { get; set; }
        public string FormulaKonsesi { get; set; }
        public bool MappingPrice { get; set; }

    }

    [SqlQuery(@"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4)")]
    [NamedSqlQuery("FindTenanByName", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and Lower(tenanname) like @key")]
    [NamedSqlQuery("FindTenantByBandara", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid = @bandaraid")]
    [NamedSqlQuery("FindTenantByBandaraAndName",@"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid = @bandaraid and LOWER(tenanname) like @key")]
    [NamedSqlQuery("FindTenantByBandaraAndTerminal", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid=@bandaraid and terminalid=@terminalid")]
    [NamedSqlQuery("FindTenantByBandaraTerminalAndName",@"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid=@bandaraid and terminalid=@terminalid and LOWER(tenanname) like @key")]
    [NamedSqlQuery("FindTenantByBandaraAndTerminalAndSubTerminal", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid=@bandaraid and terminalid=@terminalid and subterminalid=@subterminal")]
    [NamedSqlQuery("FindTenantByBandaraAndTerminalAndSubTerminalAndName", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and locationid=@bandaraid and terminalid=@terminalid and subterminalid=@subterminal and LOWER(tenanname) like @key")]

    public class TenanAdvancedSearch : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Alamat { get; set; }
        public string Gate { get; set; }
    }

    [SqlQuery(@"Select locationid as LocationId , namecompany as NameBandara from mappingcompany")]
    [NamedSqlQuery("FindBandaraById", "select * from mappingcompany where locationid = @id")]
    public class BandaraAdvanceSearch : IViewModel
    {
        public int LocationId { get; set; }
        public string NameBandara { get; set; }
    }

    [NamedSqlQuery("FindTenanActive", @"select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan where tenanid in (@data)")]

    public class TenanMonitoring : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Alamat { get; set; }
        public string Gate { get; set; }
    }

    [SqlQuery(@"Select locationid as LocationId , namecompany as NameBandara from mappingcompany where locationid = 2 ")]
    public class FindBandaraForMonitoringTenan : IViewModel
    {
        public int LocationId { get; set; }
    }

    [SqlQuery (@"Select * From tenantype")]
    [NamedSqlQuery("TenanTypeById", @"Select * From tenantype where tenantypeid=@tenantypeid")]
    [NamedSqlQuery("FindTenanTypeByName", @"Select * From tenantype where LOWER(tenantypename) = @name")]
    public class TenanType : IViewModel
    {
        public int TenanTypeId { get; set; }
        public string TenanTypeName { get; set; }
    }

    [SqlQuery(@"Select * From producttype")]
    [NamedSqlQuery("findproducttype", "select * from producttype where producttypeid=@producttypeid")]
    [NamedSqlQuery("FindProductTypeByName", "select * from producttype where LOWER(producttypename) = @name")]
    public class ProductType : IViewModel
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
    }

    [SqlQuery(@"Select locationid as LocationId , namecompany as NameBandara from mappingcompany where locationid = 1 ")]
    public class FindBandaraForMonitoringTenanAP2 : IViewModel
    {
        public int LocationId { get; set; }
    }

    [SqlQuery(@"Select kode as CcyCode From ccy")]
    [NamedSqlQuery("FindCcyCodeByName", @"Select kode as CcyCode From ccy where LOWER(kode) = @ccyCode")]
    public class FindCcyCode : IViewModel
    {
        public string CcyCode { get; set; }
    }


    [SqlQuery(@"Select * FROM tenan WHERE categoryid IN (3,4) AND mappingprice = TRUE")]
    [NamedSqlQuery("FindTenanLoungeByName", @"SELECT tenanid as TenanId, tenanname as TenanName, alamat as Alamat, gate as Gate FROM tenan WHERE categoryid IN (3,4) and mappingprice = TRUE and Lower(tenanname) like @key")]
    public class TenanLounge : IViewModel
    {
        public int TenanId { get; set; }
        public string TenanName { get; set; }
        public string Alamat { get; set; }
        public string Gate { get; set; }
        public bool MappingPrice { get; set; }
    }
}