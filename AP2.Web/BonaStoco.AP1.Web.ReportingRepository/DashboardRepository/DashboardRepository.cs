using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using BonaStoco.Inf.Reporting;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class DashboardRepository : IDashBoardRepository
    {
        QueryObjectMapper qryObjectMapper;
        IReportingRepository reportingRepository;

        public DashboardRepository()
        {
            qryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as QueryObjectMapper;
            reportingRepository = ContextRegistry.GetContext().GetObject("ReportingRepository") as IReportingRepository;
        }

        public SummaryHome GetSummaryAP()
        {
            return qryObjectMapper.Map<SummaryHome>().FirstOrDefault();
        }
        public SummaryHome GetSummaryAP2()
        {
            return qryObjectMapper.Map<SummaryHome>("SummaryAP2", new string[]{}, new object[]{}).FirstOrDefault();
        }
        public SummaryHome GetSummaryUmum()
        {
            return qryObjectMapper.Map<SummaryHome>("SummaryUmum", new string[] { }, new object[] { }).FirstOrDefault();
        }

        public SummaryHomeTenan GetSummaryTenant(int tenanid)
        {
            return qryObjectMapper.Map<SummaryHomeTenan>("FindDataToDashboardTenan", new string[] { "tenanid" }, new object[] { tenanid }).FirstOrDefault();
        }
        public SummaryHomeBandara GetSummaryBandara(string locationid, string categoryid)
        {
            return qryObjectMapper.Map<SummaryHomeBandara>("GetSummaryBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).FirstOrDefault();
        }
        public SummaryHomeTerminal GetSummaryTerminal(string locationid, string categoryid, string terminalid)
        {
            return qryObjectMapper.Map<SummaryHomeTerminal>("GetSummaryTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid, terminalid }).FirstOrDefault();
        }
        public SummaryHomeSubTerminal GetSummarySubTerminal(string locationid, string categoryid, string terminalid, string subterminal)
        {
            return qryObjectMapper.Map<SummaryHomeSubTerminal>("GetSummarySubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalid" },
                new object[4] { locationid, categoryid, terminalid, subterminal }).FirstOrDefault();
        }

        //tenan yang aktif
        public IList<DetailTenanAktif> FindDetaiTenanAktif()
        {
            return qryObjectMapper.Map<DetailTenanAktif>().ToList();
        }
        public IList<DetailTenanAktif> FindTenanAktifByName(string key)
        {
            string _key = "%" + key.ToLower() + "%";
            return qryObjectMapper.Map<DetailTenanAktif>("FindTenanAktifByName",
                new string[] { "key" }, new object[] { _key }).ToList();
        }
        public IList<DetailTenanAktif> FindDetailAkifInBandara(string locationid, string categoryid)
        {
            return qryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).ToList();
        }

        public IList<DetailTenanAktif> FindDetailAkifInTerminal(string locationid, string categoryid, string terminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid, terminalid }).ToList();
        }

        public IList<DetailTenanAktif> FindDetailAkifInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInSubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalid" },
                new object[4] { locationid, categoryid, terminalid, subterminalid }).ToList();
        }
        
        //tenan yang aktif kemarin
        public IList<DetailTenanAktifKemarin> FindDetailTenanAktifKemarin()
        {
            return qryObjectMapper.Map<DetailTenanAktifKemarin>().ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInBandara(string locationid, string categoryid)
        {
            return qryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInTerminal(string locationid, string categoryid, string terminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid, terminalid }).ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInSubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalid" },
                new object[4] { locationid, categoryid, terminalid, subterminalid }).ToList();
        }

        //tenan yang aktif hari ini
        public IList<DetailTenanAktifHariIni> FindDetailTenanAktifHariIni()
        {
            return qryObjectMapper.Map<DetailTenanAktifHariIni>().ToList();
        }

        public IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInBandara(string locationid, string categoryid)
        {
            return qryObjectMapper.Map<DetailTenanAktifHariIni>("DetailTenanAktifHariIniInBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).ToList();
        }

        public IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInTerminal(string locationid, string categoryid, string terminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktifHariIni>("DetailTenanAktifHariIniInTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid, terminalid }).ToList();
        }

        public IList<DetailTenanAktifHariIni> FindDetailAkifHariIniInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid)
        {
            return qryObjectMapper.Map<DetailTenanAktifHariIni>("DetailTenanAktifHariIniInSubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalId" },
                new object[4] { locationid, categoryid, terminalid, subterminalid }).ToList();
        }

        public IList<TenantSalesMonitoring> FindTenantSalesMonitoringByTenanAndMonthPeriode(int tenanId, string dari, string sampai)
        {
            return qryObjectMapper.Map<TenantSalesMonitoring>("FindByTenantAndMonthRange", 
                new string[3] { "tenanId", "dari", "sampai" }, 
                new object[3] { tenanId, dari, sampai }).ToList();
        }

        public IList<Info> FindNewInfo()
        {
            return qryObjectMapper.Map<Info>("SELECT * FROM info").ToList();
        }

    }
}
