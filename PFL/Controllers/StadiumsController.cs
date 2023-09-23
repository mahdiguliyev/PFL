using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;

using PFL.Utils;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class StadiumsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public StadiumsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Stadionlar";
            ViewBag.BaseUrl = "Stadiums";
        }
        // GET: Districts
        public ActionResult Index(string name, int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var stadiumsQuery = _db.Stadiums.Where(x=>!x.IsClosed).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {

                stadiumsQuery = stadiumsQuery.Where(x => x.Name.Contains(name));
    
            }
            var stadiums = stadiumsQuery.Include(x => x.District).OrderBy(x => x.Name).ToPagedList(pageIndex, pageSize);



            return View(stadiums);
        }
        

        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x=>!x.IsDeleted).OrderBy(x=>x.Id), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DistrictId")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                stadium.CreatedById = _user.UserId;
                stadium.CreationDate = DateTime.Now;
                _db.Stadiums.Add(stadium);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");
            return View(stadium);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadium stadium =_db.Stadiums.Find(id);
            if (stadium == null)
            {
                return HttpNotFound();
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");

            return View("Create", stadium);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DistrictId")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                Stadium _stadium = _db.Stadiums.Find(stadium.Id);
                if (_stadium == null)
                {
                    return HttpNotFound();
                }

                _stadium.Name = stadium.Name;
                _stadium.DistrictId = stadium.DistrictId;

                _stadium.LastUpdateById = _user.UserId;
                _stadium.LastUpdatedDate = DateTime.Now;
                _db.Entry(_stadium).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");
            return View("Create", stadium);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stadium stadium = _db.Stadiums.Find(id);
            if (stadium != null)
            {
                stadium.LastUpdateById = _user.UserId;
                stadium.LastUpdatedDate = DateTime.Now;
                stadium.IsDeleted = true;
                _db.Entry(stadium).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            Stadium stadium = _db.Stadiums.Find(id);
            if (stadium != null)
            {   
                stadium.LastUpdateById = _user.UserId;
                stadium.LastUpdatedDate = DateTime.Now;
                stadium.IsDeleted = false;
                _db.Entry(stadium).State = EntityState.Modified;
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
