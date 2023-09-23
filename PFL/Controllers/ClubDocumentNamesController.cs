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
    public class ClubDocumentNamesController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public ClubDocumentNamesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Club sənəd adları";
            ViewBag.BaseUrl = "ClubDocumentNames";
        }
        // GET: Districts
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var documentNames =_db.ClubDocumentNames.Include(x=>x.ClubDocumentType).OrderBy(x=>x.Label.Substring(0,1)).ThenBy(x=>x.Sort).ToPagedList(pageIndex, pageSize);
            return View(documentNames);
        }
        

        // GET: Districts/Create
        public ActionResult Create()
        {

            ViewBag.ClubDocumentTypeId = new SelectList(_db.ClubDocumentTypes, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClubDocumentName documentName)
        {
            if (ModelState.IsValid)
            {
                documentName.CreatedById = _user.UserId;
                documentName.CreationDate = DateTime.Now;
                _db.ClubDocumentNames.Add(documentName);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClubDocumentTypeId = new SelectList(_db.ClubDocumentTypes, "Id", "Name");
            return View(documentName);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClubDocumentName documentName =_db.ClubDocumentNames.Find(id);
            if (documentName == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClubDocumentTypeId = new SelectList(_db.ClubDocumentTypes, "Id", "Name");
            return View("Create", documentName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClubDocumentName documentName)
        {
            if (ModelState.IsValid)
            {
                ClubDocumentName _documentName = _db.ClubDocumentNames.Find(documentName.Id);
                if (_documentName == null)
                {
                    return HttpNotFound();
                }

                _documentName.Label = documentName.Label;
                _documentName.Name = documentName.Name;
                _documentName.Description = documentName.Description;
                _documentName.DocumentExtentions = documentName.DocumentExtentions;
                _documentName.ClubDocumentTypeId = documentName.ClubDocumentTypeId;
                _documentName.IsMultipleFile = documentName.IsMultipleFile;
                _documentName.Sort = documentName.Sort;
                _documentName.LastUpdateById = _user.UserId;
                _documentName.LastUpdatedDate = DateTime.Now;
                _db.Entry(_documentName).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClubDocumentTypeId = new SelectList(_db.ClubDocumentTypes, "Id", "Name");
            return View("Create", documentName);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubDocumentName documentName = _db.ClubDocumentNames.Find(id);
            if (documentName != null)
            {
                documentName.LastUpdateById = _user.UserId;
                documentName.LastUpdatedDate = DateTime.Now;
                documentName.IsDeleted = true;
                _db.Entry(documentName).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            ClubDocumentName documentName = _db.ClubDocumentNames.Find(id);
            if (documentName != null)
            {   
                documentName.LastUpdateById = _user.UserId;
                documentName.LastUpdatedDate = DateTime.Now;
                documentName.IsDeleted = false;
                _db.Entry(documentName).State = EntityState.Modified;
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
