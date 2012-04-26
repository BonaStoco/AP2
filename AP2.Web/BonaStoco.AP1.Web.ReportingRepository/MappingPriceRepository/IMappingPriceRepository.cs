using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository.MappingPriceRepository
{
    public interface IMappingPriceRepository
    {
        void AddItem(MappingPrice addMappingPrice);
        IList<MappingPriceList> GetMappingPriceByTenanId(int tenanId);
        MappingPriceList GetMappingByGuidId(string id);
        void UpdateItem(MappingPrice updateMappingPrice);
        void Delete(string id);
    }
}
