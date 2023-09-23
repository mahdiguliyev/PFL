using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;
using PFL.Utils;
using Rotativa.Options;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class PlayerOrdersController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public PlayerOrdersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Sifariş vərəqləri";
            ViewBag.BaseUrl = "PlayerOrders";
        }

        public ActionResult Index(int clubId, int? seasonId)
        {

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            Club club = _db.Clubs.FirstOrDefault(x => x.Id == clubId);

            if (club == null)
                return new HttpNotFoundResult();

            ClubTournamentType clubTournamentType = _db.ClubTournamentTypes.FirstOrDefault(x => x.ClubId == club.Id);

            if (clubTournamentType == null)
                return new HttpNotFoundResult();

            ViewBag.ClubTournamentTypeId = clubTournamentType.TournamentTypeId;
            ViewBag.SeasonId = seasonId;

            return View(club);
        }

        public ActionResult Players(int clubTypeId, int clubId, int? seasonId)
        {
            int year = DateTime.Now.Year;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            //var players = _db.ClubPlayerOrders
            //    .Include(x => x.Player)
            //    .Where(x => !x.IsDeleted && x.ClubId == clubId && x.Year == year && x.ClubTypeId == clubTypeId && !x.IsTerminated)
            //    .OrderByDescending(x => x.ClubConfirm)
            //    .ThenBy(x => x.Player.FirstName).ThenBy(x => x.Player.LastName).ThenBy(x => x.Player.FatherName);

            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter clubTypeIdParam = new SqlParameter("clubTypeId", clubTypeId);
            SqlParameter clubViewParam = new SqlParameter("clubView", "0");
            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            var players =
                _db.Database.SqlQuery<ClubPlayerOrderListDto>("ClubPlayerOrdersList @clubId, @clubTypeId, @seasonId, @clubView", clubIdParam, clubTypeIdParam, seasonIdParam, clubViewParam).ToList();

            ViewBag.SeasonId = seasonId;
            ViewBag.ClubTypeId = clubTypeId;

            return View(players);
        }

        public ActionResult PlayersPdf(int clubTypeId, int clubId, int? seasonId)
        {
            int year = DateTime.Now.Year;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            //var players = _db.ClubPlayerOrders
            //    .Include(x => x.Player)
            //    .Where(x => !x.IsDeleted && x.ClubId == clubId && x.Year == year && x.ClubTypeId == clubTypeId && !x.IsTerminated)
            //    .OrderByDescending(x => x.ClubConfirm)
            //    .ThenBy(x => x.Player.FirstName).ThenBy(x => x.Player.LastName).ThenBy(x => x.Player.FatherName);

            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter clubTypeIdParam = new SqlParameter("clubTypeId", clubTypeId);
            SqlParameter clubViewParam = new SqlParameter("clubView", "0");
            SqlParameter seasonIdParam = new SqlParameter("seasonId", seasonId);
            var players =
                _db.Database.SqlQuery<ClubPlayerOrderListDto>("ClubPlayerOrdersList @clubId, @clubTypeId, @seasonId, @clubView", clubIdParam, clubTypeIdParam, seasonIdParam, clubViewParam).ToList();

            var club = _db.Clubs.FirstOrDefault(x => x.Id == clubId);

            ViewBag.SeasonId = seasonId;

            return new Rotativa.PartialViewAsPdf(players)
            {
                PageSize = Size.A4,
                FileName = club?.Name.Replace(" ", "_") + (clubTypeId == 1 ? "_A_Komandasi_Futbolculari" : "_B_Komandasi_Futbolculari") + ".pdf"
            };
        }

        public ActionResult Row(int id)
        {
            int year = DateTime.Now.Year;

            var player = _db.ClubPlayerOrders
                .Include(x => x.Player)
                .FirstOrDefault(x => x.Id == id && x.Year == year && !x.IsDeleted);


            return View(player);
        }


        [HttpGet]
        public ActionResult Confirmation(int id, bool confirm)
        {
            var playerOrder = _db.ClubPlayerOrders.Include(x => x.Player).FirstOrDefault(x => x.Id == id);
            ViewBag.confirm = confirm;

            return View(playerOrder);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Confirmation(OperatorConfirmationDto operatorConfirmation)
        {

            var clubPlayerOrder = _db.ClubPlayerOrders.FirstOrDefault(x => x.Id == operatorConfirmation.Id);

            if (clubPlayerOrder == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (operatorConfirmation.OperatorConfirm)
            {
                clubPlayerOrder.OperatorConfirm = true;
                clubPlayerOrder.Rejected = false;
                clubPlayerOrder.OperatorConfirmById = _user.UserId;
                clubPlayerOrder.OperatorConfirmDate = DateTime.Now;
                clubPlayerOrder.LastUpdateById = _user.UserId;
                clubPlayerOrder.LastUpdatedDate = DateTime.Now;
            }
            else
            {
                clubPlayerOrder.OperatorConfirm = false;
                clubPlayerOrder.Rejected = true;
                clubPlayerOrder.ClubConfirm = false;
                clubPlayerOrder.LastUpdateById = _user.UserId;
                clubPlayerOrder.LastUpdatedDate = DateTime.Now;

                var playerOrderRejection = new ClubPlayerOrderRejection()
                {
                    ClubPlayerOrderId = operatorConfirmation.Id,
                    RejectionNote = operatorConfirmation.RejectionNote,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.ClubPlayerOrderRejections.Add(playerOrderRejection);
            }

            _db.SaveChanges();


            //return RedirectToAction("PlayerRequests");

            return Json(new { ok = true, Id = clubPlayerOrder.Id });
        }



        [HttpGet]
        public ActionResult EditAfterConfirm(int id)
        {
            var playerOrder = _db.ClubPlayerOrders
                .Where(x => x.Id == id)
                .Select(x => new EditAfterConfirmDto()
                {
                    PlayerOrderId = x.Id,
                    PlayerNumber = x.PlayerNumber
                })
                .FirstOrDefault();

            if (playerOrder == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);



            return View(playerOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAfterConfirm(EditAfterConfirmDto afterConfirmDto)
        {
            var playerOrder = _db.ClubPlayerOrders.FirstOrDefault(x => x.Id == afterConfirmDto.PlayerOrderId);

            if (playerOrder == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            playerOrder.PlayerNumber = afterConfirmDto.PlayerNumber;
            playerOrder.LastUpdateById = _user.UserId;
            playerOrder.LastUpdatedDate = DateTime.Now;
            _db.SaveChanges();

            return Json(new { ok = true, Id = playerOrder.Id });
        }




        //// GET: TournamentTypes
        //public ActionResult Detail(int? page, int? tournamentId, int? clubId, int? playerId)
        //{
        //    int pageIndex = page ?? 1;
        //    int pageSize = Constants.PageSize;

        //    var clubTournamentPlayers = _db.ClubTournamentPlayers
        //        .Include(x=>x.Club)
        //        .Include(x=>x.Tournament)
        //        .Include(x=>x.Player)
        //        .Where(x=>!x.IsDeleted)
        //        .OrderByDescending(x=>x.TournamentId)
        //        .ToPagedList(pageIndex, pageSize);

        //    return View(clubTournamentPlayers);
        //}

        // GET: TournamentTypes/Create
        //public ActionResult Create(int clubId, int tournamentId, int clubTypeId)
        //{
        //    ViewBag.ClubId = clubId;
        //    ViewBag.TournamentId = tournamentId;
        //    ViewBag.ClubTypeId = clubTypeId;

        //    return View();
        //}




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(ClubPlayerOrderDto clubPlayerOrderDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int year = DateTime.Now.Year;


        //        var alreadyExist = _db.ClubPlayerOrders.Any(x =>
        //            !x.IsDeleted && x.ClubId == clubPlayerOrderDto.ClubId &&
        //            x.Year == year &&
        //            x.PlayerId == clubPlayerOrderDto.PlayerId);

        //        if (alreadyExist)
        //        return Json(new { ok = false, error = "Eyni oyunçu iki dəfə daxil edilə bilməz!" },
        //                JsonRequestBehavior.AllowGet);

        //        ClubPlayerOrder clubTournamentPlayer =
        //            Mapper.Map<ClubPlayerOrderDto, ClubPlayerOrder>(clubPlayerOrderDto);

        //        clubTournamentPlayer.CreatedById = _user.UserId;
        //        clubTournamentPlayer.CreationDate = DateTime.Now;
        //        _db.ClubPlayerOrders.Add(clubTournamentPlayer);
        //        _db.SaveChanges();

        //        return Json(new { ok = true});
        //    }
        //    return Json(new { ok = false, error = "Model düzgün göndərilməyib." },
        //        JsonRequestBehavior.AllowGet);
        //}


        // POST: TournamentTypes/Undo/5
        //[HttpDelete, ActionName("Delete")]
        //public ActionResult Delete(int id)
        //{
        //    ClubPlayerOrder clubPlayerOrder = _db.ClubPlayerOrders.Find(id);
        //    if (clubPlayerOrder != null)
        //    {
        //        clubPlayerOrder.LastUpdateById = _user.UserId;
        //        clubPlayerOrder.LastUpdatedDate = DateTime.Now;
        //        clubPlayerOrder.IsDeleted = true;
        //        _db.Entry(clubPlayerOrder).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return Json(new { ok = true});
        //    }


        //    return Json(new { ok = false, error = "ID doğru deyil!" });
        //}

        [HttpGet]
        public ActionResult TerminateOrder(int id)
        {
            var terminationDto = _db.ClubPlayerOrders
                .Include(x => x.Player)
                .Include(x => x.Club)
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new PlayerTerminationDto()
                {
                    PlayerOrderId = x.Id,
                    FirstName = x.Player.FirstName,
                    LastName = x.Player.LastName,
                    FatherName = x.Player.FatherName,
                    ClubName = x.Club.Name
                })
                .FirstOrDefault();

            if (terminationDto == null)
            {
                return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);
            }

            return View(terminationDto);
        }


        //TODO check termination date is if between contract date, in both front and back side
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult TerminateOrder(PlayerTerminationDto terminationDto)
        {
            var playerOrder = _db.ClubPlayerOrders.FirstOrDefault(x =>
                x.Id == terminationDto.PlayerOrderId && !x.IsDeleted);

            if (playerOrder == null)
                return Json(new { ok = false, error = "Səhv müraciət! .01" }, JsonRequestBehavior.AllowGet);

            var transfer = _db.Transfers
                .Where(x => x.PlayerId == playerOrder.PlayerId && x.ClubToId == playerOrder.ClubId && !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (transfer == null)
                return Json(new { ok = false, error = "Səhv müraciət! .02" }, JsonRequestBehavior.AllowGet);

            transfer.TerminationDate = terminationDto.TerminationDate;
            transfer.LastUpdateById = _user.UserId;
            transfer.LastUpdatedDate = DateTime.Now;

            _db.SaveChanges();

            return Json(new { ok = true, Id = playerOrder.Id, Terminated = transfer.TerminationDate < DateTime.Now, TerminationDateStr = transfer.TerminationDate?.ToString("dd.MM.yyyy") });
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
