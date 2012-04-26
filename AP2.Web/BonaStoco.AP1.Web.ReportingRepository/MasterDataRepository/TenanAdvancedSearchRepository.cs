using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.Inf.DataMapper.Impl;

namespace BonaStoco.AP1.MasterData.Repository
{
    public class TenanAdvancedSearchRepository : ITenanAdvancedSearchRepository
    {
        QueryObjectMapper qryObjectMapper;

        public TenanAdvancedSearchRepository(QueryObjectMapper qryObjectMapper)
        {
            this.qryObjectMapper = qryObjectMapper;
        }

        public IList<TenanAdvancedSearch> GetAllTenan()
        {
            return qryObjectMapper.Map<TenanAdvancedSearch>().ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandara(int bandaraId)
        {
            string _bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandara",
                new string[] { "bandaraid" },
                new object[] { _bandaraid }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminal(int bandaraId, int terminalId)
        {
            string _bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandaraAndTerminal",
                new string[] { "bandaraid", "terminalid" },
                new object[] { bandaraId, terminalId }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminalAndSubTerminal(int bandaraId, int terminalId, int subTerminalId)
        {
            string _bandaraid = bandaraId.ToString();
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandaraAndTerminalAndSubTerminal",
                new string[] { "bandaraid", "terminalid", "subterminal" },
                new object[] { bandaraId, terminalId, subTerminalId }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenanByName(string key)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenanByName",
                new string[] { "key" },
                new object[] { _key }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandaraAndName(string key, int bandaraId)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandaraAndName",
                new string[] { "bandaraid", "key" },
                new object[] { bandaraId , _key }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandaraTerminalAndName(string key, int bandaraId, int terminalId)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandaraTerminalAndName",
                new string[] { "bandaraid", "terminalid", "key" },
                new object[] { bandaraId, terminalId, _key }).ToList();
        }

        public IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminalAndSubTerminalAndName(string key, int bandaraId, int terminalId, int subTerminalId)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<TenanAdvancedSearch>("FindTenantByBandaraAndTerminalAndSubTerminalAndName",
                new string[] { "bandaraid", "terminalid", "subterminal", "key" },
                new object[] { bandaraId, terminalId, subTerminalId, _key }).ToList();
        }


        public IList<TenanLounge> FindTenanLounge()
        {
            return qryObjectMapper.Map<TenanLounge>().ToList();
        }


        public IList<TenanLounge> FindTenanLoungeByName(string key)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<TenanLounge>("FindTenanLoungeByName",
                new string[] { "key" },
                new object[] { _key }).ToList();
        }
    }
}
