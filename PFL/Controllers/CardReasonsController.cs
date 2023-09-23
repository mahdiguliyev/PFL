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
    [CustomAuthorize(Roles="admin")]
    public class CardReasonsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public CardReasonsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Cərimə səbəbləri";
            ViewBag.BaseUrl = "CardReasons";
        }
        // GET: Districts
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var cardReasons =_db.CardReasons.OrderBy(x=>x.Name).ToPagedList(pageIndex, pageSize);
            return View(cardReasons);
        }
        

        // GET: Districts/Create
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] CardReason cardReason)
        {
            if (ModelState.IsValid)
            {
                cardReason.CreatedById = _user.UserId;
                cardReason.CreationDate = DateTime.Now;
                _db.CardReasons.Add(cardReason);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cardReason);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardReason cardReason =_db.CardReasons.Find(id);
            if (cardReason == null)
            {
                return HttpNotFound();
            }
            
            return View("Create", cardReason);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] CardReason cardReason)
        {
            if (ModelState.IsValid)
            {
                CardReason _cardReason = _db.CardReasons.Find(cardReason.Id);
                if (_cardReason == null)
                {
                    return HttpNotFound();
                }

                _cardReason.Name = cardReason.Name;
                _cardReason.LastUpdateById = _user.UserId;
                _cardReason.LastUpdatedDate = DateTime.Now;
                _db.Entry(_cardReason).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", cardReason);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CardReason cardReason = _db.CardReasons.Find(id);
            if (cardReason != null)
            {
                cardReason.LastUpdateById = _user.UserId;
                cardReason.LastUpdatedDate = DateTime.Now;
                cardReason.IsDeleted = true;
                _db.Entry(cardReason).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            CardReason cardReason = _db.CardReasons.Find(id);
            if (cardReason != null)
            {   
                cardReason.LastUpdateById = _user.UserId;
                cardReason.LastUpdatedDate = DateTime.Now;
                cardReason.IsDeleted = false;
                _db.Entry(cardReason).State = EntityState.Modified;
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
