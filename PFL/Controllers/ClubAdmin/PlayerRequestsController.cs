using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFL.Membership;
using PFL.Models.DTO;

using PFL.Utils;
using AutoMapper;
using PagedList;
using PFL.Entities.EntityModels;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    public class PlayerRequestsController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public PlayerRequestsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Yeni oyunçu sifarişi";
            ViewBag.BaseUrl = "PlayerRequests";
        }


        // GET: PlayerRequests
        [HttpGet]
        public ActionResult Index(string playerName, int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;
            


            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();


            var clubPlayerRequestQuery = _db.ClubPlayerRequests
                .Include(x => x.FromClub)
                .Where(x => x.RequestClubId == _user.UserClubId && !x.IsDeleted && x.SeasonId==seasonId);


            if (!string.IsNullOrEmpty(playerName))
            {
                string[] searchKeys = playerName.Split(' ');

                if (searchKeys.Length > 5)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                foreach (var item in searchKeys)
                {

                    clubPlayerRequestQuery =
                        clubPlayerRequestQuery.Where(x => (x.PlayerFirstName.ToLower() + x.PlayerLastName.ToLower() + x.PlayerFatherName.ToLower() + x.PlayerNumber).Contains(item));
                }
            }


            var clubPlayerRequest = clubPlayerRequestQuery.OrderByDescending(x => x.Id).ToPagedList(pageIndex, pageSize);





            ViewBag.PlayerNameSearchText = playerName;
            ViewBag.SeasonId = seasonId;


            return View(clubPlayerRequest);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var clubPlayerRequest = _db.ClubPlayerRequests
                .Include(x => x.RequestClub)
                .Include(x => x.FromClub)
                .Include(x => x.Country)
                .Include(x => x.ContractType)
                .Include(x => x.Position)
                .Where(x => x.RequestClubId == _user.UserClubId)
                .FirstOrDefault(x => x.Id == id);

            if (clubPlayerRequest == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var requestRejections = _db.ClubPlayerRequestRejections.Where(x => x.ClubPlayerRequestId == id).OrderByDescending(x => x.CreationDate).ToList();

            ViewBag.RequestRejections = requestRejections;



            return View(clubPlayerRequest);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult Create()
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var defaulSelection = new SelectListItem { Text = "Seçim edin", Value = "0" };
            //var fromClubId = new SelectList(_db.Clubs, "Id", "Name");


            //ViewBag.FromClubId = fromClubId;
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name", 0, "Seçim edin");
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes.Where(x => x.Id != 2), "Id", "Name", 0, "Seçim edin");
            ViewBag.PositionId = new SelectList(_db.Positions, "Id", "Name", 0, "Seçim edin");
            ViewBag.Action = "Create";


            var toClub = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);
            if (toClub == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.ToClubName = toClub.Name;

            return View();
        }



        [HttpPost]
        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult Create(PlayerRequestDto playerRequest)
        {


            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                ClubPlayerRequest clubPlayerRequest = Mapper.Map<PlayerRequestDto, ClubPlayerRequest>(playerRequest);

                clubPlayerRequest.SeasonId = SeasonHelper.GetCurrentSeasonId();

                var uploadDir = "/Files/RequestDocuments/";

                if (playerRequest.PlayerPhotoUpload != null && playerRequest.PlayerPhotoUpload.ContentLength > 0)
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
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerPhotoUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerPhotoUpload.FileName);
                    playerRequest.PlayerPhotoUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerPhotoUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerPasportUpload != null && playerRequest.PlayerPasportUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerPasportUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerPasportUpload.FileName);
                    playerRequest.PlayerPasportUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerPasportUrl = fileUrl.Replace("\\", "/");
                }


                if (playerRequest.PlayerContractUpload != null && playerRequest.PlayerContractUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerContractUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerContractUpload.FileName);
                    playerRequest.PlayerContractUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerContractUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerOtkripleniyaUpload != null && playerRequest.PlayerOtkripleniyaUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerOtkripleniyaUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerOtkripleniyaUpload.FileName);
                    playerRequest.PlayerOtkripleniyaUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerOtkripleniyaUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerTmsUpload != null && playerRequest.PlayerTmsUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerTmsUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerTmsUpload.FileName);
                    playerRequest.PlayerTmsUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerTmsUrl = fileUrl.Replace("\\", "/");
                }


                if (!string.IsNullOrEmpty(clubPlayerRequest.FromClubName) || clubPlayerRequest.FromClubId == 0)
                {
                    clubPlayerRequest.FromClubId = null;
                }


                clubPlayerRequest.OperatorConfirm = false;
                clubPlayerRequest.Rejected = false;
                clubPlayerRequest.RequestClubId = _user.UserClubId.Value;
                clubPlayerRequest.CreatedById = _user.UserId;
                clubPlayerRequest.CreationDate = DateTime.Now;


                _db.ClubPlayerRequests.Add(clubPlayerRequest);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerRequest.FromClubId);
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name", playerRequest.CitizenshipId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes.Where(x => x.Id != 2), "Id", "Name", playerRequest.ContractTypeId);
            ViewBag.PositionId = new SelectList(_db.Positions, "Id", "Name", playerRequest.PositionId);

            Club club = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);
            if (club != null)
            {
                ViewBag.ToClubName = club.Name;
            }


            return View(playerRequest);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult Edit(int id)
        {
            var clubPlayerRequest = _db.ClubPlayerRequests.FirstOrDefault(x => x.Id == id);

            if (clubPlayerRequest == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var playerRequest = Mapper.Map<ClubPlayerRequest, PlayerRequestEditDto>(clubPlayerRequest);

            ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerRequest.FromClubId ?? null);
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name", playerRequest.CitizenshipId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes.Where(x => x.Id != 2), "Id", "Name", playerRequest.ContractTypeId);
            ViewBag.PositionId = new SelectList(_db.Positions, "Id", "Name", playerRequest.PositionId);


            var requestRejections = _db.ClubPlayerRequestRejections.Where(x => x.ClubPlayerRequestId == id).OrderByDescending(x => x.CreationDate).ToList();

            ViewBag.RequestRejections = requestRejections;


            var toClub = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);
            if (toClub == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.ToClubName = toClub.Name;


            ViewBag.Action = "Edit";

            return View(playerRequest);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult Edit(PlayerRequestEditDto playerRequest)
        {

            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);




            if (ModelState.IsValid)
            {
                var clubPlayerRequest = _db.ClubPlayerRequests.FirstOrDefault(x => x.Id == playerRequest.Id);

                if (clubPlayerRequest == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                //clubPlayerRequest = Mapper.Map<ClubPlayerRequest>(playerRequest);
                clubPlayerRequest.PlayerFirstName = playerRequest.PlayerFirstName;
                clubPlayerRequest.PlayerLastName = playerRequest.PlayerLastName;
                clubPlayerRequest.PlayerFatherName = playerRequest.PlayerFatherName;
                clubPlayerRequest.FromClubId = playerRequest.FromClubId;
                clubPlayerRequest.FromClubName = playerRequest.FromClubName;
                clubPlayerRequest.ClubConfirm = playerRequest.ClubConfirm;
                clubPlayerRequest.BirthDate = playerRequest.BirthDate;
                clubPlayerRequest.CitizenshipId = playerRequest.CitizenshipId;
                clubPlayerRequest.ContractTypeId = playerRequest.ContractTypeId;
                clubPlayerRequest.ContractBeginDate = playerRequest.ContractBeginDate;
                clubPlayerRequest.ContractEndDate = playerRequest.ContractEndDate;
                clubPlayerRequest.PlayerNumber = playerRequest.PlayerNumber;
                clubPlayerRequest.PositionId = playerRequest.PositionId;


                var uploadDir = "/Files/RequestDocuments/";

                if (playerRequest.PlayerPhotoUpload != null && playerRequest.PlayerPhotoUpload.ContentLength > 0)
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
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerPhotoUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerPhotoUpload.FileName);
                    playerRequest.PlayerPhotoUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerPhotoUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerPasportUpload != null && playerRequest.PlayerPasportUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerPasportUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerPasportUpload.FileName);
                    playerRequest.PlayerPasportUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerPasportUrl = fileUrl.Replace("\\", "/");
                }


                if (playerRequest.PlayerContractUpload != null && playerRequest.PlayerContractUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerContractUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerContractUpload.FileName);
                    playerRequest.PlayerContractUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerContractUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerOtkripleniyaUpload != null && playerRequest.PlayerOtkripleniyaUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerOtkripleniyaUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerOtkripleniyaUpload.FileName);
                    playerRequest.PlayerOtkripleniyaUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerOtkripleniyaUrl = fileUrl.Replace("\\", "/");
                }

                if (playerRequest.PlayerTmsUpload != null && playerRequest.PlayerTmsUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerRequest.PlayerTmsUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + playerRequest.PlayerTmsUpload.FileName);
                    playerRequest.PlayerTmsUpload.SaveAs(filePath);
                    clubPlayerRequest.PlayerTmsUrl = fileUrl.Replace("\\", "/");
                }
         

                if (!string.IsNullOrEmpty(clubPlayerRequest.FromClubName) || clubPlayerRequest.FromClubId == 0)
                {
                    clubPlayerRequest.FromClubId = null;
                }

                clubPlayerRequest.OperatorConfirm = false;
                clubPlayerRequest.Rejected = false;
                clubPlayerRequest.RequestClubId = _user.UserClubId.Value;
                clubPlayerRequest.CreatedById = _user.UserId;
                clubPlayerRequest.CreationDate = DateTime.Now;


                //_db.Entry(clubPlayerRequest).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerRequest.FromClubId);

            return View(playerRequest);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var playerRequest = _db.ClubPlayerRequests.FirstOrDefault(x => x.Id == id);

            if(playerRequest == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var playerRequestRejections = _db.ClubPlayerRequestRejections.Where(x => x.ClubPlayerRequestId == id);

            foreach (var item in playerRequestRejections)
            {
                item.LastUpdateById = _user.UserId;
                item.LastUpdatedDate = DateTime.Now;
                item.IsDeleted = true;
            }

            playerRequest.LastUpdateById = _user.UserId;
            playerRequest.LastUpdatedDate = DateTime.Now;
            playerRequest.IsDeleted = true;

            _db.SaveChanges();

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