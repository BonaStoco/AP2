using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class AP2ReportRepository : IAP2ReportRepository
    {
        QueryObjectMapper _QueryMapper;
        public AP2ReportRepository()
        {
            _QueryMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
        }
        public IList<DetailFakturAP2> FindDetailFakturByPeriodeandTenanId(string period, int tenanId)
        {

            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandTenanId", new string[] { "period", "tenanid" }, new object[] { period, tenanId }).ToList();
        }
        public IList<DetailFakturAP2> FindBillingDetailByPeriodeandTenanId(string period, int tenanId)
        {

            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandTenanId", new string[] { "period", "tenanid" }, new object[] { period, tenanId }).ToList();
        }

        public IList<DetailFakturAP2> FindBillingDetailByPeriodeandBandaraId(string period, int tenanId, int bandaraId)
        {

            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandBandaraId", new string[] { "period", "tenanid", "bandaraid" }, new object[] { period, tenanId, bandaraId }).ToList();

        }

        public IList<DetailFakturAP2> FindBillingDetailByPeriodeandTerminalId(string period, int tenanId, int bandaraId, int terminalId)
        {
            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandTerminalId", new string[] { "period", "tenanid", "bandaraid", "terminalid" }, new object[] { period, tenanId, bandaraId, terminalId }).ToList();

        }

        public IList<DetailFakturAP2> FindBillingDetailByPeriodeandSubTerminalId(string period, int tenanId, int bandaraId, int terminalId, int subTerminalId)
        {
            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandSubTerminalId", new string[] { "period", "tenanid", "bandaraid", "terminalid", "subterminalid" }, new object[] { period, tenanId, bandaraId, terminalId, subTerminalId }).ToList();

        }

        public IList<DetailFakturAP2> FindBillingDetailByPeriodeandCategoryId(string period, int categoryId)
        {
            return _QueryMapper.Map<DetailFakturAP2>("FindBillingDetailByPeriodeandCategoryId", new string[] { "period", "categoryid" }, new object[] { period, categoryId }).ToList();

        }

        public FakturAP2 FindFakturAPByPeriodeandTenanId(string period, string tenanId)
        {
            return _QueryMapper.Map<FakturAP2>("FindFakturAPByPeriodeandTenanId", new string[] { "period", "tenanid" }, new object[] { period, Int64.Parse(tenanId) }).FirstOrDefault();
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
            return new Tenan { TenanId = Int32.Parse(tenanId), TenanName = tenanMessages[1] };
        }
        public void UpdateBillingByTenan(int tenanid, string period, string ttd, string nip, string noFaktur)
        {
            _QueryMapper.Map<FakturAP2>("UpdateBilling", new string[5] { "tenanid", "periode", "ttd", "nip", "nofaktur" },
                new object[5] { tenanid, period, ttd, nip, noFaktur });
        }


        public ExchangeRate FindRateUSD(DateTime currDate)
        {
            return _QueryMapper.Map<ExchangeRate>("FindRateUSD", new string[1] { "date" }, new object[1] { currDate.Date }).FirstOrDefault();
        }



    }
}
