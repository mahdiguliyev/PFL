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
    public class DistrictsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public DistrictsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Rayon və şəhərlər";
            ViewBag.BaseUrl = "Districts";
        }
        // GET: Districts
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var districts =_db.Districts.OrderBy(x=>x.Name).ToPagedList(pageIndex, pageSize);
            return View(districts);
        }
        

        // GET: Districts/Create
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] District district)
        {
            if (ModelState.IsValid)
            {
                district.CreatedById = _user.UserId;
                district.CreationDate = DateTime.Now;
                _db.Districts.Add(district);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(district);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district =_db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            
            return View("Create", district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] District district)
        {
            if (ModelState.IsValid)
            {
                District _district = _db.Districts.Find(district.Id);
                if (_district == null)
                {
                    return HttpNotFound();
                }

                _district.Name = district.Name;
                _district.LastUpdateById = _user.UserId;
                _district.LastUpdatedDate = DateTime.Now;
                _db.Entry(_district).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", district);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            District district = _db.Districts.Find(id);
            if (district != null)
            {
                district.LastUpdateById = _user.UserId;
                district.LastUpdatedDate = DateTime.Now;
                district.IsDeleted = true;
                _db.Entry(district).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            District district = _db.Districts.Find(id);
            if (district != null)
            {   
                district.LastUpdateById = _user.UserId;
                district.LastUpdatedDate = DateTime.Now;
                district.IsDeleted = false;
                _db.Entry(district).State = EntityState.Modified;
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
