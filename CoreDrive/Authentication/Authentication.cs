using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace CoreDrive.Authentication
{
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.   

    public interface IFormsAuthenticationService
    {
        void SignIn(string UserID, string userDisplayName, string userEmail);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string UserID, string userDisplayName, string userEmail)
        {
            string AuthenticationToken = UserID + "|" + userDisplayName + "|" + userEmail;
            if (String.IsNullOrEmpty(AuthenticationToken)) throw new ArgumentException("Value cannot be null or empty.", "AuthenticationToken");
            if (AuthenticationToken.Split('|').Length != 3) throw new ArgumentException("Invalid Authentication Token.", "AuthenticationToken");
            FormsAuthentication.SetAuthCookie(AuthenticationToken, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public static Guid CurrentUserId
        {
            get
            {
                Guid userId = Guid.Empty;
                try
                {
                    if (HttpContext.Current.Request.IsAuthenticated)
                        userId = new Guid(HttpContext.Current.User.Identity.Name.Split('|')[0]);
                }
                catch
                {
                    //to avoid un authenticated access or security break
                    //SignOut();
                }
                return userId;
            }
        }

        public static string CurrentDisplayName
        {
            get
            {
                string displayName = string.Empty;
                try
                {
                    if (HttpContext.Current.Request.IsAuthenticated)
                        displayName = HttpContext.Current.User.Identity.Name.Split('|')[1];
                }
                catch
                {
                    //to avoid un authenticated access or security break
                    //SignOut();
                }
                return displayName;
            }
        }

        public static string CurrentUserEmail
        {
            get
            {
                string currentUserEmail = string.Empty;
                try
                {
                    if (HttpContext.Current.Request.IsAuthenticated)
                        currentUserEmail = HttpContext.Current.User.Identity.Name.Split('|')[2];
                }
                catch
                {
                    //to avoid un authenticated access or security break
                    //SignOut();
                }
                return currentUserEmail;
            }
        }
    }


    public class AuthenticationController : Controller
    {
        private FormsAuthenticationService CurrentUser = new FormsAuthenticationService();


        public IFormsAuthenticationService FormsService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }

            base.Initialize(requestContext);
        }
    }

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // the "new" must be used here because we are overriding
        // the Roles property on the underlying class
        //public new SiteRoles Roles;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            //SiteRoles role = new FormsAuthenticationService().CurrentUserRole;

            //if (Roles != 0 && ((Roles & role) != role))
            //    return false;

            return true;

        }
    }

    [Serializable]
    [Flags]
    public enum SiteRoles
    {
        User = 0 << -1,
        Admin = 1 << 0
    }
}