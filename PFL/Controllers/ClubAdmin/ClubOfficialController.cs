using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;

using PFL.Utils;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    public class ClubOfficialController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public ClubOfficialController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klub rəsmiləri";
            ViewBag.BaseUrl = "ClubOfficial";
        }
        // GET: Districts
        public ActionResult Index(int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();


            var clubOfficials = _db.ClubOfficials
                .Include(x => x.Club)
                .Include(x => x.Position)
                .Where(x => !x.IsDeleted && x.ClubId == _user.UserClubId && x.SeasonId == seasonId)
                .OrderBy(x => x.Club.Name)
                .ThenBy(x => x.FatherName)
                .ThenBy(x => x.FatherName)
                .ThenBy(x => x.FatherName)
                .ToPagedList(pageIndex, pageSize);


            ViewBag.SeasonId = seasonId;

            return View(clubOfficials);
        }


        // GET: Districts/Create
        public ActionResult Create()
        {
            ViewBag.PositionId = new SelectList(_db.OfficialPositions, "Id", "Name");

            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClubOfficialDto clubOfficialDto)
        {
            if (ModelState.IsValid)
            {
                if (!_user.UserClubId.HasValue)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ClubOfficial clubOfficial = Mapper.Map<ClubOfficial>(clubOfficialDto);
                clubOfficial.ClubId = _user.UserClubId.Value;
                clubOfficial.SeasonId = SeasonHelper.GetCurrentSeasonId();

                var uploadDir = "/Files/OfficialDocuments/";

                if (clubOfficialDto.PhotoUpload != null && clubOfficialDto.PhotoUpload.ContentLength > 0)
                {
                    //FileCheckResponseType fileCheckResponseType = FileUploadHelper.CheckPDF(playerRequest.PlayerPhotoUpload);
                    //if (fileCheckResponseType != FileCheckResponseType.OK)
                    //{
                    //    if (fileCheckResponseType == FileCheckResponseType.TypeError)
                    //        ViewBag.ErrorMessage = "Sənədi pdf formatında olmalıdır.";
                    //    else
                    //        ViewBag.ErrorMessage = "Sənədi 25Mb-dən çox olmamalıdır";

                    //    return View(playerRequest);
                    //}

                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.PhotoUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.PhotoUpload.FileName);
                    clubOfficialDto.PhotoUpload.SaveAs(filePath);
                    clubOfficial.PhotoUrl = fileUrl.Replace("\\", "/");
                }

                if (clubOfficialDto.PassportUpload != null && clubOfficialDto.PassportUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.PassportUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.PassportUpload.FileName);
                    clubOfficialDto.PassportUpload.SaveAs(filePath);
                    clubOfficial.PassportUrl = fileUrl.Replace("\\", "/");
                }

                if (clubOfficialDto.ContractUpload != null && clubOfficialDto.ContractUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.ContractUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.ContractUpload.FileName);
                    clubOfficialDto.ContractUpload.SaveAs(filePath);
                    clubOfficial.ContractUrl = fileUrl.Replace("\\", "/");
                }

                if (clubOfficialDto.TrainerLicenseUpload != null && clubOfficialDto.TrainerLicenseUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.TrainerLicenseUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.TrainerLicenseUpload.FileName);
                    clubOfficialDto.TrainerLicenseUpload.SaveAs(filePath);
                    clubOfficial.TrainerLicenseUrl = fileUrl.Replace("\\", "/");
                }

                if (clubOfficialDto.DoctorDiplomaUpload != null && clubOfficialDto.DoctorDiplomaUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.DoctorDiplomaUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.DoctorDiplomaUpload.FileName);
                    clubOfficialDto.DoctorDiplomaUpload.SaveAs(filePath);
                    clubOfficial.DoctorDiplomaUrl = fileUrl.Replace("\\", "/");
                }

                if (clubOfficial.ClubConfirm)
                {
                    clubOfficial.ClubConfirmById = _user.UserId;
                    clubOfficial.ClubConfirmDate = DateTime.Now;

                    clubOfficial.OperatorConfirm = false;
                    clubOfficial.Rejected = false;
                }



                clubOfficial.CreatedById = _user.UserId;
                clubOfficial.CreationDate = DateTime.Now;
                _db.ClubOfficials.Add(clubOfficial);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PositionId = new SelectList(_db.OfficialPositions, "Id", "Name");
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name");

            return View(clubOfficialDto);
        }



        // GET: Districts/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ClubOfficial clubOfficial = _db.ClubOfficials.Include(x => x.Position).FirstOrDefault(x => x.Id == id && x.ClubId == _user.UserClubId);

            if (clubOfficial == null)
            {
                return HttpNotFound();
            }

            ClubOfficialDto clubOfficialDto = Mapper.Map<ClubOfficialDto>(clubOfficial);
            //clubOfficialDto.PositionName = clubOfficial.Position.Name;


            ViewBag.PositionId = new SelectList(_db.OfficialPositions, "Id", "Name", clubOfficialDto.PositionId);

            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name)/*.Where(x => x.Id == clubOfficialDto.CitizenshipId)*/, "Id", "Name", clubOfficialDto.CitizenshipId);

            var requestRejections = _db.ClubOfficialRejections.Where(x => x.ClubOfficialId == id).OrderByDescending(x => x.CreationDate).ToList();
            ViewBag.RequestRejections = requestRejections;




            return View("Create", clubOfficialDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClubOfficialDto clubOfficialDto)
        {
            //if (ModelState.IsValid)
            //{
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            ClubOfficial clubOfficial = _db.ClubOfficials.FirstOrDefault(x => x.Id == clubOfficialDto.Id && x.ClubId == _user.UserClubId);
            if (clubOfficial == null)
            {
                return HttpNotFound();
            }

            //clubOfficial = Mapper.Map(clubOfficialDto, clubOfficial);

            clubOfficial.ClubId = _user.UserClubId.Value;
            clubOfficial.FirstName = clubOfficialDto.FirstName;
            clubOfficial.LastName = clubOfficialDto.LastName;
            clubOfficial.FatherName = clubOfficialDto.FatherName;
            clubOfficial.PositionId = clubOfficialDto.PositionId;
            clubOfficial.BirthDate = clubOfficialDto.BirthDate;
            clubOfficial.CitizenshipId = clubOfficialDto.CitizenshipId;
            clubOfficial.Contact = clubOfficialDto.Contact;
            clubOfficial.ContractBeginDate = clubOfficialDto.ContractBeginDate;
            clubOfficial.ContractEndDate = clubOfficialDto.ContractEndDate;
            clubOfficial.ClubConfirm = clubOfficialDto.ClubConfirm;

            if (clubOfficial.ClubConfirm)
            {
                clubOfficial.ClubConfirmById = _user.UserId;
                clubOfficial.ClubConfirmDate = DateTime.Now;

                clubOfficial.OperatorConfirm = false;
                clubOfficial.Rejected = false;
            }



            var uploadDir = "/Files/OfficialDocuments/";

            if (clubOfficialDto.PhotoUpload != null && clubOfficialDto.PhotoUpload.ContentLength > 0)
            {
                //FileCheckResponseType fileCheckResponseType = FileUploadHelper.CheckPDF(playerRequest.PlayerPhotoUpload);
                //if (fileCheckResponseType != FileCheckResponseType.OK)
                //{
                //    if (fileCheckResponseType == FileCheckResponseType.TypeError)
                //        ViewBag.ErrorMessage = "Sənədi pdf formatında olmalıdır.";
                //    else
                //        ViewBag.ErrorMessage = "Sənədi 25Mb-dən çox olmamalıdır";

                //    return View(playerRequest);
                //}

                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.PhotoUpload.FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.PhotoUpload.FileName);
                clubOfficialDto.PhotoUpload.SaveAs(filePath);
                clubOfficial.PhotoUrl = fileUrl.Replace("\\", "/");
            }

            if (clubOfficialDto.PassportUpload != null && clubOfficialDto.PassportUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.PassportUpload.FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.PassportUpload.FileName);
                clubOfficialDto.PassportUpload.SaveAs(filePath);
                clubOfficial.PassportUrl = fileUrl.Replace("\\", "/");
            }

            if (clubOfficialDto.ContractUpload != null && clubOfficialDto.ContractUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.ContractUpload.FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.ContractUpload.FileName);
                clubOfficialDto.ContractUpload.SaveAs(filePath);
                clubOfficial.ContractUrl = fileUrl.Replace("\\", "/");
            }

            if (clubOfficialDto.TrainerLicenseUpload != null && clubOfficialDto.TrainerLicenseUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.TrainerLicenseUpload.FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.TrainerLicenseUpload.FileName);
                clubOfficialDto.TrainerLicenseUpload.SaveAs(filePath);
                clubOfficial.TrainerLicenseUrl = fileUrl.Replace("\\", "/");
            }

            if (clubOfficialDto.DoctorDiplomaUpload != null && clubOfficialDto.DoctorDiplomaUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubOfficialDto.DoctorDiplomaUpload.FileName);
                var fileUrl = Path.Combine(uploadDir, fileGuid + clubOfficialDto.DoctorDiplomaUpload.FileName);
                clubOfficialDto.DoctorDiplomaUpload.SaveAs(filePath);
                clubOfficial.DoctorDiplomaUrl = fileUrl.Replace("\\", "/");
            }




            clubOfficial.LastUpdateById = _user.UserId;
            clubOfficial.LastUpdatedDate = DateTime.Now;
            _db.Entry(clubOfficial).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
            //}

            //ViewBag.PositionId = new SelectList(_db.OfficialPositions/*.Where(x=>x.Id == clubOfficialDto.PositionId)*/, "Id", "Name", clubOfficialDto.PositionId);

            //ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x=>x.Sort).ThenBy(x=>x.Name)/*.Where(x => x.Id == clubOfficialDto.CitizenshipId)*/, "Id", "Name", clubOfficialDto.CitizenshipId);

            //return View("Create", clubOfficialDto);
        }



        // POST: TournamentTypes/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ClubOfficial clubOfficial = _db.ClubOfficials.FirstOrDefault(x => x.Id == id && x.ClubId == _user.UserClubId);

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
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ClubOfficial clubOfficial = _db.ClubOfficials.FirstOrDefault(x => x.Id == id && x.ClubId == _user.UserClubId);

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
