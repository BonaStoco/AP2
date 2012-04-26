using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.Generic;
using Spring.Context.Support;
using BonaStoco.AP1.MasterData.Models;

namespace BonaStoco.AP1.Web.ReportingRepository
{
    public class AP2CargoRepository : IAP2CargoRepository
    {
        AdoTemplate _AdoTemplate;

        public AP2CargoRepository()
        {
            _AdoTemplate = ContextRegistry.GetContext().GetObject("AdoTemplate") as AdoTemplate;
        }

        public void AddAirCraft(AirCraft data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into cargoaircraft(kode, nama, guid) values ('" + data.Kode + "', '" + data.Nama + "','" + Guid.NewGuid().ToString() + "')");
        }

        public void UpdateAirCraft(AirCraft data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "update cargoaircraft set kode='" + data.Kode + "', nama='" + data.Nama + "' where guid = '" + data.Guid + "'");
        }

        public void AddCustomer(Customer data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into cargocustomer(kode, nama, guid) values ('" + data.Kode + "', '" + data.Nama + "','" + Guid.NewGuid().ToString() + "')");
        }

        public void UpdateCustomer(Customer data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "update cargocustomer set kode='" + data.Kode + "', nama='" + data.Nama + "' where guid = '" + data.Guid + "'");
        }

        public void AddComodity(Comodity data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into cargocomodity(kode, nama, guid) values ('" + data.Kode + "', '" + data.Nama + "','" + Guid.NewGuid().ToString() + "')");
        }

        public void UpdateComodity(Comodity data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "update cargocomodity set kode='" + data.Kode + "', nama='" + data.Nama + "' where guid = '" + data.Guid + "'");
        }

        public void AddDestinetion(Destinetion data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "insert into cargodestinetion(kode, nama, guid) values ('" + data.Kode + "', '" + data.Nama + "','" + Guid.NewGuid().ToString() + "')");
        }

        public void UpdateDestinetion(Destinetion data)
        {
            _AdoTemplate.ExecuteNonQuery(System.Data.CommandType.Text, "update cargodestinetion set kode='" + data.Kode + "', nama='" + data.Nama + "' where guid = '" + data.Guid + "'");
        }
    }
}
