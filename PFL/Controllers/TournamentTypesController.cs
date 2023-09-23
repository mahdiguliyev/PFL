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
    public class TournamentTypesController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public TournamentTypesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Turnir tipləri";
            ViewBag.BaseUrl = "TournamentTypes";
        }

        // GET: TournamentTypes
        public ActionResult Index()
        {
            var tournamentTypes = _db.TournamentTypes;//.Include(t => t.User);
            return View(tournamentTypes.ToList());
        }

        // GET: TournamentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentType tournamentType = _db.TournamentTypes.Find(id);
            if (tournamentType == null)
            {
                return HttpNotFound();
            }
            return View(tournamentType);
        }

        // GET: TournamentTypes/Create
        public ActionResult Create()
        {
            ViewBag.Action = "Create";
            //ViewBag.LastUpdateById = new SelectList(_db.Users, "Id", "UserName");
            //ViewBag.CreatedById = new SelectList(_db.Users, "Id", "UserName");
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TournamentType tournamentType)
        {
            if (ModelState.IsValid)
            {
                tournamentType.CreatedById = _user.UserId;
                tournamentType.CreationDate = DateTime.Now;
                _db.TournamentTypes.Add(tournamentType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournamentType);
        }

        // GET: TournamentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Action = "Edit";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TournamentType tournamentType = _db.TournamentTypes.Find(id);
            if (tournamentType == null)
            {
                return HttpNotFound();
            }
          
            return View("Create", tournamentType);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TournamentType tournamentType)
        {
            if (ModelState.IsValid)
            {
                TournamentType _tournamentType = _db.TournamentTypes.Find(tournamentType.Id);
                if (_tournamentType == null)
                {
                    return HttpNotFound();
                }

                _tournamentType.Name = tournamentType.Name;
                _tournamentType.LastUpdateById = _user.UserId;
                _tournamentType.LastUpdatedDate = DateTime.Now;
                _db.Entry(_tournamentType).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", tournamentType);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TournamentType tournamentType = _db.TournamentTypes.Find(id);
            if (tournamentType != null)
            {
                tournamentType.LastUpdateById = _user.UserId;
                tournamentType.LastUpdatedDate = DateTime.Now;
                tournamentType.IsDeleted = true;
                _db.Entry(tournamentType).State = EntityState.Modified;
                _db.SaveChanges();
            }

            
            return RedirectToAction("Index");
        }

        // POST: TournamentTypes/Undo/5
        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            TournamentType tournamentType = _db.TournamentTypes.Find(id);
            if (tournamentType != null)
            {
                tournamentType.LastUpdateById = _user.UserId;
                tournamentType.LastUpdatedDate = DateTime.Now;
                tournamentType.IsDeleted = false;
                _db.Entry(tournamentType).State = EntityState.Modified;
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
