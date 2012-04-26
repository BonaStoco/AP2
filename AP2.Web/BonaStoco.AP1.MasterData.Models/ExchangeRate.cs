using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.Inf.Data.ViewModel;

namespace BonaStoco.AP1.MasterData.Models
{
    [NamedSqlQuery("FindRateUSD", "select eri.rate from exchangerateitem eri inner join exchangerate er on eri.modelguid=er.modelguid where @date >= er.startdate and @date <= er.enddate and eri.ccycode='USD' order by er.id desc limit 1")]

    public class ExchangeRate : IViewModel
    {
        public decimal Rate { get; set; }
    }
}