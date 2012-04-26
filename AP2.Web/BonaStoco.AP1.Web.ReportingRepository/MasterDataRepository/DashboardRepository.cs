using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.Inf.DataMapper.Impl;
using BonaStoco.Inf.Reporting;

namespace BonaStoco.AP1.MasterData.Repository
{
    public class DashboardRepository : IDashBoardRepository  
    {
        public QueryObjectMapper QryObjectMapper { get; set; }
        public IReportingRepository ReportingRepository { get; set; }

        public SummaryHome GetSummaryAP()
        {
            return QryObjectMapper.Map<SummaryHome>().FirstOrDefault();
        }

        public SummaryHomeTenan GetSummaryTenant(int tenanid)
        {
            return QryObjectMapper.Map<SummaryHomeTenan>("FindDataToDashboardTenan", new string[] { "tenanid" }, new object[] { tenanid }).FirstOrDefault();
        }
        public SummaryHomeBandara GetSummaryBandara(string locationid, string categoryid)
        {
            return QryObjectMapper.Map<SummaryHomeBandara>("GetSummaryBandara", 
                new string[2] { "locationid", "categoryid" }, 
                new object[2] { locationid, categoryid }).FirstOrDefault();
        }
        public SummaryHomeTerminal GetSummaryTerminal(string locationid, string categoryid, string terminalid)
        {
            return QryObjectMapper.Map<SummaryHomeTerminal>("GetSummaryTerminal",
                new string[3]{"locationid","categoryid","terminalid"},
                new object[3]{locationid,categoryid,terminalid}).FirstOrDefault();
        }
        public SummaryHomeSubTerminal GetSummarySubTerminal(string locationid, string categoryid, string terminalid, string subterminal )
        {
            return QryObjectMapper.Map<SummaryHomeSubTerminal>("GetSummarySubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalid" },
                new object[4] { locationid, categoryid, terminalid, subterminal }).FirstOrDefault();
        }

        //tanan yang aktif hari ini
        public IList<DetailTenanAktif> FindDetaiTenanAktif()
        {
            return QryObjectMapper.Map<DetailTenanAktif>().ToList();
        }
        public IList<DetailTenanAktif> FindTenanAktifByName(string key)
        {
            string _key = "%" + key.ToLower() + "%";
            return QryObjectMapper.Map<DetailTenanAktif>("FindTenanAktifByName",
                new string[] { "key" }, new object[] { _key }).ToList();
        }
        public IList<DetailTenanAktif> FindDetailAkifInBandara(string locationid, string categoryid)
        {
            return QryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).ToList();
        }

        public IList<DetailTenanAktif> FindDetailAkifInTerminal(string locationid, string categoryid, string terminalid)
        {
            return QryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid,terminalid }).ToList();
        }

        public IList<DetailTenanAktif> FindDetailAkifInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid)
        {
            return QryObjectMapper.Map<DetailTenanAktif>("DetailTenanAktifInSubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminalid" },
                new object[4] { locationid, categoryid, terminalid, subterminalid }).ToList();
        }


        //tenan yang aktif kemarin
        public IList<DetailTenanAktifKemarin> FindDetaiTenanAktifKemarin()
        {
            return QryObjectMapper.Map<DetailTenanAktifKemarin>().ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInBandara(string locationid, string categoryid)
        {
            return QryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInBandara",
                new string[2] { "locationid", "categoryid" },
                new object[2] { locationid, categoryid }).ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInTerminal(string locationid, string categoryid, string terminalid)
        {
            return QryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInTerminal",
                new string[3] { "locationid", "categoryid", "terminalid" },
                new object[3] { locationid, categoryid, terminalid }).ToList();
        }

        public IList<DetailTenanAktifKemarin> FindDetailAkifKemarinInSubTerminal(string locationid, string categoryid, string terminalid, string subterminalid)
        {
            return QryObjectMapper.Map<DetailTenanAktifKemarin>("DetailTenanAktifKemarinInSubTerminal",
                new string[4] { "locationid", "categoryid", "terminalid", "subterminal" },
                new object[4] { locationid, categoryid, terminalid, subterminalid }).ToList();
        }
    }
}