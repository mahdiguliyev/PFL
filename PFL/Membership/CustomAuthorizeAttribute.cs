using System.Web;
using System.Web.Mvc;

namespace PFL.Membership
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        protected virtual CustomPrincipal CurrentUser => HttpContext.Current.User as CustomPrincipal;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ((CurrentUser == null || CurrentUser.IsInRole(Roles)) && CurrentUser != null);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RedirectToRouteResult routeData = null;

            if (CurrentUser == null)
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Account",
                        action = "Login",
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied"
                 }
                 ));
            }

            filterContext.Result = routeData;
        }

    }
}