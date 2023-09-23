using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;
using PFL.Utils;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    public class ClubPlayerOrdersController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public ClubPlayerOrdersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Sifariş vərəqi";
            ViewBag.BaseUrl = "ClubPlayerOrders";
        }

        public ActionResult Index(int? seasonId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            Club club = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);

            if (club == null)
                return new HttpNotFoundResult();

            ClubTournamentType clubTournamentType = _db.ClubTournamentTypes.FirstOrDefault(x => x.ClubId == club.Id);

            if(clubTournamentType == null)
                return new HttpNotFoundResult();

            ViewBag.ClubTournamentTypeId = clubTournamentType.TournamentTypeId;
            ViewBag.SeasonId = seasonId;

            return View(club);
        }


        //TODO club_confirm-u parametr kimi procedura oturmek lazimdir. cunki admin terefde club_confirm=1, club terefde ise 0 olmalidir
        public ActionResult Players(int clubTypeId, int? seasonId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();


            //var players = _db.ClubPlayerOrders
            //    .Include(x => x.Player)
            //    .Where(x => !x.IsDeleted && x.ClubId == _user.UserClubId && x.Year == year && x.ClubTypeId == clubTypeId)
            //    .OrderBy(x=>x.Player.FirstName)
            //    .ThenBy(x=>x.Player.LastName);

            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter clubTypeIdParam = new SqlParameter("clubTypeId", clubTypeId);
            SqlParameter clubViewParam = new SqlParameter("clubView", "1");
            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            var players =
                _db.Database.SqlQuery<ClubPlayerOrderListDto>("ClubPlayerOrdersList @clubId, @clubTypeId, @seasonId, @clubView", clubIdParam, clubTypeIdParam, seasonIdParam, clubViewParam).ToList();


            return View(players);
        }

        public ActionResult Row(int playerOrderId)
        {

            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //int year = DateTime.Now.Year;

            var seasonId = SeasonHelper.GetCurrentSeasonId();

            var player = _db.ClubPlayerOrders
                .Include(x => x.Player)
                .FirstOrDefault(x => x.Id == playerOrderId && x.ClubId == _user.UserClubId && x.SeasonId == seasonId && !x.IsDeleted);


            return View(player);
        }

        // GET: TournamentTypes/Create
        public ActionResult Create(int clubTypeId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ClubId = _user.UserClubId;
            ViewBag.ClubTypeId = clubTypeId;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClubPlayerOrderDto clubPlayerOrderDto)
        {

            //if (ModelState.IsValid)
            //{

            clubPlayerOrderDto.ClubConfirm = true;
            int seasonId = SeasonHelper.GetCurrentSeasonId();



            if (clubPlayerOrderDto.PlayerId <= 0)
            {
                return Json(new { ok = false, error = "OYUNÇU SEÇİLMƏYİB" },
                    JsonRequestBehavior.AllowGet);
            }

            if (clubPlayerOrderDto.PlayerNumber <= 0)
            {
                return Json(new { ok = false, error = "OYUNÇUNUN NÖMRƏSİ SEÇİLMƏYİB" },
                    JsonRequestBehavior.AllowGet);
            }




            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int year = DateTime.Now.Year;


            var alreadyExist = _db.ClubPlayerOrders.Any(x =>
                !x.IsDeleted && x.ClubId == _user.UserClubId &&
                //x.Year == year &&
                x.PlayerId == clubPlayerOrderDto.PlayerId &&
                x.SeasonId == seasonId);


            if (alreadyExist)
                return Json(new { ok = false, error = "EYNİ OYUNÇU İKİ DƏFƏ DAXİL EDİLƏ BİLMƏZ!" },
                        JsonRequestBehavior.AllowGet);


            //var alreadyNumberExist = _db.ClubPlayerOrders.Any(x =>
            //    !x.IsDeleted && x.ClubId == _user.UserClubId &&
            //    //x.Year == year &&
            //    x.PlayerNumber == clubPlayerOrderDto.PlayerNumber &&
            //    x.SeasonId == seasonId);


            bool alreadyNumberExist = false;
            var currentDate = DateTime.Now;

            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter playerNumberParam = new SqlParameter("playerNumber", clubPlayerOrderDto.PlayerNumber);
            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            var playerNumbers =
                _db.Database.SqlQuery<ClubPlayerOrderListDto>("ClubPlayerOrdersListItem @clubId, @playerNumber, @seasonId", clubIdParam, playerNumberParam, seasonIdParam).ToList();


            if (playerNumbers.Any(x => (x.TerminationDate > currentDate || x.TerminationDate == null) && x.ContractEndDate > currentDate))
            {
                alreadyNumberExist = true;
            }



            if (alreadyNumberExist)
                return Json(new { ok = false, error = "OYUNÇU NÖMRƏSİ UNİKAL OLMALIDIR!" },
                    JsonRequestBehavior.AllowGet);


            ClubPlayerOrder clubPlayerOrder = new ClubPlayerOrder()
            {
                ClubId = _user.UserClubId.Value,
                PlayerId = clubPlayerOrderDto.PlayerId,
                PlayerNumber = clubPlayerOrderDto.PlayerNumber,
                Year = year,
                ClubTypeId = clubPlayerOrderDto.ClubTypeId,
                ClubConfirm = clubPlayerOrderDto.ClubConfirm
            };

            if (clubPlayerOrderDto.ClubConfirm)
            {
                clubPlayerOrder.ClubConfirmById = _user.UserId;
                clubPlayerOrder.ClubConfirmDate = DateTime.Now;

                if (string.IsNullOrEmpty(clubPlayerOrder.HealthFile) &&
                    clubPlayerOrderDto.HealthFileUpload == null)
                {
                    return Json(new { ok = false, error = "SAĞLAMLIQ ARAYIŞI SƏNƏDİ ƏLAVƏ EDİLMƏYİB." }, JsonRequestBehavior.AllowGet);
                }
            }


            if (clubPlayerOrderDto.HealthFileUpload != null && clubPlayerOrderDto.HealthFileUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var uploadDir = "/Files/Orders/";
                var photoPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubPlayerOrderDto.HealthFileUpload.FileName);
                var photoUrl = Path.Combine(uploadDir, fileGuid + clubPlayerOrderDto.HealthFileUpload.FileName);
                clubPlayerOrderDto.HealthFileUpload.SaveAs(photoPath);
                clubPlayerOrder.HealthFile = photoUrl.Replace("\\", "/");
            }


            clubPlayerOrder.SeasonId = SeasonHelper.GetCurrentSeasonId();

            clubPlayerOrder.OperatorConfirm = false;
            clubPlayerOrder.Rejected = false;


            clubPlayerOrder.CreatedById = _user.UserId;
            clubPlayerOrder.CreationDate = DateTime.Now;
            _db.ClubPlayerOrders.Add(clubPlayerOrder);
            _db.SaveChanges();



            return Json(new { ok = true, Id = clubPlayerOrder.Id });
            //}

            //return Json(new { ok = false, error = "Model düzgün göndərilməyib." },
            //    JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(int playerOrderId)
        {
            ClubPlayerOrder clubPlayerOrder = _db.ClubPlayerOrders.Include(x => x.Player).FirstOrDefault(x => x.Id == playerOrderId);

            if (clubPlayerOrder == null)
                return Json(new { ok = false, error = "ID səhv göndərilib." },
                    JsonRequestBehavior.AllowGet);

            ClubPlayerOrderDto clubPlayerOrderDto = Mapper.Map<ClubPlayerOrder, ClubPlayerOrderDto>(clubPlayerOrder);


            ViewBag.PlayerOrderRejections = _db.ClubPlayerOrderRejections.Where(x => x.ClubPlayerOrderId == playerOrderId).OrderByDescending(x => x.CreationDate).ToList();

            return View("Create", clubPlayerOrderDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClubPlayerOrderDto clubPlayerOrderDto)
        {
            clubPlayerOrderDto.ClubConfirm = true;

            var seasonId = SeasonHelper.GetCurrentSeasonId();

            if (clubPlayerOrderDto.PlayerId <= 0)
            {
                return Json(new { ok = false, error = "OYUNÇU SEÇİLMƏYİB" },
                    JsonRequestBehavior.AllowGet);
            }

            if (clubPlayerOrderDto.PlayerNumber <= 0)
            {
                return Json(new { ok = false, error = "OYUNÇUNUN NÖMRƏSİ SEÇİLMƏYİB" },
                    JsonRequestBehavior.AllowGet);
            }




            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            int year = DateTime.Now.Year;

            var clubPlayerOrder = _db.ClubPlayerOrders.FirstOrDefault(x =>
                x.Id == clubPlayerOrderDto.Id &&
                !x.IsDeleted && x.ClubId == _user.UserClubId &&
                //x.Year == year &&
                x.SeasonId== seasonId &&
                x.PlayerId == clubPlayerOrderDto.PlayerId);


            if (clubPlayerOrder == null)
                return Json(new { ok = false, error = "Müraciət doğru deyil!" }, JsonRequestBehavior.AllowGet);


            //tekrar oyuncu nomresinin yoxlanilmasi
            var alreadyNumberExist = _db.ClubPlayerOrders.Any(x =>
                !x.IsDeleted && x.ClubId == _user.UserClubId &&
                //x.Year == year &&
                x.SeasonId == seasonId &&
                x.PlayerNumber == clubPlayerOrderDto.PlayerNumber && x.PlayerId != clubPlayerOrder.PlayerId);

            if (alreadyNumberExist)
                return Json(new { ok = false, error = "OYUNÇU NÖMRƏSİ UNİKAL OLMALIDIR!" },
                    JsonRequestBehavior.AllowGet);





            clubPlayerOrder.PlayerNumber = clubPlayerOrderDto.PlayerNumber;
            clubPlayerOrder.ClubConfirm = clubPlayerOrderDto.ClubConfirm;


            if (clubPlayerOrderDto.ClubConfirm)
            {
                clubPlayerOrder.ClubConfirmById = _user.UserId;
                clubPlayerOrder.ClubConfirmDate = DateTime.Now;

                if (string.IsNullOrEmpty(clubPlayerOrder.HealthFile) &&
                    clubPlayerOrderDto.HealthFileUpload == null)
                {
                    return Json(new { ok = false, error = "SAĞLAMLIQ ARAYIŞI SƏNƏDİ ƏLAVƏ EDİLMƏYİB." }, JsonRequestBehavior.AllowGet);
                }
            }

            if (clubPlayerOrderDto.HealthFileUpload != null && clubPlayerOrderDto.HealthFileUpload.ContentLength > 0)
            {
                var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                var uploadDir = "/Files/Orders/";
                var photoPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubPlayerOrderDto.HealthFileUpload.FileName);
                var photoUrl = Path.Combine(uploadDir, fileGuid + clubPlayerOrderDto.HealthFileUpload.FileName);
                clubPlayerOrderDto.HealthFileUpload.SaveAs(photoPath);
                clubPlayerOrder.HealthFile = photoUrl.Replace("\\", "/");
            }




            clubPlayerOrder.OperatorConfirm = false;
            clubPlayerOrder.Rejected = false;
            clubPlayerOrder.LastUpdateById = _user.UserId;
            clubPlayerOrder.LastUpdatedDate = DateTime.Now;
            _db.SaveChanges();

            return Json(new { ok = true, Id = clubPlayerOrder.Id });
        }




        // POST: TournamentTypes/Undo/5
        [HttpDelete, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            ClubPlayerOrder clubPlayerOrder = _db.ClubPlayerOrders.Find(id);
            if (clubPlayerOrder != null)
            {
                clubPlayerOrder.LastUpdateById = _user.UserId;
                clubPlayerOrder.LastUpdatedDate = DateTime.Now;
                clubPlayerOrder.IsDeleted = true;
                _db.Entry(clubPlayerOrder).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { ok = true, Id = clubPlayerOrder.Id });
            }


            return Json(new { ok = false, error = "ID doğru deyil!" });
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
