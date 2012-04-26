using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Security.Principal;
using System.Text;
namespace BonaStoco.AP1.Web.Models
{
    public class APRoles
    {
        #region Roles definision

        public const string GUEST = "Guest";

        public const string TENANT_AP1_ADMINISTRATOR = "TenantAP1Administrator";
        public const string TENANT_AP1_SUPERVISOR = "TenantAP1Supervisor";
        public const string TENANT_AP1_USER = "TenantAP1User";

        public const string TENANT_AP2_ADMINISTRATOR = "TenantAP2Administrator";
        public const string TENANT_AP2_SUPERVISOR = "TenantAP2Supervisor";
        public const string TENANT_AP2_USER = "TenantAP2User";

        public const string AP1_ADMINISTRATOR = "AP1Administrator";
        public const string AP1_SUPERVISOR = "AP1Supervisor";
        public const string AP1_USER = "AP1User";

        public const string AP2_ADMINISTRATOR = "AP2Administrator";
        public const string AP2_SUPERVISOR = "AP2Supervisor";
        public const string AP2_USER = "AP2User";

        public const string AP1_BANDARA_ADMINISTRATOR = "AP1BandaraAdministrator";
        public const string AP1_BANDARA_SUPERVISOR = "AP1BandaraSupervisor";
        public const string AP1_BANDARA_USER = "AP1BandaraUser";

        public const string AP2_BANDARA_ADMINISTRATOR = "AP2BandaraAdministrator";
        public const string AP2_BANDARA_SUPERVISOR = "AP2BandaraSupervisor";
        public const string AP2_BANDARA_USER = "AP2BandaraUser";

        public const string AP1_TERMINAL_ADMINISTRATOR = "AP1TerminalAdministrator";
        public const string AP1_TERMINAL_SUPERVISOR = "AP1TerminalSupervisor";
        public const string AP1_TERMINAL_USER = "AP1TerminalUser";

        public const string AP2_TERMINAL_ADMINISTRATOR = "AP2TerminalAdministrator";
        public const string AP2_TERMINAL_SUPERVISOR = "AP2TerminalSupervisor";
        public const string AP2_TERMINAL_USER = "AP2TerminalUser";

        public const string AP1_SUBTERMINAL_ADMINISTRATOR = "AP1SubTerminalAdministrator";
        public const string AP1_SUBTERMINAL_SUPERVISOR = "AP1SubTerminalSupervisor";
        public const string AP1_SUBTERMINAL_USER = "AP1SubTerminalUser";

        public const string AP2_SUBTERMINAL_ADMINISTRATOR = "AP2SubTerminalAdministrator";
        public const string AP2_SUBTERMINAL_SUPERVISOR = "AP2SubTerminalSupervisor";
        public const string AP2_SUBTERMINAL_USER = "AP2SubTerminalUser";

        public const string TENANT_Umum_ADMINISTRATOR = "TenantUmumAdministrator";
        public const string TENANT_Umum_SUPERVISOR = "TenantUmumSupervisor";
        public const string TENANT_Umum_USER = "TenantUmumUser";

        public const string Umum_ADMINISTRATOR = "UmumAdministrator";
        public const string Umum_SUPERVISOR = "UmumSupervisor";
        public const string Umum_USER = "UmumUser";

        public const string TELKOM_USER = "TelkomUser";

        #endregion

        private static IList<string> _allRoles;
        
        public static IList<string> AllRoles
        {
            get
            {
                if (_allRoles == null)
                    InitAllRoles();
                return _allRoles;
            }
        }
        
        private static void InitAllRoles()
        {
            _allRoles = new List<String>()
            {
                GUEST,
                TENANT_AP1_ADMINISTRATOR,
                TENANT_AP1_SUPERVISOR,
                TENANT_AP1_USER,
                TENANT_AP2_ADMINISTRATOR,
                TENANT_AP2_SUPERVISOR,
                TENANT_AP2_USER,
                TENANT_Umum_ADMINISTRATOR,
                TENANT_Umum_SUPERVISOR,
                TENANT_Umum_USER,
                AP1_ADMINISTRATOR,
                AP1_SUPERVISOR,
                AP1_USER,
                AP2_ADMINISTRATOR,
                AP2_SUPERVISOR,
                AP2_USER,
                AP1_BANDARA_ADMINISTRATOR,
                AP1_BANDARA_SUPERVISOR,
                AP1_BANDARA_USER,
                AP2_BANDARA_ADMINISTRATOR,
                AP2_BANDARA_SUPERVISOR,
                AP2_BANDARA_USER,
                AP1_TERMINAL_ADMINISTRATOR,
                AP1_TERMINAL_SUPERVISOR,
                AP1_TERMINAL_USER,
                AP2_TERMINAL_ADMINISTRATOR,
                AP2_TERMINAL_SUPERVISOR,
                AP2_TERMINAL_USER,
                AP1_SUBTERMINAL_ADMINISTRATOR,
                AP1_SUBTERMINAL_SUPERVISOR,
                AP1_SUBTERMINAL_USER,
                AP2_SUBTERMINAL_ADMINISTRATOR,
                AP2_SUBTERMINAL_SUPERVISOR,
                AP2_SUBTERMINAL_USER,
                TELKOM_USER,
                Umum_ADMINISTRATOR,
                Umum_SUPERVISOR,
                Umum_USER
            };
        }

        public static string MapRoleName(RoleId role)
        {
            AP1Entities e = new AP1Entities();
            RoleMapper result = e.Roles.Where(r => r.CategoryId == role.Category &&
                                                   r.BandaraId == role.Bandara &&
                                                   r.TerminalId == role.Terminal &&
                                                   r.SubTerminalId == role.SubTerminal &&
                                                   r.Role == role.Role).FirstOrDefault();
            if (result == null) return GUEST;
            return result.Name;
        }
        public const string AP1_ROLES = "AP1Administrator," +
                                        "AP1User," +
                                        "AP1Supervisor," +
                                        "AP1BandaraAdministrator," +
                                        "AP1BandaraUser," +
                                        "AP1BandaraSupervisor," +
                                        "AP1TerminalAdministrator," +
                                        "AP1TerminalUser," +
                                        "AP1TerminalSupervisor," +
                                        "AP1SubTerminalAdministrator," +
                                        "AP1SubTerminalUser," +
                                        "AP1SubTerminalSupervisor";
        public const string AP2_ROLES = "AP2Administrator," +
                                        "AP2User," +
                                        "AP2Supervisor," +
                                        "AP2BandaraAdministrator," +
                                        "AP2BandaraUser," +
                                        "AP2BandaraSupervisor," +
                                        "AP2TerminalAdministrator," +
                                        "AP2TerminalUser," +
                                        "AP2TerminalSupervisor," +
                                        "AP2SubTerminalAdministrator," +
                                        "AP2SubTerminalUser," +
                                        "AP2SubTerminalSupervisor";
        public const string Umum_ROLES = "UmumAdministrator," +
                                         "UmumUser," +
                                         "UmumSupervisor";
        public const string AP_ROLES =  "AP1Administrator," +
                                        "AP1User," +
                                        "AP1Supervisor," +
                                        "AP1BandaraAdministrator," +
                                        "AP1BandaraUser," +
                                        "AP1BandaraSupervisor," +
                                        "AP1TerminalAdministrator," +
                                        "AP1TerminalUser," +
                                        "AP1TerminalSupervisor," +
                                        "AP1SubTerminalAdministrator," +
                                        "AP1SubTerminalUser," +
                                        "AP1SubTerminalSupervisor," +
                                        "AP2Administrator," +
                                        "AP2User," +
                                        "AP2Supervisor," +
                                        "AP2BandaraAdministrator," +
                                        "AP2BandaraUser," +
                                        "AP2BandaraSupervisor," +
                                        "AP2TerminalAdministrator," +
                                        "AP2TerminalUser," +
                                        "AP2TerminalSupervisor," +
                                        "AP2SubTerminalAdministrator," +
                                        "AP2SubTerminalUser," +
                                        "AP2SubTerminalSupervisor" +
                                        "UmumAdministrator," +
                                        "UmumUser," +
                                        "UmumSupervisor";

        public const string TENANT_ROLES = "TenantAP1Administrator," +
                                           "TenantAP1User," +
                                           "TenantAP1Supervisor," +
                                           "TenantAP2Administrator," +
                                           "TenantAP2User," +
                                           "TenantAP2Supervisor," +
                                           "TenantUmumAdministrator," +
                                           "TenantUmumUser," +
                                           "TenantUmumSupervisor";

        public const string TELKOM_ROLES = "TelkomUser";

        public static string MapHomePage(RoleId role)
        {
            if (role == null)
                return "Guest";

            AP1Entities e = new AP1Entities();
            RoleMapper result = e.Roles.Where(r => r.CategoryId == role.Category &&
                                                   r.BandaraId == role.Bandara &&
                                                   r.TerminalId == role.Terminal &&
                                                   r.SubTerminalId == role.SubTerminal &&
                                                   r.Role == role.Role).FirstOrDefault();
            if (result == null)
                return GUEST;

            return result.HomePage;
        }
        public static string MapLayout(string homePage)
        {
            if (homePage == "AP1Page")
                return "~/Views/Shared/_LayoutAP1.cshtml";
            else if (homePage == "AP1BandaraPage")
                return "~/Views/Shared/_LayoutAP1Bandara.cshtml";
            else if (homePage == "AP2Page")
                return "~/Views/Shared/_LayoutAP2.cshtml";
            else if (homePage == "AP2BandaraPage")
                return "~/Views/Shared/_LayoutAP2Bandara.cshtml";
            else if (homePage == "AP2TerminalPage")
                return "~/Views/Shared/_LayoutAP2Terminal.cshtml";
            else if (homePage == "AP2CargoPage")
                return "~/Views/Shared/_LayoutAP2Cargo.cshtml";
            else if (homePage == "AP2SubTerminalPage")
                return "~/Views/Shared/_LayoutAP2SubTerminal.cshtml";
            else if (homePage == "TenantPage")
                return "~/Views/Shared/_LayoutTenant.cshtml";
            else if (homePage == "TelkomPage")
                return "~/Views/Shared/_LayoutTelkom.cshtml";
            else if (homePage == "AP1SubTerminalPage")
                return "~/Views/Shared/_LayoutAP1Bandara.cshtml";
            else if(homePage == "UmumPage")
                return "~/Views/Shared/_LayoutUmum.cshtml";

            return "~/Views/Shared/_LayoutHome.cshtml";
        }
        public static bool IsRoot(string roleName)
        {
            return roleName == AP1_ADMINISTRATOR ||
                   roleName == AP1_SUPERVISOR ||
                   roleName == AP1_USER || 
                   roleName == AP2_ADMINISTRATOR ||
                   roleName == AP2_SUPERVISOR ||
                   roleName == AP2_USER ||
                   roleName == TELKOM_USER ||
                   roleName == Umum_ADMINISTRATOR ||
                   roleName == Umum_SUPERVISOR ||
                   roleName == Umum_USER;
        }


        public static bool IsBandara(string roleName)
        {
            return roleName.Contains("Bandara");
        }

        public static bool IsTerminal(string roleName)
        {
            return roleName.Contains("Terminal") && !roleName.Contains("SubTerminal");
        }

        public static bool IsSubTerminal(string roleName)
        {
            return roleName.Contains("SubTerminal");
        }
    }
}