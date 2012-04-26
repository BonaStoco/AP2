using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;


namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IAP2ReportRepository
    {
        IList<DetailFakturAP2> FindBillingDetailByPeriodeandTenanId(string period, int tenanId);
        IList<DetailFakturAP2> FindBillingDetailByPeriodeandBandaraId(string period, int tenanId, int bandaraId);
        IList<DetailFakturAP2> FindBillingDetailByPeriodeandTerminalId(string period, int tenanId, int bandaraId, int terminalId);
        IList<DetailFakturAP2> FindBillingDetailByPeriodeandSubTerminalId(string period, int tenanId, int bandaraId, int terminalId, int subTerminalId);
        IList<DetailFakturAP2> FindBillingDetailByPeriodeandCategoryId(string period, int categoryId);
        FakturAP2 FindFakturAPByPeriodeandTenanId(string period, string tenanId);
        BonaStoco.AP1.Web.ReportingRepository.tenanws.serverResponse GetTenan(string tenanId);
        Tenan GetTenanByTenanId(string tenanId);
        void UpdateBillingByTenan(int tenanid, string period, string ttd, string nip, string noFaktur);
        ExchangeRate FindRateUSD(DateTime currDate);
        IList<DetailFakturAP2> FindDetailFakturByPeriodeandTenanId(string period, int tenanId);
    }
}
