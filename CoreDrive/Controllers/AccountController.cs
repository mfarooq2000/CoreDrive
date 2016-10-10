using System;
using System.Web.Mvc;
using CoreDrive.Models;
using CoreDrive.DAL;
using CoreDrive.Authentication;

namespace CoreDrive.Controllers
{

    public class AccountController : AuthenticationController
    {

        DataManager dm;


        public AccountController()
        {
            dm = new DataManager();
        }

        public ActionResult Login()
        {
            //Redirect user to home page if already is Authenticated state
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            AccountModel model = new AccountModel();
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(AccountModel account)
        {
            //Redirect user to home page if already is Authenticated state
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            //check email existence in CRM
            account = dm.ValidateUser(account);
            if (account != null)
            {
                FormsService.SignIn(account.AccountId.ToString(), account.Name, account.Email);
                Session["LoggedInUser"] = account;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect or No user exists against provided email address");
            }

            // If we got this far, something failed, redisplay form
            return View(account);
        }

        private void ResetUserSession()
        {
            //try
            //{
            //    if (User.Identity.IsAuthenticated)
            //    {
            //        Guid concid = CurrentUser.CurrentUserId;
            //        if (concid != Guid.Empty)
            //            Session["LoggedInUser"] = DataManager.GetContactRecord(concid);
            //    }
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
        }



    }
}