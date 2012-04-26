using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public interface IAP2CargoRepository
    {
        void AddAirCraft(AirCraft data);
        void UpdateAirCraft(AirCraft data);
        void AddCustomer(Customer data);
        void UpdateCustomer(Customer data);
        void AddComodity(Comodity data);
        void UpdateComodity(Comodity data);
        void AddDestinetion(Destinetion data);
        void UpdateDestinetion(Destinetion data);
    }
}
