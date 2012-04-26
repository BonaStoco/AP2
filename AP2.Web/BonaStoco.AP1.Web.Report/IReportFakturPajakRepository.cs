using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.Report
{
    public interface IReportFakturPajakRepository
    {
        FakturPajak reportFakturPajakFindByPeriodeAndTenan(string periode, int tenanId);
        void UpdateNoFakturPajakByTenanAndPeriode(string periode, int tenanId, string noFakturPajak);
        ExchangeRate FindRateUSD(DateTime currDate);

        void UpdateNoFakturPajakAP2ByTenanAndPeriode(string periode, int tenanId, string noFakturPajak);
        FakturPajakAP2 reportFakturPajakAP2FindByPeriodeAndTenan(string periode, int tenanId);
    }
}
