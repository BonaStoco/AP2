using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;
using Spring.Data.Generic;
using Spring.Data.Common;
using System.Data;
using BonaStoco.Inf.Reporting;
namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class APMasterRepository : IAPMasterRepository
    {       
        QueryObjectMapper _QueryMapper;
        AdoTemplate _AdoTemplate;
        IReportingRepository _ReportingRepo;
        public APMasterRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
            _AdoTemplate = ContextRegistry.GetContext().GetObject("AdoTemplate") as AdoTemplate;
            _ReportingRepo = ContextRegistry.GetContext().GetObject("ReportingRepository") as IReportingRepository;
        }

        public FakturAP FindFakturAPByPeriodeandTenanId(string period, string tenanId)
        {
            return _QueryMapper.Map<FakturAP>("FindFakturAPByPeriodeandTenanId", new string[] { "period", "tenanid" }, new object[] { period, tenanId}).FirstOrDefault();
        }
       
        public BonaStoco.AP1.Web.ReportingRepository.tenanws.serverResponse GetTenan(string tenanId)
        {
            return new BonaStoco.AP1.Web.ReportingRepository.tenanws.BonastocoServices().gettenant(
                new BonaStoco.AP1.Web.ReportingRepository.tenanws.askTenant() { tenantid = tenanId, token = "" });
        }
        public Tenan GetTenanByTenanId(string tenanId)
        {
            var tenanWS = new BonaStoco.AP1.Web.ReportingRepository.tenanws.BonastocoServices().gettenant(
                new BonaStoco.AP1.Web.ReportingRepository.tenanws.askTenant() { tenantid = tenanId, token = "" });

            if (tenanWS.status != 0)
                return null;

            string[] tenanMessages = tenanWS.message.Split(new char[] { ';', '=' });
            return new Tenan { TenanId = Int32.Parse(tenanId), TenanName = tenanMessages[1] } ;
        }

        public IList<DetailFakturAP> FindDetailFakturByPeriodeandTenanId(string period, int tenanId)
        {

            return _QueryMapper.Map<DetailFakturAP>("FindBillingDetailByPeriodeandTenanId", new string[] { "period", "tenanid" }, new object[] { period, tenanId}).ToList();
        }

        public IList<TenanSpeedyPayment> FindBillingTenantByYear(string tahun)
        {
            return _QueryMapper.Map<TenanSpeedyPayment>("FindBillingTenantByYear", 
                new string[] { "tahun" }, 
                new object[] { tahun }).ToList();
        }

        public IList<TenanSpeedyPayment> FindBillingTenantByYearAndCategory(string tahun, int categoryId)
        {
            return _QueryMapper.Map<TenanSpeedyPayment>("FindBillingTenantByYearAndCategory", 
                new string[] { "tahun", "categoryId" },
                new object[] { tahun, categoryId }).ToList();
        }

        public IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandara(string tahun, int bandaraId)
        {
            return _QueryMapper.Map<TenanSpeedyPayment>("FindBillingTenantByYearAndBandara", 
                new string[] { "tahun" , "bandaraId" }, 
                new object[] { tahun, bandaraId }).ToList();
        }

        public IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandaraAndTerminal(string tahun, int bandaraId, int terminalId)
        {
            return _QueryMapper.Map<TenanSpeedyPayment>("FindBillingTenantByYearAndBandaraAndTerminal",
                new string[] { "tahun", "bandaraId", "terminalId" },
                new object[] { tahun, bandaraId, terminalId }).ToList();
        }

        public IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandaraAndTerminalAndSubTerminal(string tahun, int bandaraId, int terminalId, int subTerminalId)
        {
            return _QueryMapper.Map<TenanSpeedyPayment>("FindBillingTenantByYearAndBandaraAndTerminalAndSubTerminal",
                new string[] { "tahun", "bandaraId", "terminalId", "subterminalId" },
                new object[] { tahun, bandaraId, terminalId, subTerminalId }).ToList();
        }

        public void CreatePaymentListForYear(string year)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into billingtenant(tenanid,tahun) select tenanid, @year from tenan where discontinue = 0 and categoryid in(3,4)", "year", DbType.String, 4, year);
        }

        public void UpdateTenanPaymentByChecked(long tenanId, string tahun, string bulan, bool bulanCeklist)
        {
            IDbParameters dbParameter = _AdoTemplate.CreateDbParameters(new string[3] { "bulanCeklist", "tenanId", "tahun" }, new object[3] { bulanCeklist, tenanId, tahun });
            string sqlQuery;
            sqlQuery = GetUpdateBillingTenantQuery(bulan);
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, sqlQuery, dbParameter);
        }

        public void CreateUnregisterTenantPaymentByYear(string year)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into billingtenant(tenanid,tahun) select tenanid, @year from tenan where discontinue = 0 and categoryid in(3,4) and tenanid not in (select tenanid from billingtenant where tahun = @year)", "year", DbType.String, 4, year);
        }

        private string GetUpdateBillingTenantQuery(string bulan)
        {
            string sql = "";
            switch (bulan)
            {
                case "jan":
                    sql = "update billingtenant set januari=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "feb":
                    sql = "update billingtenant set februari=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "mar":
                    sql = "update billingtenant set maret=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "apr":
                    sql = "update billingtenant set april=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "mei":
                    sql = "update billingtenant set mei=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "jun":
                    sql = "update billingtenant set juni=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "jul":
                    sql = "update billingtenant set juli=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "agu":
                    sql = "update billingtenant set agustus=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "sep":
                    sql = "update billingtenant set september=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "okt":
                    sql = "update billingtenant set oktober=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "nov":
                    sql = "update billingtenant set november=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
                case "des":
                    sql = "update billingtenant set desember=@bulanCeklist where tenanid=@tenanId and tahun=@tahun";
                    break;
            }
            return sql;
        }

        public IList<LaporanProduksi> FindAllByCategory(string period)
        {
            return _QueryMapper.Map<LaporanProduksi> ("FindAllByCategory", new string[1] { "period" }, new object[1] { period }).ToList();
        }

        public void UpdateBillingByTenan(int tenanid, string period, string ttd, string nip, string noFaktur)
        {
            _QueryMapper.Map<FakturAP>("UpdateBilling", new string[5] { "tenanid", "periode", "ttd", "nip", "nofaktur" },
                new object[5] { tenanid, period, ttd, nip, noFaktur });
        }


        public IList<TenanProduct> FindTenanByDate(DateTime tanggal)
        {
            return _QueryMapper.Map<TenanProduct>("FindTenanByDate",
                new string[1] { "tanggal" },
                new object[1] { tanggal.Date }).ToList();
        }
        
        public IList<TenanProduct> FindAllTenanByWeek(DateTime startdate, DateTime enddate)
        {
            return _QueryMapper.Map<TenanProduct>("FindAllTenanByWeek",
                new string[2] { "startdate", "enddate" },
                new object[2] { startdate, enddate }).ToList();
        }
        
        public IList<TenanProduct> FindTenanByMonth()
        {
            return _QueryMapper.Map<TenanProduct>("SELECT DISTINCT ON(tenanid) tenanid, tenanname, to_char(tanggal, 'mm-yyyy') as tanggal FROM productchange WHERE to_char(tanggal, 'mm-yyyy')=to_char(now(), 'mm-yyyy') ORDER BY tenanid").ToList();
        }

        public IList<TenanProduct> FindAllTenan()
        {
            return _QueryMapper.Map<TenanProduct>().ToList();
        }

        public IList<ProductChange> FindProductChangeByTenanAndDate(int tenanid)
        {
            return _QueryMapper.Map<ProductChange>("FindProductChangeByTenanAndDate",
                new string[1] { "tenanid"},
                new object[1] { tenanid }).ToList();
        }

        public IList<ProductChange> FindProductChangeByTenanAndMonth(int tenanid)
        {
            return _QueryMapper.Map<ProductChange>("FindProductChangeByTenanAndMonth",
                new string[1] { "tenanid" },
                new object[1] { tenanid }).ToList();
        }
        
        public IList<ProductChange> FindProductChangeByTenanAndWeek(int tenanid, DateTime startdate, DateTime enddate)
        {
            return _QueryMapper.Map<ProductChange>("FindProductChangeByTenanAndWeek",
                new string[3] { "tenanid", "startdate", "enddate" },
                new object[3] { tenanid, startdate, enddate }).ToList();
        }

        public IList<ProductChange> FindAllProductChange(int tenanid, int totalrow, int currpage)
        {
            return _QueryMapper.Map<ProductChange>("FindAllProductChangeByTenan", 
                    new string[3]{"tenanid", "totalrow", "currpage"}, 
                    new object[3]{ tenanid, totalrow, currpage }).ToList();
        }

        public IList<ProductChange> CountAllProductChangeByTenan(int tenanid)
        {
            return _QueryMapper.Map<ProductChange>("CountAllProductChangeByTenan",
                    new string[1] { "tenanid" },
                    new object[1] { tenanid }).ToList();
        }

        public IList<BonaStoco.AP1.Web.Report.ProductPrint> FindAllProductByCode(string kode, int tenanid)
        {
            return _QueryMapper.Map<BonaStoco.AP1.Web.Report.ProductPrint>("FindAllProductByCode",
                new string[2] { "kode", "tenanid" },
                new object[2] { kode, tenanid }).ToList();
        }

        public IList<SalesProductDetail> FindSalesDetailProductByTransactionNo(string transactionNo ,long tenanId)
        {
            return _QueryMapper.Map<SalesProductDetail>("FindByTransactionNo",
                new string[] { "transactionNo", "tenanId" }, new object[] { transactionNo,tenanId }).ToList();
        }

        public IList<SalesSummaryProduct> FindSalesSummaryByTenantAndDate(string tenanId, DateTime tanggal)
        {
            return _QueryMapper.Map<SalesSummaryProduct>("FindSummaryByTenantAndDate",
                new string[] { "tenanId", "transactionDate" }, new object[] { Int64.Parse(tenanId), tanggal }).ToList();
        }

        public IList<SessionSummaryPerKasir> FindSessionPerkasirByTenantAndDate(int tenanId, DateTime tanggal)
        {
            return _QueryMapper.Map<SessionSummaryPerKasir>("FindSessionByTenantAndDate",
                new string[] { "tenanId", "tanggal" }, new object[] { tenanId, tanggal }).ToList();
        }
        public IList<SummaryPerKasir> FindSummaryPerkasirByDateAndTenan(int tenanId, DateTime tanggal, int sessionId)
        {
            return _QueryMapper.Map<SummaryPerKasir>("FindSummaryPerkasirByDateAndTenan",
                new string[] { "tenanId", "tanggal", "sessionId" }, new object[] { tenanId, tanggal, sessionId }).ToList();
        }

        public ExchangeRate FindRateUSD(DateTime currDate)
        {
            return _QueryMapper.Map<ExchangeRate>("FindRateUSD", new string[1] { "date" }, new object[1] { currDate.Date }).FirstOrDefault();
        }
        public IList<TenantDailySalesMonitoringEPOS> FindTenantDailySalesMoniyoringByDate(string date)
        {
            return _QueryMapper.Map<TenantDailySalesMonitoringEPOS>("FindByDate", new string[1] { "date" }, new object[1] { date }).ToList();
        }

        public void UpdateBilling(UpdateFakturAP updateFakturAP)
        {
            _QueryMapper.Map<UpdateFakturAP>(Update(updateFakturAP));
        }


        public string Update(UpdateFakturAP updateFakturAP)
        {
            return string.Format("UPDATE billing SET nofaktur='{0}', ttd='{1}',nip='{2}',bagihasil={3},konsesi={4},formulakonsesi='{5}',pajakbagihasil={6}, penjualan={7},tagihan={8},tarif={9}, target={10}  WHERE period='{11}' and tenanid='{12}'",
                                  updateFakturAP.NoFaktur, updateFakturAP.Ttd, updateFakturAP.Nip, updateFakturAP.BagiHasil, updateFakturAP.Konsesi, updateFakturAP.FormulaKonsesi, updateFakturAP.PajakBagiHasil, updateFakturAP.Penjualan, updateFakturAP.Tagihan, updateFakturAP.Tarif, updateFakturAP.Target, updateFakturAP.Period, updateFakturAP.TenanId);
        }
    }
}

