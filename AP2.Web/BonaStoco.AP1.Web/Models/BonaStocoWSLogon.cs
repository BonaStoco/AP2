using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using BonaStoco.Inf.ExceptionUtils;
using System.Web.Security;
using BonaStoco.AP1.MasterData.Models;
using Spring.Context.Support;
using BonaStoco.Inf.ExceptionUtils;
using System.Collections.Specialized;
using System.Configuration;
namespace BonaStoco.AP1.Web.Models
{
    public class BonaStocoWSLogon
    {
        BonaStoco.AP1.Web.sercurityws.BonastocoServices bonaWS;
        bool isAuthenticated = false;
        string errorMessage = string.Empty;
        BonaStoco.AP1.Web.sercurityws.loginResponse loginResponse = null;
        bool cannotConnectToBonaStocoWS = true;
        string usr = string.Empty;
        string pwd = string.Empty;
        RoleId roleId=new RoleId(0,0,0,0,0);

        public BonaStocoWSLogon()
        {
            bonaWS = new BonaStoco.AP1.Web.sercurityws.BonastocoServices();
            bonaWS.Timeout = 600000;
        }

        public bool IsAuthenticated { get { return !cannotConnectToBonaStocoWS && isAuthenticated; } }
        public BonaStoco.AP1.Web.sercurityws.loginResponse Response { get { return loginResponse; } }
        public RoleId ROLE { get { return roleId; }}
        public string ErrorMessage { get { return errorMessage; } }
        
        public void Login(string username, string password)
        {
            try
            {
                this.usr = username;
                this.pwd = password;
                
                TestConnection();
                if (cannotConnectToBonaStocoWS)
                    return;

                this.loginResponse = bonaWS.logon(new BonaStoco.AP1.Web.sercurityws.Person() { email = MD5(username), password = MD5(password), code = Dns.GetHostName() });
                
                isAuthenticated = loginResponse.status == 0;
                errorMessage = loginResponse.message;
                if (!isAuthenticated)
                    return;
                
                CheckApplicationRoles();
                string virtualHost = ExtractVirtualHostFromLoginResponse(this.loginResponse);
                NameValueCollection settings = (NameValueCollection)ConfigurationManager.GetSection("UmumRoleSettings");
                if (isAuthenticated)  
                {
                    if (virtualHost.ToLower().Contains("umum"))
                        this.roleId = new RoleId(
                            int.Parse(settings["CategoryId"]),
                            int.Parse(settings["BandaraId"]),
                            int.Parse(settings["TerminalId"]),
                            int.Parse(settings["SubTerminalId"]),
                            int.Parse(settings["RoleId"]));
                    else
                        this.roleId = new RoleId(ExtractCategoryIdFromLoginResponse(this.loginResponse),
                                                 ExtractBandaraIdFromLoginResponse(this.loginResponse),
                                                 ExtractTerminalIdFromLoginResponse(this.loginResponse),
                                                 ExtractSubTerminalIdFromLoginResponse(this.loginResponse),
                                                 loginResponse.role);
                    CreateLocalUserIfNecessary();
                    CreateOrUpdateTenanIfNecessary();
                }
            }
            catch (WebException ex)
            {
                isAuthenticated = false;
                if (ex.Message.Contains("The underlying connection was closed: The connection was closed unexpectedly."))
                    errorMessage = "Koneksi ke internet tidak ada";
                else
                    errorMessage = ex.GetInnermostException().Message;
            }
            catch (Exception ex)
            {
                isAuthenticated = false;
                errorMessage = ex.GetInnermostException().Message;
            }
        }

        private void CheckApplicationRoles()
        {
           string appRoles = System.Configuration.ConfigurationManager.AppSettings["AppRoles"];
           string[] appRolesArray = appRoles.Split(',');
           int categoryId = ExtractCategoryIdFromLoginResponse(this.loginResponse);
           bool isInRole = appRolesArray.Contains(categoryId.ToString());
           if(!isInRole){
               isAuthenticated = false;
               loginResponse.message = "Invalid username or password";
               this.errorMessage = "Invalid username or password";
           }
        }

        private void CreateOrUpdateTenanIfNecessary()
        {
            IMasterDataRepository masterDataRepo = (IMasterDataRepository)
                ContextRegistry.GetContext().GetObject("MasterDataRepository");
            Tenan tenan = masterDataRepo.FindTenanById(loginResponse.companyid);
            if (tenan == null)
                new RabbitHelper().SendTenanCreatedMessage(
                    new Messages.TenanCreatedMessage
                    {
                        TenanId = loginResponse.companyid,
                        TenanName = loginResponse.company,
                        LocationId = roleId.Bandara,
                        TerminalId = roleId.Terminal,
                        SubTerminalId = roleId.SubTerminal,
                        CategoryId = roleId.Category,
                        Tarif = 10,
                        HeadOffice=int.Parse(loginResponse.headoffice)
                    }
                );
            else
            {
                bool isCategoryDirty = tenan.CategoryId != roleId.Category;
                bool isLocationDirty = tenan.LocationId != roleId.Bandara;
                bool isTerminalDirty = tenan.TerminalId != roleId.Terminal;
                bool isSubTerminalDirty = tenan.SubTerminalId != roleId.SubTerminal;

                if (isCategoryDirty ||
                   isLocationDirty ||
                   isTerminalDirty ||
                   isSubTerminalDirty)
                {
                    Messages.TenanEditedMessage msg = new Messages.TenanEditedMessage
                    {
                        TenanId = tenan.TenanId,
                        TenanName = tenan.TenanName,
                        Alamat = tenan.Alamat,
                        Nppkp = tenan.Nppkp,
                        Npwp = tenan.Npwp,
                        TanggalBergabung = tenan.TanggalBergabung,
                        Tarif = tenan.Tarif,
                        TenanTypeId = tenan.TenanTypeId,
                        ProductTypeId = tenan.ProductTypeId,
                        Gate = tenan.Gate,
                        CategoryId = roleId.Category,
                        LocationId = roleId.Bandara,
                        TerminalId = roleId.Terminal,
                        SubTerminalId = roleId.SubTerminal,
                        HeadOffice = tenan.HeadOffice
                    };
                    new RabbitHelper().SendTenanEditedMessage(msg);
                }
            }
        }

        private void TestConnection()
        {
            try
            {
                BonaStoco.AP1.Web.sercurityws.serverResponse respon = bonaWS.hello(new BonaStoco.AP1.Web.sercurityws.sayHello() { name = System.Net.Dns.GetHostName() });
                cannotConnectToBonaStocoWS = respon.status != 0;
                if (!respon.status.Equals(0))
                    errorMessage = respon.message;
            }
            catch (Exception ex)
            {
                cannotConnectToBonaStocoWS = true;
                errorMessage = "Koneksi internet ada. Tetapi, server BonaStoco sedang bermasalah atau akses internet anda sedang diblokir provider.\r\n\r\nError: " + ex.GetInnermostException().Message;
            }
        }
        public string MD5(string originalPassword)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-", "").ToLower();
        }
        public void CreateLocalUserIfNecessary()
        {
            if (loginResponse.status != 0) return;

            MembershipUser muser = Membership.GetUser(usr);
            if (muser == null)
            {
                Membership.CreateUser(usr, pwd);
                Roles.AddUserToRole(usr, APRoles.MapRoleName(roleId));
            }

            if (IsRoleChanged())
            {
                foreach (string role in APRoles.AllRoles)
                {
                    if (Roles.IsUserInRole(usr, role))
                        Roles.RemoveUserFromRole(usr, role);
                }
                Roles.AddUserToRole(usr, APRoles.MapRoleName(roleId));
            }
        }
        private bool IsRoleChanged()
        {
            string roleName = APRoles.MapRoleName(roleId);
            return !Roles.IsUserInRole(usr, roleName);
        }

        private int ExtractCategoryIdFromLoginResponse(BonaStoco.AP1.Web.sercurityws.loginResponse loginResponse)
        {
            IList<string> companyReserved = loginResponse.reserved.Split(';').ToList();
            string[] categoryArr = companyReserved.Where( c => c.Contains("categoryid") ).FirstOrDefault().Split('=');
            return Int32.Parse(categoryArr[1]);
        }
        private int ExtractBandaraIdFromLoginResponse(BonaStoco.AP1.Web.sercurityws.loginResponse loginResponse)
        {
            IList<string> companyReserved = loginResponse.reserved.Split(';').ToList();
            string[] bandaraArr = companyReserved.Where(c => c.Contains("locationid")).FirstOrDefault().Split('=');
            return Int32.Parse(bandaraArr[1]);
        }
        private int ExtractTerminalIdFromLoginResponse(BonaStoco.AP1.Web.sercurityws.loginResponse loginResponse)
        {
            IList<string> companyReserved = loginResponse.reserved.Split(';').ToList();
            string[] terminalArr = companyReserved.Where(c => c.Contains("portid")).FirstOrDefault().Split('=');
            return Int32.Parse(terminalArr[1]);
        }
        private int ExtractSubTerminalIdFromLoginResponse(BonaStoco.AP1.Web.sercurityws.loginResponse loginResponse)
        {
            IList<string> companyReserved = loginResponse.reserved.Split(';').ToList();
            string[] subTerminalArr = companyReserved.Where(c => c.Contains("terminalid")).FirstOrDefault().Split('=');
            return Int32.Parse(subTerminalArr[1]);
        }
        private string ExtractVirtualHostFromLoginResponse(sercurityws.loginResponse loginResponse)
        {
            IList<string> companyReserved = loginResponse.reserved.Split(';').ToList();
            string[] virtualHostArr = companyReserved.Where(c => c.ToLower().Contains("virtualhost")).FirstOrDefault().Split('=');
            return virtualHostArr[1];
        }
    }
}