using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonaStoco.AP1.Web.Models;
namespace BonaStoco.AP1.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize(Roles = APRoles.AP_ROLES)]
        public ActionResult RoleMaper()
        {
            RoleMaperSettingModel roleMapSetting = new RoleMaperSettingModel();
            roleMapSetting.Category = new Category { Id = 4, Name = "AP2 Tenant" };
            roleMapSetting.Location = new Location(0, "not available");
            roleMapSetting.Terminal = new Terminal(0, "<Not Available>");
            roleMapSetting.SubTerminal = new SubTerminal(0, "<Not Available>");
            roleMapSetting.Role = new UserRole(0, "User");
            
            roleMapSetting.Categories = new List<Category>
            {
                new Category { Id = 3, Name = "AP1 Tenant" },
                new Category { Id = 4, Name = "AP2 Tenant" }
            };
            
            roleMapSetting.Locations = new List<Location>
            {
                new Location(0, "not available"),
                new Location(1, "Soekarno Hatta Jakarta"),
                new Location(2, "Ngurah Rai Bali"),
                new Location(3, "Juanda Surabaya"),
                new Location(4, "Sultan Hasanuddin Makasar"),
                new Location(5, "Supadi Pontianak"),
                new Location(6, "Adisucipto Yogyakarta")
            };

            roleMapSetting.Terminals = new List<Terminal>
            {
                new Terminal(0, "<Not Available>"),
                new Terminal(1, "Terminal 1"),
                new Terminal(2, "Terminal 2"),
                new Terminal(3, "Terminal 3"),
                new Terminal(4, "Cargo")
            };

            roleMapSetting.SubTerminals = new List<SubTerminal>
            {
                new SubTerminal(0, "<Not Available>"),
                new SubTerminal(1, "Terminal 1A"),
                new SubTerminal(2, "Terminal 1B"),
                new SubTerminal(3, "Terminal 1C"),
                new SubTerminal(4, "Terminal 2D"),
                new SubTerminal(5, "Terminal 2E"),
                new SubTerminal(6, "Terminal 2F"),
                new SubTerminal(7, "Domestik"),
                new SubTerminal(8, "International"),
                new SubTerminal(9, "RA 1"),
                new SubTerminal(10, "RA 2"),
                new SubTerminal(11, "RA 3")
            };

            roleMapSetting.Roles = new List<UserRole>
            {
                new UserRole(0, "User"),
                new UserRole(1, "Administrator"),
                new UserRole(2, "Supervisor")
            };

            return View(roleMapSetting);
        }

        public ActionResult MapRole()
        {
            try
            {
                var catetoryId = int.Parse(Request.Form["Category"]);
                var locationId = int.Parse(Request.Form["Location"]);
                var terminalId = int.Parse(Request.Form["Terminal"]);
                var subTerminalId = int.Parse(Request.Form["SubTerminal"]);
                var roleId = int.Parse(Request.Form["Role"]);

                AP1Entities e = new AP1Entities();
                RoleMapper role = e.Roles.Where(r => r.CategoryId == catetoryId &&
                                                       r.BandaraId == locationId &&
                                                       r.TerminalId == terminalId &&
                                                       r.SubTerminalId == subTerminalId &&
                                                       r.Role == roleId).FirstOrDefault();
                if (role == null)
                {
                    string roleName = GetRoleName(catetoryId, roleId);
                    role = new RoleMapper(catetoryId, locationId, terminalId, subTerminalId, roleId, roleName, "TenantPage");
                    e.Roles.Add(role);
                }
                else
                {
                    role.CategoryId = catetoryId;
                    role.BandaraId = locationId;
                    role.TerminalId = terminalId;
                    role.SubTerminalId = subTerminalId;
                    role.Name = GetRoleName(catetoryId, roleId);
                    role.HomePage = "TenantPage";
                }

                e.SaveChanges();
                ViewBag.HasError = false;
            }
            catch (Exception ex)
            {
                ViewBag.HasError = true;
                ViewBag.ErrorMessage = ex.Message;
            }

            return View("MapRoleResult");
        }

        private string GetRoleName(int catId, int roleId)
        {
            if (catId == 4 && roleId == 0)
                return APRoles.TENANT_AP2_USER;
            else if (catId == 4 && roleId == 1)
                return APRoles.TENANT_AP2_ADMINISTRATOR;
            else if (catId == 4 && roleId == 2)
                return APRoles.TENANT_AP2_SUPERVISOR;
            else if (catId == 3 && roleId == 0)
                return APRoles.TENANT_AP1_USER;
            else if (catId == 3 && roleId == 1)
                return APRoles.TENANT_AP1_ADMINISTRATOR;
            else if (catId == 3 && roleId == 2)
                return APRoles.TENANT_AP1_SUPERVISOR;
            else
                return "";
        }
    }
}