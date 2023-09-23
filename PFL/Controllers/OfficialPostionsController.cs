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
    public class OfficialPositionsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public OfficialPositionsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klub rəsmi vəzifələr";
            ViewBag.BaseUrl = "OfficialPositions";
        }
        // GET: Districts
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var officialPositions =_db.OfficialPositions.OrderBy(x=>x.Name).ToPagedList(pageIndex, pageSize);
            return View(officialPositions);
        }
        

        // GET: Districts/Create
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] OfficialPosition officialPosition)
        {
            if (ModelState.IsValid)
            {
                officialPosition.CreatedById = _user.UserId;
                officialPosition.CreationDate = DateTime.Now;
                _db.OfficialPositions.Add(officialPosition);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(officialPosition);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficialPosition officialPosition =_db.OfficialPositions.Find(id);
            if (officialPosition == null)
            {
                return HttpNotFound();
            }
            
            return View("Create", officialPosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] OfficialPosition officialPosition)
        {
            if (ModelState.IsValid)
            {
                OfficialPosition _officialPosition = _db.OfficialPositions.Find(officialPosition.Id);
                if (_officialPosition == null)
                {
                    return HttpNotFound();
                }

                _officialPosition.Name = officialPosition.Name;
                _officialPosition.LastUpdateById = _user.UserId;
                _officialPosition.LastUpdatedDate = DateTime.Now;
                _db.Entry(_officialPosition).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", officialPosition);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OfficialPosition officialPosition = _db.OfficialPositions.Find(id);
            if (officialPosition != null)
            {
                officialPosition.LastUpdateById = _user.UserId;
                officialPosition.LastUpdatedDate = DateTime.Now;
                officialPosition.IsDeleted = true;
                _db.Entry(officialPosition).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            OfficialPosition officialPosition = _db.OfficialPositions.Find(id);
            if (officialPosition != null)
            {   
                officialPosition.LastUpdateById = _user.UserId;
                officialPosition.LastUpdatedDate = DateTime.Now;
                officialPosition.IsDeleted = false;
                _db.Entry(officialPosition).State = EntityState.Modified;
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
