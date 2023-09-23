using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFL.Entities.EntityModels;
using PFL.Membership;


namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class ContractTypesController : Controller
    {
        private PFLContext db = new PFLContext();

        // GET: ContractTypes
        public ActionResult Index()
        {
            var contractTypes = db.ContractTypes.Include(c => c.User).Include(c => c.User1);
            return View(contractTypes.ToList());
        }

        // GET: ContractTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractType contractType = db.ContractTypes.Find(id);
            if (contractType == null)
            {
                return HttpNotFound();
            }
            return View(contractType);
        }

        // GET: ContractTypes/Create
        public ActionResult Create()
        {
            ViewBag.LastUpdateById = new SelectList(db.Users, "Id", "UserName");
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: ContractTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastUpdatedDate,LastUpdateById,CreationDate,CreatedById,IsDeleted")] ContractType contractType)
        {
            if (ModelState.IsValid)
            {
                db.ContractTypes.Add(contractType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LastUpdateById = new SelectList(db.Users, "Id", "UserName", contractType.LastUpdateById);
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "UserName", contractType.CreatedById);
            return View(contractType);
        }

        // GET: ContractTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractType contractType = db.ContractTypes.Find(id);
            if (contractType == null)
            {
                return HttpNotFound();
            }
            ViewBag.LastUpdateById = new SelectList(db.Users, "Id", "UserName", contractType.LastUpdateById);
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "UserName", contractType.CreatedById);
            return View(contractType);
        }

        // POST: ContractTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastUpdatedDate,LastUpdateById,CreationDate,CreatedById,IsDeleted")] ContractType contractType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LastUpdateById = new SelectList(db.Users, "Id", "UserName", contractType.LastUpdateById);
            ViewBag.CreatedById = new SelectList(db.Users, "Id", "UserName", contractType.CreatedById);
            return View(contractType);
        }

        // GET: ContractTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractType contractType = db.ContractTypes.Find(id);
            if (contractType == null)
            {
                return HttpNotFound();
            }
            return View(contractType);
        }

        // POST: ContractTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContractType contractType = db.ContractTypes.Find(id);
            db.ContractTypes.Remove(contractType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
