using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.AP1.Web.Report;
using BonaStoco.AP1.Web.ReportingRepository;
using BonaStoco.AP1.TenantMonitoring;
using BonaStoco.AP1.TenantMonitoring.Repository;
using BonaStoco.Inf.Reporting;
using Newtonsoft.Json;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using BonaStoco.AP1.TenantMonitoring.Dto;

namespace BonaStoco.AP1.Web.Controllers
{
    public class MonitoringDashBoardController : Controller
    {
        //
        // GET: /MonitoringDashBoard/
        ITenanActivityMonitoringRepository tenanActivityMonitoringRepository = new TenanActivityMonitoringRepository();

        public ActionResult MonitoringDashBoard()
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            int bandara = (cp.Role.Category == 1) ? 2 : 1;
            string tahun = DateTime.Now.Year.ToString();
            string bulan = DateTime.Now.Month.ToString();
            setViewHeader(bulan, tahun, bandara);
            IList<TenanActivityMonitoring> monitoring = tenanActivityMonitoringRepository.tenanActivityMonitoring(bandara, GetPeriod(DateTime.Today.Month - 1), GetPeriod(DateTime.Today.Month));
            setViewHeader(bulan, tahun, bandara);
            if (monitoring == null)
            {
                return View("Index");
            }
            return View("MonitoringDashBoard", monitoring);
        }

        private void setViewHeader(string bulan, string tahun, int noBandara)
        {
            ViewBag.Bulan = bulan;
            ViewBag.Tahun = tahun;
            ViewBag.Bandara = noBandara;
        }

        public JsonResult MonitoringDashBoardByPeriode(int bandara)
        {
            CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
            IList<TenanActivityMonitoring> tenantMonitoring = tenanActivityMonitoringRepository.tenanActivityMonitoring(bandara, GetPeriod(DateTime.Today.Month - 1), GetPeriod(DateTime.Today.Month));
            List<ActiveMonitoring> activeMonitorings = new List<ActiveMonitoring>();
            foreach (var monitoring in tenantMonitoring)
            {
                ActiveMonitoring[] activityMonitoring = Newtonsoft.Json.JsonConvert.DeserializeObject<ActiveMonitoring[]>(monitoring.Data);
                foreach (var activity in activityMonitoring)
                {
                    activity.Year = monitoring.GetYear();
                    activity.Month = monitoring.GetMonth();
                }
                activeMonitorings.AddRange(activityMonitoring);
            }
            DateTime firstDay = DateTime.Today.AddDays(DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month) * -1);
            var activeMonitoringByRange = from m in activeMonitorings
                                          where m.Date >= firstDay && m.Date <= DateTime.Today
                                          select m;
            return Json(activeMonitoringByRange, JsonRequestBehavior.AllowGet);
        }

        private string GetPeriod(int month)
        {
            DateTime toDay = DateTime.Today;
            int tahun = toDay.Year;
            int bulan = month;
            string period = string.Format("{0}{1}", tahun, bulan.ToString().PadLeft(2,'0'));
            return period;
        }

        public JsonResult FindBandara(string id)
        {
            BandaraAdvanceSearch bandara = MasterDataRepository().FindTenanActivityByIdBandara(Int32.Parse(id));
            if (bandara == null)
            {
                return Json("Bandara Tidak Di Temmukan", JsonRequestBehavior.AllowGet);
            }
            return Json(bandara.NameBandara, JsonRequestBehavior.AllowGet);
        }

        private IMasterDataRepository MasterDataRepository()
        {
            return (IMasterDataRepository)ContextRegistry.
                GetContext().GetObject("MasterDataRepository");
        }

        #region Dashboard Home
        public JsonResult GetTenanAktifHome(string date, int bandara)
        {
            DateTime _date = DateTime.Parse(date);
            var loadedData = ReportingRepository().GetByExample<TenantActivityMonitoring>(new { Code = _date.ToString("yyyyMMdd"), Bandara = bandara }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD = JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            if (amSD == null) return null;
          IList<TenanMonitoring> data=new List<TenanMonitoring>();
            if (amSD.Actives.Length != 0)
            {
                string sql = "select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan {0}";
                StringBuilder str = new StringBuilder();

                foreach (var activity in amSD.Actives)
                {
                    str.AppendFormat("{0},", activity);
                }
                if (str.Length > 0)
                {
                    str = str.Remove(str.Length - 1, 1);
                    str.Insert(0, "where tenanid in (");
                    str.Append(")");
                }

                QueryObjectMapper mapper = (QueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
                data = mapper.Map<TenanMonitoring>(String.Format(sql, str.ToString()));
            }
            else
            {
                data = null;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenanNonAktifHome(string date, int bandara)
        {
            DateTime _date = DateTime.Parse(date);
            var loadedData = ReportingRepository().GetByExample<TenantActivityMonitoring>(new { Code = _date.ToString("yyyyMMdd"), Bandara = bandara }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD = JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            if (amSD.NonActives == null) return null;
            IList<TenanMonitoring> data = new List<TenanMonitoring>();
            if (amSD.NonActives.Length != 0)
            {
                string sql = "select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan {0}";
                StringBuilder str = new StringBuilder();
                foreach (var nonActivity in amSD.NonActives)
                {
                    str.AppendFormat("{0},", nonActivity);
                }
                if (str.Length > 0)
                {
                    str = str.Remove(str.Length - 1, 1);
                    str.Insert(0, "where tenanid in (");
                    str.Append(")");
                }

                QueryObjectMapper mapper = (QueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
                data = mapper.Map<TenanMonitoring>(String.Format(sql, str.ToString()));
            }
            else
            {
                data = null;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenanActiveNonTransactionHome(string date, int bandara)
        {
            DateTime _date = DateTime.Parse(date);
            var loadedData = ReportingRepository().GetByExample<TenantActivityMonitoring>(new { Code = _date.ToString("yyyyMMdd"), Bandara = bandara }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD = JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            if (amSD.ActiveNonTransactions == null) return null;
             IList<TenanMonitoring> data = new List<TenanMonitoring>();
            
            if (amSD.ActiveNonTransactions.Length != 0)
            {
                string sql = "select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan {0}";
                StringBuilder str = new StringBuilder();
                foreach (var activeNonTransaction in amSD.ActiveNonTransactions)
                {
                    str.AppendFormat("{0},", activeNonTransaction);
                }
                if (str.Length > 0)
                {
                    str = str.Remove(str.Length - 1, 1);
                    str.Insert(0, "where tenanid in (");
                    str.Append(")");
                }

                QueryObjectMapper mapper = (QueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
               data = mapper.Map<TenanMonitoring>(String.Format(sql, str.ToString()));
            }
            else
            {
                data = null;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Monitoring
        public IList<TenanMonitoring> GetTenanAktif(int hari, int bulan, int tahun, int bandara)
        {
            DateTime date = new DateTime(tahun, bulan, hari);
            var loadedData = ReportingRepository().GetByExample<TenantActivityMonitoring>(new { Code = date.ToString("yyyyMMdd"), Bandara = bandara }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD = JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            string sql = "select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan {0}";
            StringBuilder str = new StringBuilder();
            foreach (var activity in amSD.Actives)
            {
                str.AppendFormat("{0},", activity);
            }
            if (str.Length > 0)
            {
                str = str.Remove(str.Length - 1, 1);
                str.Insert(0, "where tenanid in (");
                str.Append(")");
            }

            QueryObjectMapper mapper = (QueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
            IList<TenanMonitoring> data = mapper.Map<TenanMonitoring>(String.Format(sql, str.ToString()));
            return data;
        }

        public IList<TenanMonitoring> GetTenanNonAktif(int hari, int bulan, int tahun, int bandara)
        {
            DateTime date = new DateTime(tahun, bulan, hari);
            var loadedData = ReportingRepository().GetByExample<TenantActivityMonitoring>(new { Code = date.ToString("yyyyMMdd"), Bandara = bandara }).FirstOrDefault();
            if (loadedData == null)
                return null;
            var amSD = JsonConvert.DeserializeObject<ActivityMonitorSD>(loadedData.Data);
            string sql = "select tenanid as tenanid, tenanname as tenanname, alamat as alamat, gate as gate from tenan {0}";
            StringBuilder str = new StringBuilder();
            foreach (var nonActivity in amSD.NonActives)
            {
                str.AppendFormat("{0},", nonActivity);
            }
            if (str.Length > 0)
            {
                str = str.Remove(str.Length - 1, 1);
                str.Insert(0, "where tenanid in (");
                str.Append(")");
            }

            QueryObjectMapper mapper = (QueryObjectMapper)ContextRegistry.GetContext().GetObject("QueryObjectMapper");
            IList<TenanMonitoring> data = mapper.Map<TenanMonitoring>(String.Format(sql, str.ToString()));

            return data;
        }
        #endregion
        public JsonResult DetailTenanAktif(int hari, int bulan, int tahun, int bandara)
        {
            return Json(GetTenanAktif(hari, bulan, tahun, bandara), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailTenanNonAktif(int hari, int bulan, int tahun, int bandara)
        {

            return Json(GetTenanNonAktif(hari, bulan, tahun, bandara), JsonRequestBehavior.AllowGet);
        }

        public FileContentResult ExportTenanAktif(string hari, string bulan, string tahun, string bandara)
        {
            IList<TenanMonitoring> tenanAktif = GetTenanAktif(int.Parse(hari), int.Parse(bulan), int.Parse(tahun), int.Parse(bandara));
            string csv = "Kode Tenan,Nama Tenan,Alamat\r\n";
            foreach (TenanMonitoring tenan in tenanAktif)
            {
                if (tenan.Alamat.Contains("\r\n"))
                    tenan.Alamat = tenan.Alamat.Replace("\r\n", "-");
                if (tenan.Alamat.Contains(','))
                    tenan.Alamat = tenan.Alamat.Replace(',', ' ');
                if (tenan.TenanName.Contains(','))
                    tenan.TenanName = tenan.TenanName.Replace(',', ' ');

                csv += tenan.TenanId.ToString() + ',';
                csv += tenan.TenanName + ',';
                csv += tenan.Alamat + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "List Tenan Aktif (" + hari + "-" + bulan + "-" + tahun + ".csv");
        }
        public FileContentResult ExportTenanNonAktif(string hari, string bulan, string tahun, string bandara)
        {
            IList<TenanMonitoring> tenanNonAktif = GetTenanNonAktif(int.Parse(hari), int.Parse(bulan), int.Parse(tahun), int.Parse(bandara));
            string csv = "Kode Tenan,Nama Tenan,Alamat\r\n";
            foreach (TenanMonitoring tenan in tenanNonAktif)
            {
                if (tenan.Alamat.Contains("\r\n"))
                    tenan.Alamat = tenan.Alamat.Replace("\r\n", "-");
                if (tenan.Alamat.Contains(','))
                    tenan.Alamat = tenan.Alamat.Replace(',', ' ');
                if (tenan.TenanName.Contains(','))
                    tenan.TenanName = tenan.TenanName.Replace(',', ' ');

                csv += tenan.TenanId.ToString() + ',';
                csv += tenan.TenanName + ',';
                csv += tenan.Alamat + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "List Tenan Tidak Aktif (" + hari + "-" + bulan + "-" + tahun + ".csv");
        }
        public IReportingRepository ReportingRepository()
        {
            return ContextRegistry.GetContext().GetObject("ReportingRepository") as IReportingRepository;
        }
    }
}
