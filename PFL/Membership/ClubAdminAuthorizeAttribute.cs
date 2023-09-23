using System.Web;
using System.Web.Mvc;

namespace PFL.Membership
{
    public class ClubAdminAuthorizeAttribute : AuthorizeAttribute
    {

        protected virtual CustomPrincipal CurrentUser => HttpContext.Current.User as CustomPrincipal;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return CurrentUser.UserClubId.HasValue;
        }

    }
}