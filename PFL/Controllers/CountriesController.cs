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
    public class CountriesController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public CountriesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Ölkələr";
            ViewBag.BaseUrl = "Countries";
        }

        // GET: Countries
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var countries = _db.Countries.OrderBy(x=>x.Sort).ThenBy(x => x.Name).ToPagedList(pageIndex, pageSize);
            return View(countries);
        }

      
        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                country.CreatedById = _user.UserId;
                country.CreationDate = DateTime.Now;
                _db.Countries.Add(country);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = _db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdateById = new SelectList(_db.Users, "Id", "UserName", country.LastUpdateById);
            ViewBag.CreatedById = new SelectList(_db.Users, "Id", "UserName", country.CreatedById);
            return View("Create", country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                Country _country = _db.Countries.Find(country.Id);
                if (_country == null)
                {
                    return HttpNotFound();
                }

                _country.Name = country.Name;
                _country.LastUpdateById = _user.UserId;
                _country.LastUpdatedDate = DateTime.Now;
                _db.Entry(_country).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create", country);
        }


        
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = _db.Countries.Find(id);
            if (country != null)
            {
                country.LastUpdateById = _user.UserId;
                country.LastUpdatedDate = DateTime.Now;
                country.IsDeleted = true;
                _db.Entry(country).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            Country country = _db.Countries.Find(id);
            if (country != null)
            {
                country.LastUpdateById = _user.UserId;
                country.LastUpdatedDate = DateTime.Now;
                country.IsDeleted = false;
                _db.Entry(country).State = EntityState.Modified;
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
