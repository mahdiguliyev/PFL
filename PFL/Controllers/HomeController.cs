using PFL.Membership;
using System.Web.Mvc;
using BLL;


namespace PFL.Controllers
{
    public class HomeController : Controller
    {
        //private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public HomeController()
        {
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
        }

        [CustomAuthorize(Roles = "admin,club-admin,referee")]
        public ActionResult Index()
        {
            bool hasClubAdmin = _user.IsInRole("club-admin");
            bool hasReferee = _user.IsInRole("referee");

            if (hasClubAdmin)
            {
                return RedirectToAction("Details", "Club");
            }

            if (hasReferee)
            {
                return RedirectToAction("Matches", "Referee");
            }

            return View();
        }
    }
}