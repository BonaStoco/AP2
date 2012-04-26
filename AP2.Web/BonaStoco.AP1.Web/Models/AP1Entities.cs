using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace BonaStoco.AP1.Web.Models
{
    public class AP1Entities : DbContext
    {
        public DbSet<GRNItemModel> GRNItem { get; set; }
        //public DbSet<NewProductsModel> NewProduct { get; set; }
        public DbSet<GRNAutoNumbering> GRNAutoNumbering { get; set; }
        public DbSet<RoleMapper> Roles { get; set; }
    }

    public class SampleData : System.Data.Entity.DropCreateDatabaseIfModelChanges<AP1Entities>
    {
        protected override void Seed(AP1Entities context)
        {
            /*
            new List<GRNItemModel>
            {
                new GRNItemModel
                {
                    NamaBarang = "Test",
                    Qty = 10,
                    Harga = 1000,
                    Jumlah = 10000,
                    ProductId = 1,
                    UserId = "oetawan@inforsys.co.id"
                }
            }.ForEach(m => context.GRNItem.Add(m));

            new List<GRNAutoNumbering>
            {
                new GRNAutoNumbering() { Year = 2011, Month = 7, Index = 0, TenantId = 2240 }
            }.ForEach(e => context.GRNAutoNumbering.Add(e));
            */

            new List<RoleMapper>
            {
                new RoleMapper(2,0,0,0,1,APRoles.AP2_ADMINISTRATOR,"AP2Page"),
                new RoleMapper(2,0,0,0,0,APRoles.AP2_USER,"AP2Page"),
                new RoleMapper(2,0,0,0,2,APRoles.AP2_SUPERVISOR,"AP2Page"),

                new RoleMapper(2,1,0,0,1,APRoles.AP2_BANDARA_ADMINISTRATOR,"AP2BandaraPage"),
                new RoleMapper(2,1,0,0,0,APRoles.AP2_BANDARA_USER,"AP2BandaraPage"),
                new RoleMapper(2,1,0,0,2,APRoles.AP2_BANDARA_SUPERVISOR,"AP2BandaraPage"),

                new RoleMapper(2,1,1,0,1,APRoles.AP2_TERMINAL_ADMINISTRATOR,"AP2TerminalPage"),
                new RoleMapper(2,1,1,0,0,APRoles.AP2_TERMINAL_USER,"AP2TerminalPage"),
                new RoleMapper(2,1,1,0,2,APRoles.AP2_TERMINAL_SUPERVISOR,"AP2TerminalPage"),

                new RoleMapper(2,1,2,0,1,APRoles.AP2_TERMINAL_ADMINISTRATOR,"AP2TerminalPage"),
                new RoleMapper(2,1,2,0,0,APRoles.AP2_TERMINAL_USER,"AP2TerminalPage"),
                new RoleMapper(2,1,2,0,2,APRoles.AP2_TERMINAL_SUPERVISOR,"AP2TerminalPage"),

                new RoleMapper(2,1,3,0,1,APRoles.AP2_TERMINAL_ADMINISTRATOR,"AP2TerminalPage"),
                new RoleMapper(2,1,3,0,0,APRoles.AP2_TERMINAL_USER,"AP2TerminalPage"),
                new RoleMapper(2,1,3,0,2,APRoles.AP2_TERMINAL_SUPERVISOR,"AP2TerminalPage"),

                new RoleMapper(2,1,4,0,1,APRoles.AP2_TERMINAL_ADMINISTRATOR,"AP2CargoPage"),
                new RoleMapper(2,1,4,0,0,APRoles.AP2_TERMINAL_USER,"AP2CargoPage"),
                new RoleMapper(2,1,4,0,2,APRoles.AP2_TERMINAL_SUPERVISOR,"AP2CargoPage"),

                new RoleMapper(2,1,1,1,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,1,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,1,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,1,2,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,2,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,2,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,1,3,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,3,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,1,3,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,2,4,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,4,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,4,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,2,5,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,5,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,5,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,2,6,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,6,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,2,6,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,4,9,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,9,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,9,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,4,10,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,10,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,10,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(2,1,4,11,1,APRoles.AP2_SUBTERMINAL_ADMINISTRATOR,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,11,0,APRoles.AP2_SUBTERMINAL_USER,"AP2SubTerminalPage"),
                new RoleMapper(2,1,4,11,2,APRoles.AP2_SUBTERMINAL_SUPERVISOR,"AP2SubTerminalPage"),

                new RoleMapper(1,0,0,0,1,APRoles.AP1_ADMINISTRATOR,"AP1Page"),
                new RoleMapper(1,0,0,0,0,APRoles.AP1_USER,"AP1Page"),
                new RoleMapper(1,0,0,0,2,APRoles.AP1_SUPERVISOR,"AP1Page"),

                new RoleMapper(1,2,0,0,1,APRoles.AP1_BANDARA_ADMINISTRATOR,"AP1BandaraPage"),
                new RoleMapper(1,2,0,0,0,APRoles.AP1_BANDARA_USER,"AP1BandaraPage"),
                new RoleMapper(1,2,0,0,2,APRoles.AP1_BANDARA_SUPERVISOR,"AP1BandaraPage"),

                new RoleMapper(1,3,0,0,1,APRoles.AP1_BANDARA_ADMINISTRATOR,"AP1BandaraPage"),
                new RoleMapper(1,3,0,0,0,APRoles.AP1_BANDARA_USER,"AP1BandaraPage"),
                new RoleMapper(1,3,0,0,2,APRoles.AP1_BANDARA_SUPERVISOR,"AP1BandaraPage"),

                new RoleMapper(1,4,0,0,1,APRoles.AP1_BANDARA_ADMINISTRATOR,"AP1BandaraPage"),
                new RoleMapper(1,4,0,0,0,APRoles.AP1_BANDARA_USER,"AP1BandaraPage"),
                new RoleMapper(1,4,0,0,2,APRoles.AP1_BANDARA_SUPERVISOR,"AP1Page"),

                new RoleMapper(1,5,0,0,1,APRoles.AP1_BANDARA_ADMINISTRATOR,"AP2BandaraPage"),
                new RoleMapper(1,5,0,0,0,APRoles.AP1_BANDARA_USER,"AP2BandaraPage"),
                new RoleMapper(1,5,0,0,2,APRoles.AP1_BANDARA_SUPERVISOR,"AP2BandaraPage"),

                new RoleMapper(1,6,0,0,1,APRoles.AP1_BANDARA_ADMINISTRATOR,"AP2BandaraPage"),
                new RoleMapper(1,6,0,0,0,APRoles.AP1_BANDARA_USER,"AP2BandaraPage"),
                new RoleMapper(1,6,0,0,2,APRoles.AP1_BANDARA_SUPERVISOR,"AP2BandaraPage"),

                new RoleMapper(3,0,0,0,1,APRoles.TENANT_AP1_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(3,0,0,0,0,APRoles.TENANT_AP1_USER, "TenantPage"),
                new RoleMapper(3,0,0,0,2,APRoles.TENANT_AP1_SUPERVISOR, "TenantPage"),

                new RoleMapper(3,2,0,0,1,APRoles.TENANT_AP1_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(3,2,0,0,0,APRoles.TENANT_AP1_USER, "TenantPage"),
                new RoleMapper(3,2,0,0,2,APRoles.TENANT_AP1_SUPERVISOR, "TenantPage"),

                new RoleMapper(3,2,1,7,1,APRoles.TENANT_AP1_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(3,2,1,7,0,APRoles.TENANT_AP1_USER, "TenantPage"),
                new RoleMapper(3,2,1,7,2,APRoles.TENANT_AP1_SUPERVISOR, "TenantPage"),
				
				new RoleMapper(3,2,1,8,1,APRoles.TENANT_AP1_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(3,2,1,8,0,APRoles.TENANT_AP1_USER, "TenantPage"),
                new RoleMapper(3,2,1,8,2,APRoles.TENANT_AP1_SUPERVISOR, "TenantPage"),

                new RoleMapper(4,0,0,0,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,0,0,0,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,0,0,0,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(4,1,4,9,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,1,4,9,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,1,4,9,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(4,1,4,10,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,1,4,10,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,1,4,10,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(4,1,4,11,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,1,4,11,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,1,4,11,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(1,2,1,7,1,APRoles.AP1_SUBTERMINAL_ADMINISTRATOR,"AP1SubTerminalPage"),
                new RoleMapper(1,2,1,7,0,APRoles.AP1_SUBTERMINAL_USER,"AP1SubTerminalPage"),
                new RoleMapper(1,2,1,7,2,APRoles.AP1_SUBTERMINAL_SUPERVISOR,"AP1SubTerminalPage"),

                new RoleMapper(1,2,1,8,1,APRoles.AP1_SUBTERMINAL_ADMINISTRATOR,"AP1SubTerminalPage"),
                new RoleMapper(1,2,1,8,0,APRoles.AP1_SUBTERMINAL_USER,"AP1SubTerminalPage"),
                new RoleMapper(1,2,1,8,2,APRoles.AP1_SUBTERMINAL_SUPERVISOR,"AP1SubTerminalPage"),

                new RoleMapper(4,1,1,1,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,1,1,1,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,1,1,1,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(4,1,1,2,1,APRoles.TENANT_AP2_ADMINISTRATOR, "TenantPage"),
                new RoleMapper(4,1,1,2,0,APRoles.TENANT_AP2_USER, "TenantPage"),
                new RoleMapper(4,1,1,2,2,APRoles.TENANT_AP2_SUPERVISOR, "TenantPage"),

                new RoleMapper(0,0,0,0,4,APRoles.TELKOM_USER,"TelkomPage"),

                new RoleMapper(1,2,46,0,1,APRoles.AP1_TERMINAL_ADMINISTRATOR,"AP1TerminalPage"),

                new RoleMapper(6,16,19,13,1,APRoles.TENANT_Umum_ADMINISTRATOR,"TenantPage"),
                new RoleMapper(6,16,19,13,0,APRoles.TENANT_Umum_USER,"TenantPage"),
                new RoleMapper(6,16,19,13,2,APRoles.TENANT_Umum_SUPERVISOR,"TenantPage"),

                new RoleMapper(5,16,19,13,1,APRoles.Umum_ADMINISTRATOR,"UmumPage"),
                new RoleMapper(5,16,19,13,0,APRoles.Umum_USER,"UmumPage"),
                new RoleMapper(5,16,19,13,2,APRoles.Umum_SUPERVISOR,"UmumPage")
            }.ForEach(e => context.Roles.Add(e));
        }
    }
}