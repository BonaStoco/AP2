using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class ReportFakturPajakRepository : IReportFakturPajakRepository
    {
        QueryObjectMapper queryObjectMapper;

        public ReportFakturPajakRepository()
        {
            queryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }

        public FakturPajak reportFakturPajakFindByPeriodeAndTenan(string periode, int tenanId)
        {
            return queryObjectMapper.Map<FakturPajak>("findByTenanAndPeriode", new string[] { "periode", "tenanid"}, new object[] { periode, tenanId }).FirstOrDefault();
        }

        public void UpdateNoFakturPajakByTenanAndPeriode(string periode, int tenanId, string noFakturPajak)
        {
            queryObjectMapper.Map<FakturPajak>("UpdateNoFakturByTenan", new string[3] { "periode", "tenanid", "nofakturpajak" },
                new object[3] { periode, tenanId, noFakturPajak }).FirstOrDefault();
        }


        public MasterData.Models.ExchangeRate FindRateUSD(DateTime currDate)
        {
            return queryObjectMapper.Map<ExchangeRate>("FindRateUSD", new string[1] { "date" }, new object[1] { currDate.Date }).FirstOrDefault();
        
        }

        public void UpdateNoFakturPajakAP2ByTenanAndPeriode(string periode, int tenanId, string noFakturPajak)
        {
            queryObjectMapper.Map<FakturPajakAP2>("UpdateNoFakturByTenan", new string[3] { "periode", "tenanid", "nofakturpajak" },
                new object[3] { periode, tenanId, noFakturPajak }).FirstOrDefault();
        }
        
        public FakturPajakAP2 reportFakturPajakAP2FindByPeriodeAndTenan(string periode, int tenanId)
        {
            return queryObjectMapper.Map<FakturPajakAP2>("findByTenanAndPeriode", new string[] { "periode", "tenanid" }, new object[] { periode, tenanId }).FirstOrDefault();

        }
    }
}