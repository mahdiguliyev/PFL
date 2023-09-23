using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;

using PFL.Utils;

namespace PFL.Controllers
{
    public class ClubOfficialsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public ClubOfficialsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klubların rəsmiləri";
            ViewBag.BaseUrl = "ClubOfficials";
        }
        // GET: Districts
        public ActionResult Index(int? clubId, int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var clubOfficialsQuery = _db.ClubOfficials
                .Include(x => x.Club)
                .Include(x => x.Position)
                .Where(x => !x.IsDeleted && x.ClubConfirm && x.SeasonId == seasonId);

            if (clubId.HasValue)
            {
                clubOfficialsQuery = clubOfficialsQuery.Where(x => x.ClubId == clubId);
                ViewBag.ClubId = clubId;
            }

            var clubOfficials = clubOfficialsQuery
                                    .OrderBy(x => x.Club.Name)
                                    .ThenBy(x => x.FirstName)
                                    .ThenBy(x => x.LastName)
                                    .ThenBy(x => x.FatherName).ToPagedList(pageIndex, pageSize);

            ViewBag.SeasonId = seasonId;

            return View(clubOfficials);
        }


        // GET: Districts/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClubId,OfficialFirstName,OfficialLastName,OfficialFatherName,OfficialPositionId")] ClubOfficial clubOfficial)
        {
            if (ModelState.IsValid)
            {
                clubOfficial.CreatedById = _user.UserId;
                clubOfficial.CreationDate = DateTime.Now;
                _db.ClubOfficials.Add(clubOfficial);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clubOfficial);
        }



        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clubOfficial = _db.ClubOfficials.Include(x => x.Club).Include(x => x.Position)
                .Select(x => new ClassOfficialOperatorUpdateDto()
                {
                    Id = x.Id,
                    ContractBeginDate = x.ContractBeginDate,
                    ContractEndDate = x.ContractEndDate,
                    FullName = x.FirstName + " " + " " + x.LastName + " " + x.FatherName,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PositionName = x.Position.Name
                })
                .FirstOrDefault(x => x.Id == id);
            if (clubOfficial == null)
            {
                return HttpNotFound();
            }

            return View(clubOfficial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClassOfficialOperatorUpdateDto clubOfficialUpdateDto)
        {
            if (ModelState.IsValid)
            {
                ClubOfficial clubOfficial = _db.ClubOfficials.Find(clubOfficialUpdateDto.Id);
                if (clubOfficial == null)
                {
                    return HttpNotFound();
                }

                var logClubOfficial = new LogClubOfficial()
                {
                    MainId = clubOfficial.Id,
                    ClubId = clubOfficial.ClubId,
                    FirstName = clubOfficial.FirstName,
                    LastName = clubOfficial.LastName,
                    FatherName = clubOfficial.FatherName,
                    PositionId = clubOfficial.PositionId,
                    ContractBeginDate = clubOfficial.ContractBeginDate,
                    ContractEndDate = clubOfficial.ContractEndDate,
                    BirthDate = clubOfficial.BirthDate,
                    TerminationDate = clubOfficial.TerminationDate,
                    TerminationReason = clubOfficial.TerminationReason,
                    CitizenshipId = clubOfficial.CitizenshipId,
                    Contact = clubOfficial.Contact,
                    PhotoUrl = clubOfficial.PhotoUrl,
                    PassportUrl = clubOfficial.PassportUrl,
                    ContractUrl = clubOfficial.ContractUrl,
                    TrainerLicenseUrl = clubOfficial.TrainerLicenseUrl,
                    DoctorDiplomaUrl = clubOfficial.DoctorDiplomaUrl,
                    ClubConfirm = clubOfficial.ClubConfirm,
                    ClubConfirmById = clubOfficial.ClubConfirmById,
                    ClubConfirmDate = clubOfficial.ClubConfirmDate,
                    Rejected = clubOfficial.Rejected,
                    OperatorConfirm = clubOfficial.OperatorConfirm,
                    OperatorConfirmById = clubOfficial.OperatorConfirmById,
                    OperatorConfirmDate = clubOfficial.OperatorConfirmDate,
                    LastUpdatedDate = clubOfficial.LastUpdatedDate,
                    LastUpdateById = clubOfficial.LastUpdateById,
                    CreationDate = clubOfficial.CreationDate,
                    CreatedById = clubOfficial.CreatedById,
                    IsDeleted = clubOfficial.IsDeleted
                };    //Mapper.Map<ClubOfficial, LogClubOfficial>(clubOfficial);
                _db.LogClubOfficials.Add(logClubOfficial);
                _db.SaveChanges();

                clubOfficial.ContractEndDate = clubOfficialUpdateDto.ContractEndDate;
                clubOfficial.LastUpdateById = _user.UserId;
                clubOfficial.LastUpdatedDate = DateTime.Now;
                _db.Entry(clubOfficial).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index", new { clubId = clubOfficial.ClubId });
            }
            return View(clubOfficialUpdateDto);
        }




        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Detail(int id)
        {
            var clubOfficial = _db.ClubOfficials
                .Include(x => x.Club)
                .Include(x => x.Position)
                .Include(x => x.Country)
                .FirstOrDefault(x => x.Id == id);

            if (clubOfficial == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var requestRejections = _db.ClubOfficialRejections.Where(x => x.ClubOfficialId == id).OrderByDescending(x => x.CreationDate).ToList();

            ViewBag.RequestRejections = requestRejections;

            return View(clubOfficial);
        }



        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Confirmation(ClubOfficialOperatorDto clubOfficialOperatorDto)
        {

            var clubOfficial = _db.ClubOfficials.FirstOrDefault(x => x.Id == clubOfficialOperatorDto.Id);

            if (clubOfficial == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (clubOfficialOperatorDto.OperatorConfirm)
            {
                clubOfficial.OperatorConfirm = true;
                clubOfficial.Rejected = false;
                clubOfficial.OperatorConfirmById = _user.UserId;
                clubOfficial.OperatorConfirmDate = DateTime.Now;
                clubOfficial.LastUpdateById = _user.UserId;
                clubOfficial.LastUpdatedDate = DateTime.Now;
            }
            else
            {
                clubOfficial.OperatorConfirm = false;
                clubOfficial.Rejected = true;
                clubOfficial.ClubConfirm = false;
                clubOfficial.LastUpdateById = _user.UserId;
                clubOfficial.LastUpdatedDate = DateTime.Now;

                var requestRejection = new ClubOfficialRejection()
                {
                    ClubOfficialId = clubOfficialOperatorDto.Id,
                    RejectionNote = clubOfficialOperatorDto.RejectionNote,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.ClubOfficialRejections.Add(requestRejection);
            }

            _db.SaveChanges();


            //return RedirectToAction("PlayerRequestDetail", new { id = playerRequestOperator.Id });
            return RedirectToAction("Index", new { clubId = clubOfficial.ClubId });
        }




        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClubOfficial clubOfficial = _db.ClubOfficials.Find(id);
            if (clubOfficial != null)
            {
                clubOfficial.LastUpdateById = _user.UserId;
                clubOfficial.LastUpdatedDate = DateTime.Now;
                clubOfficial.IsDeleted = true;
                _db.Entry(clubOfficial).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(int id)
        {
            ClubOfficial clubOfficial = _db.ClubOfficials.Find(id);
            if (clubOfficial != null)
            {
                clubOfficial.LastUpdateById = _user.UserId;
                clubOfficial.LastUpdatedDate = DateTime.Now;
                clubOfficial.IsDeleted = false;
                _db.Entry(clubOfficial).State = EntityState.Modified;
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
