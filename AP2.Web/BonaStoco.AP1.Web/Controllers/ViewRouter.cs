using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BonaStoco.AP1.Web.Models;
namespace BonaStoco.AP1.Web.Controllers
{
    public class ControllerRouter
    {
        public static string RouteHome(string role)
        {
            string result = "Home";
            switch (role)
            {
                case APRoles.AP1_ADMINISTRATOR:
                case APRoles.AP1_USER:
                    result = "AP1Home";
                    break;
                case APRoles.AP2_ADMINISTRATOR:
                case APRoles.AP2_USER:  
                    result = "AP2Home";
                    break;
                case APRoles.TENANT_AP1_ADMINISTRATOR:
                case APRoles.TENANT_AP1_SUPERVISOR:
                case APRoles.TENANT_AP1_USER:
                case APRoles.TENANT_AP2_ADMINISTRATOR:
                case APRoles.TENANT_AP2_SUPERVISOR:
                case APRoles.TENANT_AP2_USER:
                    result = "TenantPage";
                    break;
                case APRoles.TELKOM_USER:
                    result = "TelkomPage";
                    break;
                default:
                    break;
            }
           

            return result;
        }
    }
}