using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IAPMasterRepository
    {
        FakturAP FindFakturAPByPeriodeandTenanId(string period, string tenanId);
        IList<TenanSpeedyPayment> FindBillingTenantByYear(string tahun);
        BonaStoco.AP1.Web.ReportingRepository.tenanws.serverResponse GetTenan(string tenanId);
        Tenan GetTenanByTenanId(string tenanId);
        IList<DetailFakturAP> FindDetailFakturByPeriodeandTenanId(string period, int tenanId);
        void CreatePaymentListForYear(string year);
        void CreateUnregisterTenantPaymentByYear(string year);
        void UpdateTenanPaymentByChecked(long tenanId, string tahun, string bulan, bool bulanCeklis);
        IList<TenanSpeedyPayment> FindBillingTenantByYearAndCategory(string tahun, int categoryId);
        IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandara(string tahun, int bandaraId);
        IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandaraAndTerminal(string tahun, int bandaraId, int terminalId);
        IList<TenanSpeedyPayment> FindBillingTenantByYearAndBandaraAndTerminalAndSubTerminal(string tahun, int bandaraId, int terminalId, int subTerminalId);
        IList<LaporanProduksi> FindAllByCategory(string period);
        void UpdateBillingByTenan(int tenanid, string period, string ttd, string nip, string noFaktur);
        IList<TenanProduct> FindTenanByDate(DateTime tanggal);
        IList<TenanProduct> FindAllTenanByWeek(DateTime startdate, DateTime enddate);
        IList<TenanProduct>  FindTenanByMonth();
        IList<TenanProduct> FindAllTenan();
        IList<ProductChange> FindProductChangeByTenanAndDate(int tenanid);
        IList<ProductChange> FindProductChangeByTenanAndWeek(int tenanid, DateTime startdate, DateTime enddate);
        IList<ProductChange> FindProductChangeByTenanAndMonth(int tenanid);
        IList<ProductChange> FindAllProductChange(int tenanid, int totalrow, int currpage);
        IList<BonaStoco.AP1.Web.Report.ProductPrint> FindAllProductByCode(string kode, int tenanid);
        IList<ProductChange> CountAllProductChangeByTenan(int tenanid);
        IList<SalesProductDetail> FindSalesDetailProductByTransactionNo(string transactionNo, long tenanId);
        IList<SalesSummaryProduct> FindSalesSummaryByTenantAndDate(string tenanId, DateTime tanggal);
        IList<SessionSummaryPerKasir> FindSessionPerkasirByTenantAndDate(int tenanId, DateTime tanggal);
        IList<SummaryPerKasir> FindSummaryPerkasirByDateAndTenan(int tenanId, DateTime tanggal, int sessionId);
        ExchangeRate FindRateUSD(DateTime currDate);
        IList<TenantDailySalesMonitoringEPOS> FindTenantDailySalesMoniyoringByDate(string date);
     
        void UpdateBilling(UpdateFakturAP updateFakturAP);
    }
}