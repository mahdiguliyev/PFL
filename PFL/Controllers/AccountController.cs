using Newtonsoft.Json;
using PFL.Membership;
using PFL.Models.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PFL.Utils;

namespace PFL.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOff();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                CustomMembership membership = new CustomMembership();

                //if (System.Web.Security.Membership.ValidateUser(loginView.UserName, loginView.Password))
                if (membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)membership.GetUser(loginView.UserName, false);
                    if (user != null)
                    {

                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.Name).ToArray()
                        };

                        
                        int? userClubId = RoleHelper.UserClubId(user.UserId);

                        if (userClubId != null)
                        {
                            userModel.UserClubId = userClubId.Value;
                        }


                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Token", enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        if(user != null && user.Roles.Any(x=>x.Name.Contains("club-admin")))
                            return RedirectToAction("Details", "Club");

                        if (user != null && user.Roles.Any(x => x.Name.Contains("referee")))
                            return RedirectToAction("Matches", "Referee");

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(loginView);
        }


        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterViewModel registerViewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        CustomMembership membership = new CustomMembership();

        //        membership.CreateUser(registerViewModel.UserName, registerViewModel.Password, registerViewModel.Email, "pfl", "pfl", true, "key", out System.Web.Security.MembershipCreateStatus status);

        //        if (status == System.Web.Security.MembershipCreateStatus.Success)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //    }

        //    return View(registerViewModel);
        //}


        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie("Token", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            HttpCookie cookieClubName = new HttpCookie("ClubName");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }


        //[NonAction]
        //public void VerificationEmail(string email, string activationCode)
        //{
        //    var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

        //    var fromEmail = new MailAddress("xxxxx@gmail.com", "");
        //    var toEmail = new MailAddress(email);

        //    var fromEmailPassword = "******************";
        //    string subject = "Activation Account !";

        //    string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        //    };

        //    using (var message = new MailMessage(fromEmail, toEmail)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true

        //    })

        //        smtp.Send(message);

        //}

        //[CustomAuthorize(Roles = "admin")]
        //public JsonResult ChangePassword(AdminPasswordChange model)
        //{

        //    GenericResponse<string> response = new GenericResponse<string>();

        //    string HassedPassword = SHAHelper.SHA512Generator(model.Password);

        //    using (PFLContext db = new PFLContext())
        //    {
        //        Models.Entities.User user = db.Users.Where(x => x.Id == model.UserId).FirstOrDefault();

        //        if (user != null)
        //        {
        //            user.Password = HassedPassword;
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            response.success = false;
        //            response.error = new BaseExceptionModel()
        //            {
        //                code = 1,
        //                message = "İstifadəçi tapılmadı."
        //            };
        //        }

        //    }

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

    }
}