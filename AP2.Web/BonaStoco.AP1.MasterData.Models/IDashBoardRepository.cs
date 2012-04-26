using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.MasterData.Models
{
    public interface IDashBoardRepository
    {
        SummaryHome GetSummaryAP();
        SummaryHome GetSummaryAP2();
        SummaryHome GetSummaryUmum();
        SummaryHomeBandara GetSummaryBandara(string locationid, string categoryid);
        SummaryHomeTerminal GetSummaryTerminal(string locationid, string categoryid, string terminalid);
        SummaryHomeSubTerminal GetSummarySubTerminal(string locationid, string categoryid, string terminalid, string subterminal);
        IList<DetailTenanAktif> FindDetaiTenanAktif();
        IList<DetailTenanAktif> FindDetailAkifInBandara(string locationid, string categoryid);
        IList<DetailTenanAktif> FindDetailAkifInTerminal(string locationid, string categoryid, string terminalid);
        IList<DetailTenanAktif> FindDetailAkifInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid);
        IList<DetailTenanAktifKemarin> FindDetailTenanAktifKemarin();
        IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInBandara(string locationid, string categoryid);
        IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInTerminal(string locationid, string categoryid, string terminalid);
        IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid);
        IList<DetailTenanAktifHariIni> FindDetailTenanAktifHariIni();
        IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInBandara(string locationid, string categoryid);
        IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInTerminal(string locationid, string categoryid, string terminalid);
        IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid);
        SummaryHomeTenan GetSummaryTenant(int tenanid);
        IList<DetailTenanAktif> FindTenanAktifByName(string key);
        IList<TenantSalesMonitoring> FindTenantSalesMonitoringByTenanAndMonthPeriode(int tenanId, string dari, string sampai);
        IList<Info> FindNewInfo();
    }
}
