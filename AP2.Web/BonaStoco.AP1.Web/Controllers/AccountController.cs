using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BonaStoco.AP1.Web.Models;
using System.Security.Cryptography;
using System.Text;
using BonaStoco.Inf.ExceptionUtils;
namespace BonaStoco.AP1.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                BonaStocoWSLogon bonaWSLogon = new BonaStocoWSLogon();
                bonaWSLogon.Login(model.UserName, model.Password);
                bool authenticateOnInternet = bonaWSLogon.IsAuthenticated;
                bool validatedUserOnLocal = Membership.ValidateUser(model.UserName, model.Password);

                if (authenticateOnInternet && !validatedUserOnLocal)
                {
                    try
                    {
                        Membership.DeleteUser(model.UserName);
                        bonaWSLogon.CreateLocalUserIfNecessary();
                        validatedUserOnLocal = true;
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Gagal memperbaharui password Anda di lokal dengan pesan error :" + e.GetInnermostException());
                    }
                }

                if (authenticateOnInternet)
                {
                    if (validatedUserOnLocal)
                    {
                        CompanyProfiles cp = new CompanyProfiles(this.HttpContext);
                        cp.CompanyReserved = bonaWSLogon.Response.reserved;
                        cp.CompanyId = bonaWSLogon.Response.companyid;
                        cp.CompanyName = bonaWSLogon.Response.company;
                        cp.Role = bonaWSLogon.ROLE;
                        cp.RoleName = APRoles.MapRoleName(cp.Role);
                        cp.HomePage = APRoles.MapHomePage(cp.Role);

                        Response.Cookies.Add(new HttpCookie("tenantid", cp.CompanyId.ToString()));
                        Response.Cookies.Add(new HttpCookie("tenantname", cp.CompanyName));

                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            UserToken = bonaWSLogon.Response.token;
                            return RedirectToAction(cp.HomePage, "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("",
                        bonaWSLogon.ErrorMessage.Trim() != string.Empty ?
                        bonaWSLogon.ErrorMessage :
                        "User atau password anda salah.");
                }
            }
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Administrator")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        [Authorize(Roles = "Administrator")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult UbahPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UbahPassword(ChangePasswordModel model)
        {
            sercurityws.BonastocoServices bonaWS = new sercurityws.BonastocoServices();
            BonaStocoWSLogon wsLogon = new BonaStocoWSLogon();
            sercurityws.serverResponse response = bonaWS.changepassword(new sercurityws.changepasswd() { token = UserToken, oldpassword = wsLogon.MD5(model.OldPassword), newpassword = wsLogon.MD5(model.NewPassword) });
            if (response.status != 0)
            {
                ModelState.AddModelError("", response.message);
                return View();
            }
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
            bool changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
            if (!changePasswordSucceeded)
            {
                ModelState.AddModelError("", "Batal merubah password");
                return View();
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn");
        }

        private string UserToken
        {
            get { return (string)this.HttpContext.Session["usertoken"]; }
            set { this.HttpContext.Session["usertoken"] = value; }
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}