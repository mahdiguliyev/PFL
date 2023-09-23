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
    public class RefereesController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public RefereesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Hakimlər, nümayəndələr və inspektorlar";
            ViewBag.BaseUrl = "Referees";
        }
        // GET: Districts
        public ActionResult Index(string refereeName, int? page, int status = 1)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var refereesQuery = _db.Referees.AsQueryable();

            if (!string.IsNullOrEmpty(refereeName))
            {
                string[] searchKeys = refereeName.Split(' ');

                if (searchKeys.Length > 5)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                foreach (var item in searchKeys)
                {

                    refereesQuery =
                        refereesQuery.Where(x => (x.FirstName.ToLower() + x.LastName.ToLower() + x.FatherName.ToLower()).Contains(item));
                }
            }
            ViewBag.RefereeName = refereeName;


            if (status == 1)
            {
                refereesQuery = refereesQuery.Where(x => !x.IsClosed);
            }
            else if (status == 2)
            {
                refereesQuery = refereesQuery.Where(x => x.IsClosed);
            }

            ViewBag.Status = status;

            var referees = refereesQuery.Include(x => x.District).Include(x => x.RefereeType).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToPagedList(pageIndex, pageSize);



            return View(referees);
        }


        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted).OrderBy(x => x.Id), "Id", "Name");
            ViewBag.RefereeTypeId = new SelectList(_db.RefereeTypes, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,FatherName,Grade,DistrictId,RefereeTypeId")] Entities.EntityModels.Referee referee)
        {
            if (ModelState.IsValid)
            {
                referee.CreatedById = _user.UserId;
                referee.CreationDate = DateTime.Now;
                _db.Referees.Add(referee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");
            ViewBag.RefereeTypeId = new SelectList(_db.RefereeTypes, "Id", "Name");
            return View(referee);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entities.EntityModels.Referee referee = _db.Referees.Find(id);
            if (referee == null)
            {
                return HttpNotFound();
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name", referee.DistrictId);
            ViewBag.RefereeTypeId = new SelectList(_db.RefereeTypes, "Id", "Name");

            return View("Create", referee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,FatherName,Grade,DistrictId,RefereeTypeId")] Entities.EntityModels.Referee referee)
        {
            if (ModelState.IsValid)
            {
                Entities.EntityModels.Referee _referee = _db.Referees.Find(referee.Id);
                if (_referee == null)
                {
                    return HttpNotFound();
                }

                _referee.RefereeTypeId = referee.RefereeTypeId;
                _referee.FirstName = referee.FirstName;
                _referee.LastName = referee.LastName;
                _referee.FatherName = referee.FatherName;
                _referee.Grade = referee.Grade;
                _referee.DistrictId = referee.DistrictId;

                _referee.LastUpdateById = _user.UserId;
                _referee.LastUpdatedDate = DateTime.Now;
                _db.Entry(_referee).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");
            ViewBag.RefereeTypeId = new SelectList(_db.RefereeTypes, "Id", "Name");
            return View("Create", referee);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entities.EntityModels.Referee referee = _db.Referees.Find(id);
            if (referee != null)
            {
                referee.LastUpdateById = _user.UserId;
                referee.LastUpdatedDate = DateTime.Now;
                referee.IsDeleted = true;
                referee.IsClosed = true;
                _db.Entry(referee).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            Entities.EntityModels.Referee referee = _db.Referees.Find(id);
            if (referee != null)
            {
                referee.LastUpdateById = _user.UserId;
                referee.LastUpdatedDate = DateTime.Now;
                referee.IsDeleted = false;
                referee.IsClosed = false;
                _db.Entry(referee).State = EntityState.Modified;
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
