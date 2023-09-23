using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFL.Entities.EntityModels;
using PFL.Membership;


namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class RolesController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public RolesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Rollar";
            ViewBag.BaseUrl = "Roles";
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View(_db.Roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.CreatedById = _user.UserId;
                role.CreationDate = DateTime.Now;
                _db.Roles.Add(role);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("Create", role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                Role _role = _db.Roles.Find(role.Id);
                if (_role == null)
                {
                    return HttpNotFound();
                }

                _role.Name = role.Name;
                _role.LastUpdateById = _user.UserId;
                _role.LastUpdatedDate = DateTime.Now;
                _db.Entry(_role).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            return View("Create", role);
        }

        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = _db.Roles.Find(id);
            if (role != null)
            {
                role.LastUpdateById = _user.UserId;
                role.LastUpdatedDate = DateTime.Now;
                role.IsDeleted = true;
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        // POST: TournamentTypes/Undo/5
        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            Role role = _db.Roles.Find(id);
            if (role != null)
            {
                role.LastUpdateById = _user.UserId;
                role.LastUpdatedDate = DateTime.Now;
                role.IsDeleted = false;
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
