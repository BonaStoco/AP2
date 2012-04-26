using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.DataMapper.Impl;
using BonaStoco.Inf.Reporting;
using Spring.Data.Generic;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;
using BonaStoco.Inf.DataMapper;

namespace BonaStoco.AP1.Web.ReportingRepository.MappingPriceRepository
{
    public class MappingPriceRepository : IMappingPriceRepository
    {
        private IReportingRepository reportingRepository;
        private IQueryObjectMapper _qryObjectMapper { get; set; }
        private IReportingRepository ReportingRepository
        {
            get
            {
                if (reportingRepository == null)
                    reportingRepository = ContextRegistry.GetContext().GetObject("ReportingRepository") as IReportingRepository;
                return reportingRepository;
            }
        }

        public MappingPriceRepository()
        {
            _qryObjectMapper = ContextRegistry.GetContext().GetObject("QueryObjectMapper") as IQueryObjectMapper;
        }
        public void AddItem(MappingPrice addMappingPrice)
        {
            ReportingRepository.Save<MappingPrice>(addMappingPrice); 
        }
        public IList<MappingPriceList> GetMappingPriceByTenanId(int tenanId)
        {
            return _qryObjectMapper.Map<MappingPriceList>("GetMappingPriceByTenanId", new string[] { "tenanid" }, new object[] { tenanId }).ToList();
        }
        public void UpdateItem(MappingPrice updateMappingPrice)
        {
            ReportingRepository.Update<MappingPrice>(updateMappingPrice, new { updateMappingPrice.Id });
        }
        public MappingPriceList GetMappingByGuidId(string id)
        {
            return _qryObjectMapper.Map<MappingPriceList>("GetMappingByTenanIdAndProductId", new string[]{"id" },new object[]{id}).FirstOrDefault();
        }
        public void Delete(string id)
        {
            _qryObjectMapper.Map<MappingPriceList>("Delete", new string[] { "id" }, new object[] { id }).FirstOrDefault();
        }
    }
}
