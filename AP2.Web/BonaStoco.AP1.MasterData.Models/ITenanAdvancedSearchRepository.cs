using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaStoco.AP1.MasterData.Models
{
    public interface ITenanAdvancedSearchRepository
    {
        IList<TenanAdvancedSearch> GetAllTenan();
        IList<TenanAdvancedSearch> FindTenantByBandara(int bandaraId);
        IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminal(int bandaraId, int terminalId);
        IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminalAndSubTerminal(int bandaraId, int terminalId, int subTerminalId);
        IList<TenanAdvancedSearch> FindTenanByName(string key);
        IList<TenanAdvancedSearch> FindTenantByBandaraAndName(string key, int bandaraId);
        IList<TenanAdvancedSearch> FindTenantByBandaraTerminalAndName(string key, int bandaraId, int terminalId);
        IList<TenanAdvancedSearch> FindTenantByBandaraAndTerminalAndSubTerminalAndName(string key, int bandaraId, int terminalId, int subTerminalId);
        IList<TenanLounge> FindTenanLounge();
        IList<TenanLounge> FindTenanLoungeByName(string key);
    }
}
