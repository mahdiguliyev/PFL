using PFL.Membership;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFL.Entities.EntityModels;
using PFL.Models.DTO;
using PFL.Utils;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    public class ClubDocumentsController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public ClubDocumentsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klubun sənədləri";
            ViewBag.BaseUrl = "ClubDocumentsOld";
        }




        // GET: ClubDocumentsOld
        public ActionResult Index()
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Club club = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);

            if (club == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            var clubTournamentType = _db.ClubTournamentTypes.FirstOrDefault(x => x.ClubId == club.Id);

            ViewBag.ClubTournamentTypeId = clubTournamentType?.TournamentTypeId ?? 0;


            return View(club);
        }

        [HttpGet]
        public ActionResult List(int clubType)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!new[] { 2, 3, 4 }.Contains(clubType))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var seasonId = SeasonHelper.GetCurrentSeasonId();


            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter documentTypeIdParam = new SqlParameter("documentTypeId", clubType);
            List<ClubDocumentDto> clubDocuments = _db.Database.SqlQuery<ClubDocumentDto>("GetClubDocuments @seasonId, @clubId, @documentTypeId", seasonIdParam, clubIdParam, documentTypeIdParam).ToList();


            ViewBag.ClubType = clubType;

            return View(clubDocuments);
        }

        [HttpGet]
        public ActionResult Row(int clubType, int documentId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!new[] { 2, 3, 4 }.Contains(clubType))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var seasonId = SeasonHelper.GetCurrentSeasonId();

            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter documentTypeIdParam = new SqlParameter("documentTypeId", clubType);
            List<ClubDocumentDto> clubDocuments = _db.Database.SqlQuery<ClubDocumentDto>("GetClubDocuments @seasonId, @clubId, @documentTypeId", seasonIdParam, clubIdParam, documentTypeIdParam).ToList();


            return View(clubDocuments.FirstOrDefault(x => x.DocumentId == documentId));
        }

        [HttpPost]
        public ActionResult UploadDocument(int documentNameId, int? documentId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ClubDocumentName clubDocumentName = _db.ClubDocumentNames.FirstOrDefault(x => x.Id == documentNameId);

            if (clubDocumentName == null)
                return Json(new { ok = false, error = "Səhv müraciət!" });





            if (documentId.HasValue)
            {
                ClubDocument _clubDocument = _db.ClubDocuments.FirstOrDefault(x => x.Id == documentId);

                if (_clubDocument == null)
                    return Json(new { ok = false, error = "Səhv müraciət!" });

                _clubDocument.LastUpdatedDate = DateTime.Now;
                _clubDocument.LastUpdateById = _user.UserId;
                _clubDocument.IsDeleted = true;
                _db.SaveChanges();
            }

            ClubDocument clubDocument = null;
            clubDocument = new ClubDocument { CreationDate = DateTime.Now, CreatedById = _user.UserId };
            _db.ClubDocuments.Add(clubDocument);


            var uploadDir = "/Files/ClubDocuments/";



            if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                //FileCheckResponseType fileCheckResponseType = FileUploadHelper.CheckPDF(Request.Files[0]);
                //if (fileCheckResponseType != FileCheckResponseType.OK)
                //{
                //    string errorMessage = "";
                //    if (fileCheckResponseType == FileCheckResponseType.TypeError)
                //        errorMessage = "Stadionun sənədi pdf formatında olmalıdır.";
                //    else
                //        errorMessage = "Stadionun sənədi 5Mb-dən çox olmamalıdır";

                //    return Json(new { ok = false, error = errorMessage });
                //}


                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + Request.Files[0].FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + Request.Files[0].FileName);
                Request.Files[0].SaveAs(filePath);

                clubDocument.FilePath = fileUrl.Replace("\\", "/");
            }

            clubDocument.SeasonId = SeasonHelper.GetCurrentSeasonId();
            clubDocument.Year = DateTime.Now.Year;
            clubDocument.ClubId = _user.UserClubId.Value;
            clubDocument.ClubDocumenNameId = documentNameId;
            clubDocument.ClubConfirm = true;
            clubDocument.ClubConfirmById = _user.UserId;
            clubDocument.ClubConfirmDate = DateTime.Now;
            clubDocument.OperatorConfirm = false;
            clubDocument.Rejected = false;

            _db.SaveChanges();


            return Json(new { ok = true, Id = clubDocument.Id, DocumentNameId = documentNameId });
        }

    }
}