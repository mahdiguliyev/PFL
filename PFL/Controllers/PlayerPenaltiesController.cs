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
    [CustomAuthorize(Roles = "admin")]
    public class PlayerPenaltiesController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public PlayerPenaltiesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Kart limit cərimələri";
            ViewBag.BaseUrl = "PlayerPenalties";
        }
        // GET: Districts
        public ActionResult Index(int? page, string playerName, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var playerPenaltiesQuery = _db.VPlayerPenalties.Where(x => x.SeasonId == seasonId);

            if (!string.IsNullOrEmpty(playerName))
            {
                string[] searchKeys = playerName.Split(' ');

                if (searchKeys.Length > 5)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                foreach (var item in searchKeys)
                {

                    playerPenaltiesQuery =
                        playerPenaltiesQuery.Where(x => (x.PlayerFirstName.ToLower() + x.PlayerLastName.ToLower()).Contains(item));
                }
            }

            ViewBag.PlayerNameSearchText = playerName;
            ViewBag.SeasonId = seasonId;

            var playerPenalties = playerPenaltiesQuery.OrderBy(x => x.AdminReview).ThenBy(x => x.Payed).ToPagedList(pageIndex, pageSize);

            return View(playerPenalties);
        }


        [HttpGet]
        public ActionResult CustomCreate()
        {
            //ViewBag.TournamentId = _db.Tournaments.Where(x => !x.IsDeleted && x.Completed != null && !x.Completed.Value)
            //    .OrderByDescending(x => x.SeasonStartYear);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomCreate([Bind(Include = "MatchClubPlayerId,PenaltyReason,PassMatchCount,PenaltyPriceAmount,AdminReview")] PlayerTournamentPenalty playerTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                playerTournamentPenalty.CreatedById = _user.UserId;
                playerTournamentPenalty.CreationDate = DateTime.Now;
                playerTournamentPenalty.IsManual = true;

                _db.PlayerTournamentPenalties.Add(playerTournamentPenalty);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(playerTournamentPenalty);
        }


        // GET: Districts/Edit/5
        public ActionResult CustomEdit(int id)
        {
            PlayerTournamentPenalty playerTournamentPenalty = _db.PlayerTournamentPenalties
                                                                    .Include(x => x.MatchClubPlayer)
                                                                    .Include(x => x.MatchClubPlayer.Match)
                                                                    .Include(x => x.MatchClubPlayer.Match.HomeClub)
                                                                    .Include(x => x.MatchClubPlayer.Match.GuestClub)
                                                                    .Include(x => x.MatchClubPlayer.Player)
                                                                    .FirstOrDefault(x => x.Id == id);
            if (playerTournamentPenalty == null)
            {
                return HttpNotFound();
            }

            if (!(!playerTournamentPenalty.AdminReview && playerTournamentPenalty.IsManual))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View("CustomCreate", playerTournamentPenalty);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            PlayerTournamentPenalty playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == id);
            if (playerTournamentPenalty == null)
            {
                return HttpNotFound();
            }

            if (!(!playerTournamentPenalty.AdminReview && !playerTournamentPenalty.IsManual))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(playerTournamentPenalty);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomEdit([Bind(Include = "Id,MatchClubPlayerId,PenaltyReason,PassMatchCount,PenaltyPriceAmount,AdminReview")] PlayerTournamentPenalty playerTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                PlayerTournamentPenalty _playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == playerTournamentPenalty.Id);
                if (_playerTournamentPenalty == null)
                {
                    return HttpNotFound();
                }

                if (!(!_playerTournamentPenalty.AdminReview && _playerTournamentPenalty.IsManual))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                _playerTournamentPenalty.MatchClubPlayerId = playerTournamentPenalty.MatchClubPlayerId;
                _playerTournamentPenalty.PenaltyReason = playerTournamentPenalty.PenaltyReason;
                _playerTournamentPenalty.PassMatchCount = playerTournamentPenalty.PassMatchCount;
                _playerTournamentPenalty.PenaltyPriceAmount = playerTournamentPenalty.PenaltyPriceAmount;
                _playerTournamentPenalty.AdminReview = playerTournamentPenalty.AdminReview;
                _playerTournamentPenalty.IsManual = true;
                _playerTournamentPenalty.LastUpdateById = _user.UserId;
                _playerTournamentPenalty.LastUpdatedDate = DateTime.Now;
                _db.Entry(_playerTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("CustomCreate", playerTournamentPenalty);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PassMatchCount,PenaltyPriceAmount,AdminReview")] PlayerTournamentPenalty playerTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                PlayerTournamentPenalty _playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == playerTournamentPenalty.Id);
                if (_playerTournamentPenalty == null)
                {
                    return HttpNotFound();
                }

                if (!(!_playerTournamentPenalty.AdminReview && !_playerTournamentPenalty.IsManual))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                _playerTournamentPenalty.PassMatchCount = playerTournamentPenalty.PassMatchCount;
                _playerTournamentPenalty.PenaltyPriceAmount = playerTournamentPenalty.PenaltyPriceAmount;
                _playerTournamentPenalty.AdminReview = playerTournamentPenalty.AdminReview;
                _playerTournamentPenalty.LastUpdateById = _user.UserId;
                _playerTournamentPenalty.LastUpdatedDate = DateTime.Now;
                _db.Entry(_playerTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(playerTournamentPenalty);
        }

        [HttpGet]
        public ActionResult SetAsPayed(int id)
        {
            PlayerTournamentPenalty playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == id);
            if (playerTournamentPenalty == null || !playerTournamentPenalty.AdminReview)
            {
                return HttpNotFound();
            }

            playerTournamentPenalty.LastUpdateById = _user.UserId;
            playerTournamentPenalty.LastUpdatedDate = DateTime.Now;
            playerTournamentPenalty.Payed = true;
            _db.SaveChanges();


            return RedirectToAction("Index");
        }




        [HttpGet]
        public ActionResult Delete(int id)
        {
            var playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == id);

            if (playerTournamentPenalty != null && playerTournamentPenalty.IsManual)
            {
                playerTournamentPenalty.LastUpdateById = _user.UserId;
                playerTournamentPenalty.LastUpdatedDate = DateTime.Now;
                playerTournamentPenalty.IsDeleted = true;
                _db.Entry(playerTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        #region Edit after confirm

        [HttpGet]
        public ActionResult EditAfterConfirm(int id)
        {
            PenaltyEditAfterConfirmDto dto = _db.PlayerTournamentPenalties
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new PenaltyEditAfterConfirmDto()
                {
                    Id = x.Id,
                    PassMatchCount = x.PassMatchCount,
                    PenaltyPriceAmount = x.PenaltyPriceAmount
                })
                .FirstOrDefault();


            return View(dto);
        }


        [HttpPost]
        public ActionResult EditAfterConfirm(PenaltyEditAfterConfirmDto request)
        {

            PlayerTournamentPenalty playerTournamentPenalty =
                _db.PlayerTournamentPenalties.FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

            if (playerTournamentPenalty == null)
                return new HttpNotFoundResult();


            playerTournamentPenalty.PassMatchCount = request.PassMatchCount;
            playerTournamentPenalty.PenaltyPriceAmount = request.PenaltyPriceAmount;
            playerTournamentPenalty.LastUpdateById = _user.UserId;
            playerTournamentPenalty.LastUpdatedDate = DateTime.Now;

            _db.SaveChanges();


            return RedirectToAction("Index");
        }


        #endregion



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
