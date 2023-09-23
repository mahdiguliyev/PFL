using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;
using PFL.Models.ViewModels;
using PFL.Utils;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class ClubsController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public ClubsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klublar";
            ViewBag.BaseUrl = "Clubs";
        }


        // GET: Clubs
        public ActionResult Index(string clubName, int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;


            var clubsQuery = _db.VClubs.AsQueryable();

            if (!string.IsNullOrEmpty(clubName))
            {
                clubsQuery = clubsQuery.Where(x => x.ClubName.Contains(clubName));
            }
            var clubs = clubsQuery.OrderByDescending(x => (x.ClubPlayerRequestCount + x.ClubOfficialRequestCount)).ToPagedList(pageIndex, pageSize);


            ViewBag.SearchKeyClubName = clubName;

            return View(clubs);
        }


        public ActionResult Notifications(string clubName, int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = 20;//Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var clubsQuery = _db.VClubNotifications.Where(x => x.SeasonId == seasonId);

            if (!string.IsNullOrEmpty(clubName))
            {
                clubsQuery = clubsQuery.Where(x => x.ClubName.Contains(clubName));
            }
            var clubs = clubsQuery.OrderByDescending(x => (x.ClubPlayerRequestCount + x.ClubOfficialRequestCount)).ToPagedList(pageIndex, pageSize);


            ViewBag.SearchKeyClubName = clubName;
            ViewBag.SeasonId = seasonId;

            return View(clubs);
        }




        // GET: Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = _db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // GET: Clubs/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name");

            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClubViewModel clubViewModel)
        {

            if (ModelState.IsValid)
            {

                Club club = Mapper.Map<ClubViewModel, Club>(clubViewModel);


                if (clubViewModel.LogoUpload != null && clubViewModel.LogoUpload.ContentLength > 0)
                {
                    var uploadDir = "/Files/Logos/";
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var logoPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubViewModel.LogoUpload.FileName);
                    var logoUrl = Path.Combine(uploadDir, clubViewModel.LogoUpload.FileName);
                    clubViewModel.LogoUpload.SaveAs(logoPath);
                    club.LogoUrl = logoUrl.Replace("\\", "/");
                }


                if (clubViewModel.StadiumPhotoUpload != null && clubViewModel.StadiumPhotoUpload.ContentLength > 0)
                {
                    var uploadDir = "/Files/Staduims/";
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var stadiumPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubViewModel.StadiumPhotoUpload.FileName);
                    var stadiumUrl = Path.Combine(uploadDir, clubViewModel.StadiumPhotoUpload.FileName);
                    clubViewModel.StadiumPhotoUpload.SaveAs(stadiumPath);
                    club.StadiumPhotoUrl = stadiumPath.Replace("\\", "/");
                }



                club.CreatedById = _user.UserId;
                club.CreationDate = DateTime.Now;

                _db.Clubs.Add(club);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name", clubViewModel.DistrictId);
            return View(clubViewModel);
        }

        // GET: Clubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = _db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name", club.DistrictId);


            ClubViewModel clubViewModel = Mapper.Map<Club, ClubViewModel>(club);

            return View("Create", clubViewModel);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClubViewModel clubViewModel)
        {


            if (ModelState.IsValid)
            {
                Club club = Mapper.Map<ClubViewModel, Club>(clubViewModel);


                club.CreatedById = _user.UserId;
                club.CreationDate = DateTime.Now;

                _db.Entry(club).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name", clubViewModel.DistrictId);

            return View("Create", clubViewModel);
        }



        [HttpGet]
        public ActionResult Documents(int clubId, int seasonId)
        {
            Club club = _db.Clubs.FirstOrDefault(x => x.Id == clubId);

            if (club == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var clubTournamentType = _db.ClubTournamentTypes.FirstOrDefault(x => x.ClubId == club.Id);

            ViewBag.ClubTournamentTypeId = clubTournamentType?.TournamentTypeId ?? 0;
            ViewBag.SeasonId = seasonId;

            return View(club);
        }

        [HttpGet]
        public ActionResult DocumentsByType(int clubId, int clubType, int seasonId)
        {
            if (!new[] {2, 3, 4}.Contains(clubType))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var season = SeasonHelper.GetSeason(seasonId);

            SqlParameter seasonIdParam = new SqlParameter("seasonId", season.Id);
            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter documentTypeIdParam = new SqlParameter("documentTypeId", clubType);
            List<ClubDocumentDto> clubDocuments = _db.Database.SqlQuery<ClubDocumentDto>("GetClubDocuments @seasonId, @clubId, @documentTypeId", seasonIdParam, clubIdParam, documentTypeIdParam).ToList();


            ViewBag.ClubType = clubType;

            return View("DocumentList", clubDocuments);
        }

        [HttpGet]
        public ActionResult DocumentsByTypeRow(int clubId, int clubType, int documentId, int seasonId)
        {
            if (!new[] { 2, 3, 4 }.Contains(clubType))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var season = SeasonHelper.GetSeason(seasonId);

            SqlParameter seasonIdParam = new SqlParameter("seasonId", season.Id);
            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter documentTypeIdParam = new SqlParameter("documentTypeId", clubType);
            List<ClubDocumentDto> clubDocuments = _db.Database.SqlQuery<ClubDocumentDto>("GetClubDocuments @seasonId, @clubId, @documentTypeId", seasonIdParam, clubIdParam, documentTypeIdParam).ToList();


            ViewBag.ClubType = clubType;

            return View("DocumentRow", clubDocuments.FirstOrDefault(x=>x.DocumentId == documentId));
        }


        [HttpGet]
        public ActionResult DocumentConfirmation(int documentId, bool confirm)
        {
            var clubDocument = _db.ClubDocuments.FirstOrDefault(x => x.Id == documentId);
            ViewBag.confirm = confirm;

            return View(clubDocument);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult DocumentConfirmation(OperatorConfirmationDto operatorConfirmation)
        {

            var clubDocument = _db.ClubDocuments.FirstOrDefault(x => x.Id == operatorConfirmation.Id);

            if (clubDocument == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (operatorConfirmation.OperatorConfirm)
            {
                clubDocument.OperatorConfirm = true;
                clubDocument.Rejected = false;
                clubDocument.OperatorConfirmById = _user.UserId;
                clubDocument.OperatorConfirmDate = DateTime.Now;
                clubDocument.LastUpdateById = _user.UserId;
                clubDocument.LastUpdatedDate = DateTime.Now;
            }
            else
            {
                clubDocument.OperatorConfirm = false;
                clubDocument.Rejected = true;
                clubDocument.ClubConfirm = false;
                clubDocument.LastUpdateById = _user.UserId;
                clubDocument.LastUpdatedDate = DateTime.Now;

                var clubDocumentRejection = new ClubDocumentRejection()
                {
                    ClubDocumentId = operatorConfirmation.Id,
                    RejectionNote = operatorConfirmation.RejectionNote,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.ClubDocumentRejections.Add(clubDocumentRejection);
            }

            _db.SaveChanges();


            //return RedirectToAction("PlayerRequests");

            return Json(new { ok = true, Id = clubDocument.Id, DocumentNameId = clubDocument.ClubDocumenNameId });
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
