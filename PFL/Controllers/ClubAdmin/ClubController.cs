using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PFL.Membership;

using PFL.Models.ViewModels;
using PFL.Utils;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PagedList;
using PFL.Entities.EntityModels;
using Rotativa;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    [ClubAdminAuthorize]
    public class ClubController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public ClubController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Klublar";
            ViewBag.BaseUrl = "Club";
        }




        // GET: Clubs/Details/5
        public ActionResult Details()
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Club club = _db.Clubs
                .Include(x => x.District)
                .FirstOrDefault(x => x.Id == _user.UserClubId);
            if (club == null)
            {
                return HttpNotFound();
            }

            HttpCookie cookie = new HttpCookie("ClubName");
            cookie.Expires = DateTime.Now.AddDays(2);
            cookie.SetEncodedValue(club.Name);
            Response.Cookies.Add(cookie);

            return View(club);
        }



        // GET: Clubs/Edit/5
        public ActionResult Edit()
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Club club = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);
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
                if (!_user.UserClubId.HasValue)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Club club = _db.Clubs.FirstOrDefault(x => x.Id == _user.UserClubId);
                if (club == null)
                    return HttpNotFound();

                club.Name = clubViewModel.Name;
                club.DistrictId = clubViewModel.DistrictId;
                club.CreationYear = clubViewModel.CreationYear;
                club.Stadium = clubViewModel.Stadium;
                club.StadiumCapacity = clubViewModel.StadiumCapacity;
                club.Address = clubViewModel.Address;
                club.Phone = clubViewModel.Phone;
                club.Fax = clubViewModel.Fax;
                club.Email = clubViewModel.Email;
                club.WebSite = clubViewModel.WebSite;
                club.ClubPresident = clubViewModel.ClubPresident;
                club.Note = clubViewModel.Note;

                if (clubViewModel.LogoUpload != null && clubViewModel.LogoUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var uploadDir = "/Files/Logos/";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubViewModel.LogoUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubViewModel.LogoUpload.FileName);
                    clubViewModel.LogoUpload.SaveAs(filePath);
                    club.LogoUrl = fileUrl.Replace("\\", "/");
                }

                if (clubViewModel.StadiumPhotoUpload != null && clubViewModel.StadiumPhotoUpload.ContentLength > 0)
                {
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var uploadDir = "/Files/Stadiums/";
                    var filePath = Path.Combine(Server.MapPath(uploadDir), fileGuid + clubViewModel.StadiumPhotoUpload.FileName);
                    var fileUrl = Path.Combine(uploadDir, fileGuid + clubViewModel.StadiumPhotoUpload.FileName);
                    clubViewModel.StadiumPhotoUpload.SaveAs(filePath);
                    club.StadiumPhotoUrl = fileUrl.Replace("\\", "/");
                }




                club.LastUpdateById = _user.UserId;
                club.LastUpdatedDate = DateTime.Now;

                _db.Entry(club).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Details");
            }
            ViewBag.DistrictId = new SelectList(_db.Districts.Where(x => !x.IsDeleted), "Id", "Name", clubViewModel.DistrictId);

            return View("Create", clubViewModel);
        }





        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult Matches(int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!_user.UserClubId.HasValue)
                return new EmptyResult();

            //var season = SeasonHelper.GetSeason(seasonId);
            //if(season==null)
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();


            var currentDate = DateTime.Now;

            var tournamentTours = _db.TournamentTours
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub)
                .Include(x => x.Match.GuestClub)
                .Include(x => x.Tournament)
                .Where(x => (x.Match.HomeClubId == _user.UserClubId.Value || x.Match.GuestClubId == _user.UserClubId.Value)

                            && x.Tournament.SeasonId == seasonId
                            //&& ((x.Tournament.SeasonStartYear == currentDate.Year && currentDate.Month > 6) || (x.Tournament.SeasonEndYear == currentDate.Year && currentDate.Month < 7))

                            && x.Match.MatchDate != null)
                .OrderByDescending(x => x.Match.MatchDate)//.ThenByDescending(x => x.Match.MatchDate)
                .ToPagedList(pageIndex, pageSize);

            ViewBag.ClubId = _user.UserClubId.Value;
            ViewBag.SeasonId = seasonId;

            return View(tournamentTours);
        }

        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult MatchParticipants(int matchId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            //bu hissede turnirin adi ve turun nomresi view-ya oturulecek
            //bu hissede player listi ile maçda iştirak edən player listi yoxlanılacaq və view-ya yeni bir list view model qaytarilacaq

            Match match = _db.Matches
                .Include(x => x.HomeClub)
                .Include(x => x.GuestClub)
                .FirstOrDefault(x => x.Id == matchId && (x.HomeClubId == _user.UserClubId || x.GuestClubId == _user.UserClubId));

            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            bool clubMatchConfirm = false;

            if (match.HomeClubId == _user.UserClubId)
            {
                clubMatchConfirm = match.HomeClubConfirm ?? false;
            }
            else
            {
                clubMatchConfirm = match.GuestClubConfirm ?? false;
            }

            ViewBag.ClubMatchConfirm = clubMatchConfirm;

            if (match.HomeClubId == _user.UserClubId)
            {
                ViewBag.ClubExpConfirmStatus = match.HomeClubExpConfirmAllow;
            }
            else
            {
                ViewBag.ClubExpConfirmStatus = match.GuestClubExpConfirmAllow;
            }


            ViewBag.ClubId = _user.UserClubId.Value;


            ViewBag.OpponentClubProtokol = DateTime.Now.AddMinutes(75) >= match.MatchDate &&
                                           (match.HomeClubConfirm != null && match.HomeClubConfirm.Value) &&
                                           (match.GuestClubConfirm != null && match.GuestClubConfirm.Value);


            return View(match);
        }


        //TODO In ClubProtokolPlayers proc also return captain and selected fields
        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult PlayerSelection(int matchId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            Match match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Tournament tournament = (from t in _db.Tournaments
                                     join tt in _db.TournamentTours on t.Id equals tt.TournamentId
                                     where tt.MatchId == matchId
                                     select t).FirstOrDefault();

            if (tournament == null)
                return new HttpNotFoundResult();

            ////var clubPlayerOrders = from cpo in _db.ClubPlayerOrders
            ////                       select cpo;
            ////var matchPlayerSelectionViewModel = clubPlayerOrders.Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted && x.ClubId == _user.UserClubId)
            ////    .Include(x => x.Player)
            ////    //.Include(x => x.Player.Position)
            ////    .Where(x => x.ClubId == _user.UserClubId && !x.IsDeleted && x.ClubConfirm && x.OperatorConfirm && !x.Rejected)
            ////    .Select(x => new MatchPlayerSelectionViewModel()
            ////    {
            ////        ClubTypeId = x.ClubTypeId,
            ////        PlayerId = x.PlayerId,
            ////        PlayerNumber = x.PlayerNumber,
            ////        PlayerFirstName = x.Player.FirstName,
            ////        PlayerLastName = x.Player.LastName,
            ////        PlayerFatherName = x.Player.FatherName,
            ////        PlayerBirthDate = x.Player.BirthDate,
            ////        PositionLabel = x.Player.Position.Label,
            ////        CitizenshipId = x.Player.CitizenshipId,
            ////        Captain = false,
            ////        Selected = false
            ////    }).ToList();


            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter matchIdParam = new SqlParameter("matchId", matchId);
            SqlParameter matchDateParam = new SqlParameter("matchDate", match.MatchDate);
            var matchPlayerSelectionViewModel =
                _db.Database.SqlQuery<MatchPlayerSelectionViewModel>("ClubProtokolPlayers @clubId, @matchId, @matchDate", clubIdParam, matchIdParam, matchDateParam).ToList();





            var matchClubPlayers = _db.MatchClubPlayerShifts.Where(x => !x.IsDeleted && x.MatchId == matchId && x.ClubId == _user.UserClubId);

            foreach (var item in matchClubPlayers)
            {
                var selectedPlayer = matchPlayerSelectionViewModel.FirstOrDefault(x => x.PlayerId == item.PlayerId);

                if (selectedPlayer != null)
                {
                    selectedPlayer.Selected = true;
                    selectedPlayer.MainStaff = item.MinuteIn == 0;
                }
            }

            MatchLimitViewModel limit = new MatchLimitViewModel();


            limit.Limit11Count = _db.MatchClubPlayerShifts.Count(x =>
                !x.IsDeleted
                && x.MatchId == matchId
                && x.ClubId == _user.UserClubId
                && x.MinuteIn == 0);
            limit.Limit11 = limit.Limit11Count == 11;

            limit.LimitLegionerCount = _db.MatchClubPlayerShifts.Count(x =>
                !x.IsDeleted
                && x.MatchId == matchId
                && x.ClubId == _user.UserClubId
                && x.MinuteIn == 0
                && x.Player.CitizenshipId != 16);
            limit.LimitLegioner = limit.LimitLegionerCount == 6;

            DateTime youngDate = new DateTime(tournament.YoungPlayerAge21Limit, 1, 1);

            limit.LimitYoungCount = _db.MatchClubPlayerShifts.Count(x =>
                !x.IsDeleted
                && x.MatchId == matchId
                && x.ClubId == _user.UserClubId
                && x.MinuteIn == 0
                && x.Player.BirthDate > youngDate);
            limit.LimitYoung = limit.LimitYoungCount == 1;


            ViewBag.Limit = limit;
            ViewBag.MatchId = matchId;


            #region Club Captain

            var captainPlayerId = _db.MatchClubPlayers.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == _user.UserClubId &&
                x.Captain == 1).Select(x => x.PlayerId).FirstOrDefault();

            var captain = matchPlayerSelectionViewModel.FirstOrDefault(x => x.PlayerId == captainPlayerId);
            if (captain != null)
            {
                captain.Captain = true;
            }

            #endregion

            ViewBag.YoungPlayerAge21Limit = tournament.YoungPlayerAge21Limit;
            ViewBag.YoungPlayerAge19Limit = tournament.YoungPlayerAge19Limit;

            var sortedMatchPlayers = SortHelper.SortProtokolPlayerList(matchPlayerSelectionViewModel, tournament.YoungPlayerAge21Limit);




            bool clubMatchConfirm = false;

            if (match.HomeClubId == _user.UserClubId)
            {
                clubMatchConfirm = match.HomeClubConfirm ?? false;
            }
            else
            {
                clubMatchConfirm = match.GuestClubConfirm ?? false;
            }

            ViewBag.ClubMatchConfirm = clubMatchConfirm;
            ViewBag.MatchId = match.Id;

            return View(sortedMatchPlayers);
        }

        [HttpGet]
        public ActionResult AddCaptain(int matchId, long playerId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var matchConfirmStatus = ClubMatchConfirmStatus(_user.UserClubId.Value, matchId);
            if (matchConfirmStatus.confirmStatus)
                return Json(new { ok = false, error = matchConfirmStatus.errorMessage });


            var matchPlayer = _db.MatchClubPlayers.FirstOrDefault(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == _user.UserClubId &&
                x.PlayerId == playerId);

            if (matchPlayer == null)
                return Json(new { ok = false, error = "Səhv müraciət." }, JsonRequestBehavior.AllowGet);

            if (matchPlayer.Captain == 1)
            {
                matchPlayer.Captain = 0;
            }
            else
            {
                #region removing old captain

                var matchClubCaptain = _db.MatchClubPlayers.FirstOrDefault(x =>
                    !x.IsDeleted &&
                    x.MatchId == matchId &&
                    x.ClubId == _user.UserClubId &&
                    x.Captain == 1);

                if (matchClubCaptain != null)
                    matchClubCaptain.Captain = 0;

                _db.SaveChanges();

                #endregion

                matchPlayer.Captain = 1;
            }

            _db.SaveChanges();

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }




        [CustomAuthorize(Roles = "club-admin")]
        public ActionResult OfficialSelection(int matchId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tournament = _db.TournamentTours.Where(x => x.MatchId == matchId).Select(x => x.Tournament)
                .FirstOrDefault();
            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clubOfficialOrders = from coo in _db.ClubOfficialOrders
                                         //join tt in _db.TournamentTours on cto.TournamentId equals tt.TournamentId
                                         //join m in _db.Matches on tt.MatchId equals m.Id
                                     where !coo.IsDeleted //m.Id == matchId && cto.ClubId == _user.UserClubId
                                     select coo;


            clubOfficialOrders = clubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => !x.IsDeleted &&
                            x.ClubConfirm && x.OperatorConfirm && !x.Rejected &&
                            x.ClubId == _user.UserClubId &&
                            x.ClubOfficial.ContractBeginDate <= match.MatchDate &&
                            ((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > match.MatchDate) ||
                             (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > match.MatchDate))
                            && x.SeasonId == tournament.SeasonId/* && x.ClubOfficial.SeasonId == tournament.SeasonId*/)
                .OrderBy(x => x.ClubOfficial.Position.Sort);

            var matchOfficialSelectionViewModels = clubOfficialOrders.Select(x => new MatchOfficialSelectionViewModel()
            {
                ClubOfficialId = x.ClubOfficialId,
                OfficialFirstName = x.ClubOfficial.FirstName,
                OfficialLastName = x.ClubOfficial.LastName,
                OfficialFatherName = x.ClubOfficial.FatherName,
                OfficialPositionName = x.ClubOfficial.Position.Name,
                ClubTypeId = x.ClubTypeId,
                Selected = false
            }).ToList();

            var matchOfficials = _db.MatchClubOfficials.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == _user.UserClubId);

            foreach (var item in matchOfficials)
            {
                var selectedOfficial = matchOfficialSelectionViewModels.FirstOrDefault(x => x.ClubOfficialId == item.ClubOfficialId);

                if (selectedOfficial != null)
                {
                    selectedOfficial.Selected = true;
                    selectedOfficial.MatchOfficialZoneTypeId = item.MatchOfficialZoneTypeId;
                }
            }



            ViewBag.MatchId = matchId;
            ViewBag.TournamentTypeId = tournament.TournamentTypeId;

            return View(matchOfficialSelectionViewModels);
        }



        //[CustomAuthorize(Roles = "club-admin")]
        //[HttpGet]
        //public ActionResult AddMatchPlayer(int matchId, bool mainStaff)
        //{
        //    ViewBag.HasCaptain = _db.MatchClubPlayers.Any(x => x.ClubId == _user.UserClubId && x.MatchId == matchId && x.Captain == 1);
        //    ViewBag.MatchId = matchId;
        //    ViewBag.MainStaff = mainStaff;

        //    return View();
        //}

        [CustomAuthorize(Roles = "club-admin")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddMatchPlayer(AddPlayerToMatchViewModel addPlayerToMatchViewModel)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var matchConfirmStatus = ClubMatchConfirmStatus(_user.UserClubId.Value, addPlayerToMatchViewModel.MatchId);
            if (matchConfirmStatus.confirmStatus)
                return Json(new { ok = false, error = matchConfirmStatus.errorMessage });

            Tournament tournament = (from t in _db.Tournaments
                                     join tt in _db.TournamentTours on t.Id equals tt.TournamentId
                                     where tt.MatchId == addPlayerToMatchViewModel.MatchId
                                     select t).FirstOrDefault();

            if (tournament == null)
                return Json(new { ok = false, error = "Səhv müraciət. 0x0001" });


            //checking if player added and captain not selected
            if (addPlayerToMatchViewModel.Captain != null && addPlayerToMatchViewModel.Captain.Value)
            {
                var captains = _db.MatchClubPlayers.Where(x =>
                    !x.IsDeleted &&
                    x.MatchId == addPlayerToMatchViewModel.MatchId &&
                    x.ClubId == _user.UserClubId &&
                    x.PlayerClubTypeId == addPlayerToMatchViewModel.PlayerClubTypeId &&
                    x.Captain == 1);

                foreach (var captain in captains)
                {
                    captain.Captain = 0;
                }

                _db.SaveChanges();
            }




            if (addPlayerToMatchViewModel.AddStatus)
            {
                if (addPlayerToMatchViewModel.Captain != null && addPlayerToMatchViewModel.Captain.Value)
                {
                    var captain = _db.MatchClubPlayers.FirstOrDefault(x =>
                        !x.IsDeleted &&
                        x.MatchId == addPlayerToMatchViewModel.MatchId &&
                        x.ClubId == _user.UserClubId &&
                        x.PlayerId == addPlayerToMatchViewModel.PlayerId);

                    if (captain != null)
                    {
                        var isMainStaff = _db.MatchClubPlayerShifts.Any(x =>
                            !x.IsDeleted
                            && x.MatchId == addPlayerToMatchViewModel.MatchId
                            && x.ClubId == _user.UserClubId.Value
                            && x.PlayerId == addPlayerToMatchViewModel.PlayerId
                            && x.MinuteIn == 0);

                        if (isMainStaff)
                        {
                            captain.Captain = 1;
                            _db.SaveChanges();
                            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ok = false, error = "Ehtiyat oyunçu kapitan seçilə bilməz." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }



                TournamentTour tournamentTour =
                    _db.TournamentTours.Include(x => x.Match).FirstOrDefault(x => x.MatchId == addPlayerToMatchViewModel.MatchId);

                if (_db.MatchClubPlayers.Any(x =>
                    !x.IsDeleted &&
                    x.ClubId == _user.UserClubId && x.MatchId == addPlayerToMatchViewModel.MatchId &&
                    x.PlayerId == addPlayerToMatchViewModel.PlayerId))
                    return Json(new { ok = false, error = "Eyni oyunçu iki dəfə daxil edilə bilməz!" },
                        JsonRequestBehavior.AllowGet);


                if (tournamentTour != null)
                {

                    Player player = _db.Players.FirstOrDefault(x => x.Id == addPlayerToMatchViewModel.PlayerId);

                    if (player == null)
                        return Json(new { ok = false, error = "Oyunçu tapılmadı." },
                            JsonRequestBehavior.AllowGet);


                    #region Player Penalty Limit

                    SqlParameter matchIdParam = new SqlParameter("matchId", addPlayerToMatchViewModel.MatchId);
                    SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
                    SqlParameter playerIdParam = new SqlParameter("playerId", player.Id);
                    var playerPenaltyStatus =
                        _db.Database.SqlQuery<PlayerPenaltyStatus>("CheckPlayerPenalty @matchId, @clubId, @playerId", matchIdParam, clubIdParam, playerIdParam).FirstOrDefault();


                    if (playerPenaltyStatus == null)
                        return Json(new { ok = false, error = "Sistem xətası!" });

                    if (playerPenaltyStatus.Status == 0)
                        return Json(new { ok = false, error = playerPenaltyStatus.Note });


                    #endregion

                    if (addPlayerToMatchViewModel.MainStaff)
                    {

                        #region Main Staff 11 limit

                        var mainPlayerCount = _db.MatchClubPlayerShifts.Count(x =>
                            !x.IsDeleted
                            && x.MatchId == addPlayerToMatchViewModel.MatchId
                            && x.ClubId == _user.UserClubId.Value
                            && x.MinuteIn == 0);
                        if (mainPlayerCount >= 11)
                            return Json(new { ok = false, error = "Əsas heyətə 11-dən çox oyunçu seçilə bilməz!" },
                                JsonRequestBehavior.AllowGet);

                        #endregion

                        if (tournament.TournamentTypeId == 1 || tournament.TournamentTypeId == 4)
                        {
                            #region legionary max 6 limit

                            if (player.CitizenshipId != 16)
                            {
                                var legionaryCount = _db.MatchClubPlayerShifts.Include(x => x.Player).Count(x =>
                                    !x.IsDeleted &&
                                    x.MatchId == addPlayerToMatchViewModel.MatchId &&
                                    x.ClubId == _user.UserClubId.Value &&
                                    x.Player.CitizenshipId != 16 &&
                                    x.MinuteIn == 0);

                                if (legionaryCount >= tournament.LegionerLimit)
                                    return Json(new { ok = false, error = tournament.LegionerLimit + "-dan çox legioner seçilə bilməz!" },
                                        JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }





                        #region Young player min 1 limit

                        DateTime youngPlayerDateLimit = new DateTime(tournament.YoungPlayerAge21Limit, 1, 1);

                        var notYoungPlayerCount = _db.MatchClubPlayerShifts.Count(x =>
                            !x.IsDeleted
                            && x.MatchId == addPlayerToMatchViewModel.MatchId
                            && x.ClubId == _user.UserClubId.Value
                            && x.MinuteIn == 0
                            && x.Player.BirthDate < youngPlayerDateLimit);

                        if (notYoungPlayerCount == 10 && player.BirthDate < youngPlayerDateLimit)
                        {
                            return Json(new { ok = false, error = "Əsas heyyətdə az bir gənc oyunçu seçilməlidir!" },
                                JsonRequestBehavior.AllowGet);
                        }


                        #endregion



                        #region Young player 21 min limit

                        //DateTime youngPlayer21DateLimit = new DateTime(tournament.YoungPlayerAge21Limit, 1, 1);

                        //var notYoungPlayer21Count = _db.MatchClubPlayerShifts.Count(x =>
                        //    !x.IsDeleted
                        //    && x.MatchId == addPlayerToMatchViewModel.MatchId
                        //    && x.ClubId == _user.UserClubId.Value
                        //    && x.MinuteIn == 0
                        //    && x.Player.BirthDate < youngPlayer21DateLimit);


                        //if (notYoungPlayer21Count + tournament.YoungPlayerAge21Count +
                        //    tournament.YoungPlayerAge19Count == 11 && player.BirthDate < youngPlayer21DateLimit)
                        //{
                        //    return Json(new { ok = false, error = $"Əsas heyyətdə {11 - (tournament.YoungPlayerAge21Count + tournament.YoungPlayerAge19Count) } və daha yaşlı oyunçu limitini keçmisiniz!" });
                        //}


                        //if (notYoungPlayer21Count == 11 - tournament.YoungPlayerAge21Count && player.BirthDate < youngPlayer21DateLimit)
                        //{
                        //    return Json(new { ok = false, error = $"Əsas heyyətdə az {tournament.YoungPlayerAge21Count} {tournament.YoungPlayerAge21Limit} təvəllüdlü və ya daha gənc oyunçu seçilməlidir!" },
                        //        JsonRequestBehavior.AllowGet);
                        //}


                        #endregion


                        #region Young player 19 min limit

                        //DateTime youngPlayer19DateLimit = new DateTime(tournament.YoungPlayerAge19Limit, 1, 1);

                        //var notYoungPlayer19Count = _db.MatchClubPlayerShifts.Count(x =>
                        //    !x.IsDeleted
                        //    && x.MatchId == addPlayerToMatchViewModel.MatchId
                        //    && x.ClubId == _user.UserClubId.Value
                        //    && x.MinuteIn == 0
                        //    && x.Player.BirthDate < youngPlayer19DateLimit);

                        //if (notYoungPlayer19Count == 11 - tournament.YoungPlayerAge19Count && player.BirthDate < youngPlayer19DateLimit)
                        //{
                        //    return Json(new { ok = false, error = $"Əsas heyyətdə az {tournament.YoungPlayerAge19Count} {tournament.YoungPlayerAge19Limit} təvəllüdlü və ya daha gənc oyunçu seçilməlidir!" },
                        //        JsonRequestBehavior.AllowGet);
                        //}


                        #endregion

                    }
                    else
                    {
                        #region Ehtiyat Staff 12 limit

                        var mainPlayerCount = (from mcp in _db.MatchClubPlayers
                                               join mcps in _db.MatchClubPlayerShifts on
                                                   new { mcp.MatchId, mcp.ClubId, mcp.PlayerId } equals
                                                   new { mcps.MatchId, mcps.ClubId, mcps.PlayerId }
                                               where !mcp.IsDeleted &&
                                                     !mcps.IsDeleted &&
                                                     mcps.MatchId == addPlayerToMatchViewModel.MatchId &&
                                                     mcps.ClubId == _user.UserClubId.Value &&
                                                     mcps.MinuteIn == null
                                               select mcps.Id).Count();

                        if (mainPlayerCount > 11)
                            return Json(new { ok = false, error = "Ehtiyat heyyətə 12-dən çox oyunçu seçilə bilməz!" },
                                JsonRequestBehavior.AllowGet);

                        #endregion
                    }



                    DateTime tournamentStartDate = new DateTime(tournament.SeasonStartYear, 7, 1);
                    DateTime tournamentEndDate = new DateTime(tournament.SeasonEndYear, 7, 1);


                    var clubPlayerOrder = _db.ClubPlayerOrders.FirstOrDefault(x =>
                        x.ClubId == _user.UserClubId && x.PlayerId == addPlayerToMatchViewModel.PlayerId &&
                        x.CreationDate >= tournamentStartDate &&
                        x.CreationDate < tournamentEndDate);


                    if (clubPlayerOrder == null)
                    {
                        return Json(new { ok = false, error = "Sifariş verilməmiş oyunçu seçilə bilməz!" });
                    }

                    MatchClubPlayer matchClubPlayer = new MatchClubPlayer()
                    {
                        ClubId = _user.UserClubId.Value,
                        MatchId = addPlayerToMatchViewModel.MatchId,
                        PlayerId = addPlayerToMatchViewModel.PlayerId,
                        PlayerNumber = clubPlayerOrder.PlayerNumber,
                        PlayerClubTypeId = addPlayerToMatchViewModel.PlayerClubTypeId,
                        Captain = addPlayerToMatchViewModel.Captain.HasValue ? (addPlayerToMatchViewModel.Captain.Value ? 1 : 0) : 0,
                        CreatedById = _user.UserId,
                        CreationDate = DateTime.Now
                    };


                    MatchClubPlayerShift matchClubPlayerShift = new MatchClubPlayerShift()
                    {
                        ClubId = _user.UserClubId.Value,
                        MatchId = addPlayerToMatchViewModel.MatchId,
                        PlayerId = addPlayerToMatchViewModel.PlayerId,
                        CreatedById = _user.UserId,
                        CreationDate = DateTime.Now
                    };

                    if (addPlayerToMatchViewModel.MainStaff)
                    {
                        //matchClubPlayer.MinuteIn = 0;
                        matchClubPlayerShift.MinuteIn = 0;
                        matchClubPlayer.Played = 1;
                    }


                    _db.MatchClubPlayerShifts.Add(matchClubPlayerShift);
                    _db.MatchClubPlayers.Add(matchClubPlayer);
                    _db.SaveChanges();

                    return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var matchPlayer = _db.MatchClubPlayers
                    .FirstOrDefault(x => !x.IsDeleted
                                         && x.MatchId == addPlayerToMatchViewModel.MatchId
                                         && x.PlayerId == addPlayerToMatchViewModel.PlayerId
                                         && x.ClubId == _user.UserClubId);

                var matchPlayerShift = _db.MatchClubPlayerShifts
                    .FirstOrDefault(x => !x.IsDeleted
                                         && x.MatchId == addPlayerToMatchViewModel.MatchId
                                         && x.PlayerId == addPlayerToMatchViewModel.PlayerId
                                         && x.ClubId == _user.UserClubId);

                //if (matchPlayer == null || matchPlayerShift == null)
                //    return Json(new { ok = false, error = "Məlumat tapılmadı!" },
                //        JsonRequestBehavior.AllowGet);

                //Then if needed change to isdeleted
                //matchPlayer.IsDeleted = true;
                //matchPlayerShift.IsDeleted = true;
                _db.Entry(matchPlayer).State = EntityState.Deleted;
                _db.Entry(matchPlayerShift).State = EntityState.Deleted;
                _db.SaveChanges();
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);

            }



            return Json(new { ok = false, error = "Turnir düzgün seçilməyib." }, JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(Roles = "club-admin")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddMatchOfficial(AddOfficialToMatchViewModel addOfficial)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var matchConfirmStatus = ClubMatchConfirmStatus(_user.UserClubId.Value, addOfficial.MatchId);
            if (matchConfirmStatus.confirmStatus)
                return Json(new { ok = false, error = matchConfirmStatus.errorMessage });


            var matchOfficial = _db.MatchClubOfficials.FirstOrDefault(x =>
                !x.IsDeleted &&
                x.MatchId == addOfficial.MatchId &&
                x.ClubId == _user.UserClubId &&
                x.ClubOfficialId == addOfficial.OfficialId);

            if (addOfficial.AddStatus)
            {
                if (matchOfficial != null)
                {
                    return Json(new { ok = false, error = "Klubun eyni rəsmisi iki dəfə daxil edilə bilməz!" });
                }


                #region Official Penalty Limit

                SqlParameter matchIdParam = new SqlParameter("matchId", addOfficial.MatchId);
                SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
                SqlParameter officialIdParam = new SqlParameter("officialId", addOfficial.OfficialId);
                var playerPenaltyStatus =
                    _db.Database.SqlQuery<PlayerPenaltyStatus>("CheckOfficialPenalty @matchId, @clubId, @officialId", matchIdParam, clubIdParam, officialIdParam).FirstOrDefault();


                if (playerPenaltyStatus == null)
                    return Json(new { ok = false, error = "Sistem xətası!" });

                if (playerPenaltyStatus.Status == 0)
                    return Json(new { ok = false, error = playerPenaltyStatus.Note });


                #endregion


                #region TZ limit

                if (addOfficial.MatchOfficialZoneTypeId == 1)
                {
                    var tzLimit = _db.MatchClubOfficials.Count(x =>
                       !x.IsDeleted &&
                        x.MatchId == addOfficial.MatchId && x.ClubId == _user.UserClubId &&
                        x.ClubTypeId == addOfficial.OfficialClubTypeId && x.MatchOfficialZoneTypeId == 1);

                    if (tzLimit >= 7)
                    {
                        return Json(new { ok = false, error = "T/Z maksimum 7 nəfər seçilə bilər." });
                    }
                }

                #endregion

                #region EY limit

                if (addOfficial.MatchOfficialZoneTypeId == 2)
                {
                    var eyLimit = _db.MatchClubOfficials.Count(x =>
                        !x.IsDeleted &&
                        x.MatchId == addOfficial.MatchId && x.ClubId == _user.UserClubId &&
                        x.ClubTypeId == addOfficial.OfficialClubTypeId && x.MatchOfficialZoneTypeId == 2);

                    if (eyLimit >= 8)
                    {
                        return Json(new { ok = false, error = "Ə/Z maksimum 8 nəfər seçilə bilər." });
                    }
                }

                #endregion


                var matchClubOfficial = new MatchClubOfficial()
                {
                    MatchId = addOfficial.MatchId,
                    ClubId = _user.UserClubId.Value,
                    ClubOfficialId = addOfficial.OfficialId,
                    ClubTypeId = addOfficial.OfficialClubTypeId,
                    MatchOfficialZoneTypeId = addOfficial.MatchOfficialZoneTypeId,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.MatchClubOfficials.Add(matchClubOfficial);
                _db.SaveChanges();

                return Json(new { ok = true });
            }


            if (matchOfficial == null)
                return Json(new { ok = false, error = "Sətr tapılmadı!" });


            matchOfficial.IsDeleted = true;
            matchOfficial.LastUpdateById = _user.UserId;
            matchOfficial.LastUpdatedDate = DateTime.Now;
            _db.SaveChanges();

            return Json(new { ok = true });
        }




        public ActionResult Protokol(int matchId, int clubId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);




            Tournament tournament = (from t in _db.Tournaments
                                     join tt in _db.TournamentTours on t.Id equals tt.TournamentId
                                     where tt.MatchId == matchId
                                     select t).FirstOrDefault();

            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            //colden gelen clubId-nin matchId-ye bagli clublardan birinin olmasinin yoxlanilmasi

            var match = _db.Matches
                .Include(x => x.HomeClub)
                .Include(x => x.GuestClub)
                .Where(x => x.Id == matchId)
                .Select(x => new
                {
                    MatchDate = x.MatchDate,
                    HomeClubId = x.HomeClubId,
                    HomeClubName = x.HomeClub.Name,
                    GuestClubId = x.GuestClubId,
                    GuestClubName = x.GuestClub.Name,
                    HomeClubConfirm = x.HomeClubConfirm,
                    GuestClubConfirm = x.GuestClubConfirm
                })
                .FirstOrDefault();

            if (match == null)
                return HttpNotFound();


            if (_user.UserClubId.Value != clubId)
            {
                if (clubId != match.HomeClubId && clubId != match.GuestClubId)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (!(DateTime.Now.AddMinutes(75) >= match.MatchDate &&
                      (match.HomeClubConfirm != null && match.HomeClubConfirm.Value) &&
                      (match.GuestClubConfirm != null && match.GuestClubConfirm.Value)))
                {
                    clubId = _user.UserClubId.Value;
                }

            }
            else
            {
                if (_user.UserClubId != match.HomeClubId && _user.UserClubId != match.GuestClubId)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }







            //MatchPlayers
            var matchPlayerSelectionViewModel = MatchPlayerSelections(matchId, match.MatchDate, clubId);
            var sortedMatchPlayer =
                SortHelper.SortProtokolPlayerList(matchPlayerSelectionViewModel, tournament.YoungPlayerAge21Limit);


            //MatchOfficials
            ViewBag.MatchOfficialSelectionViewModels = MatchOfficialSelections(tournament.SeasonId, matchId, clubId, match.MatchDate);



            string clubName = "";

            if (clubId == match.HomeClubId)
                clubName = match.HomeClubName;

            if (clubId == match.GuestClubId)
                clubName = match.GuestClubName;

            ViewBag.ClubName = clubName;
            ViewBag.MatchId = matchId;
            ViewBag.ClubId = clubId;
            ViewBag.IsPdf = false;
            ViewBag.YoungPlayerAge21Limit = tournament.YoungPlayerAge21Limit;
            ViewBag.YoungPlayerAge19Limit = tournament.YoungPlayerAge19Limit;
            ViewBag.TournamentTypeId = tournament.TournamentTypeId;

            return View(sortedMatchPlayer);
        }





        public ActionResult ProtokolPdf(int matchId, int clubId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            Tournament tournament = (from t in _db.Tournaments
                                     join tt in _db.TournamentTours on t.Id equals tt.TournamentId
                                     where tt.MatchId == matchId
                                     select t).FirstOrDefault();

            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            //colden gelen clubId-nin matchId-ye bagli clublardan birinin olmasinin yoxlanilmasi

            var match = _db.Matches
                .Include(x => x.HomeClub)
                .Include(x => x.GuestClub)
                .Where(x => x.Id == matchId)
                .Select(x => new
                {
                    MatchDate = x.MatchDate,
                    HomeClubId = x.HomeClubId,
                    HomeClubName = x.HomeClub.Name,
                    GuestClubId = x.GuestClubId,
                    GuestClubName = x.GuestClub.Name,
                    HomeClubConfirm = x.HomeClubConfirm,
                    GuestClubConfirm = x.GuestClubConfirm
                })
                .FirstOrDefault();

            if (match == null)
                return HttpNotFound();


            if (_user.UserClubId.Value != clubId)
            {
                if (clubId != match.HomeClubId && clubId != match.GuestClubId)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (!(DateTime.Now.AddMinutes(75) >= match.MatchDate &&
                      (match.HomeClubConfirm != null && match.HomeClubConfirm.Value) &&
                      (match.GuestClubConfirm != null && match.GuestClubConfirm.Value)))
                {
                    clubId = _user.UserClubId.Value;
                }

            }
            else
            {
                if (_user.UserClubId != match.HomeClubId && _user.UserClubId != match.GuestClubId)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



            //MatchPlayers
            var matchPlayerSelectionViewModel = MatchPlayerSelections(matchId, match.MatchDate, clubId);
            var sortedMatchPlayer =
                SortHelper.SortProtokolPlayerList(matchPlayerSelectionViewModel, tournament.YoungPlayerAge21Limit);


            //MatchOfficials
            ViewBag.MatchOfficialSelectionViewModels = MatchOfficialSelections(tournament.SeasonId, matchId, clubId, match.MatchDate);


            string clubName = "";

            if (clubId == match.HomeClubId)
                clubName = match.HomeClubName;

            if (clubId == match.GuestClubId)
                clubName = match.GuestClubName;

            ViewBag.ClubName = clubName;
            ViewBag.MatchId = matchId;
            ViewBag.ClubId = clubId;
            ViewBag.IsPdf = true;
            ViewBag.YoungPlayerAge21Limit = tournament.YoungPlayerAge21Limit;
            ViewBag.YoungPlayerAge19Limit = tournament.YoungPlayerAge19Limit;
            ViewBag.TournamentTypeId = tournament.TournamentTypeId;


            return new ViewAsPdf("Protokol", sortedMatchPlayer);
        }


        #region Private Methods

        private List<MatchOfficialSelectionViewModel> MatchOfficialSelections(int seasonId, int matchId, int clubId, DateTime? matchDate)
        {

            if (!matchDate.HasValue)
                return new List<MatchOfficialSelectionViewModel>();


            var clubOfficialOrders = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted &&
                            x.ClubId == clubId &&
                            x.ClubOfficial.ContractBeginDate <= matchDate &&
                            ((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > matchDate) ||
                             (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > matchDate))
                            && x.SeasonId == seasonId)
                .OrderBy(x => x.ClubOfficial.Position.Sort);


            var matchOfficialSelectionViewModels = clubOfficialOrders.Select(x => new MatchOfficialSelectionViewModel()
            {
                ClubOfficialId = x.ClubOfficialId,
                OfficialFirstName = x.ClubOfficial.FirstName,
                OfficialLastName = x.ClubOfficial.LastName,
                OfficialFatherName = x.ClubOfficial.FatherName,
                OfficialPositionName = x.ClubOfficial.Position.Name,
                ClubTypeId = x.ClubTypeId,
                Selected = false
            }).ToList();

            var matchOfficials = _db.MatchClubOfficials.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == clubId);

            foreach (var item in matchOfficials)
            {
                var selectedOfficial = matchOfficialSelectionViewModels.FirstOrDefault(x => x.ClubOfficialId == item.ClubOfficialId);

                if (selectedOfficial != null)
                {
                    selectedOfficial.Selected = true;
                    selectedOfficial.MatchOfficialZoneTypeId = item.MatchOfficialZoneTypeId;
                }
            }

            return matchOfficialSelectionViewModels;
        }

        //TODO In ClubProtokolPlayers proc also return captain and selected fields
        private List<MatchPlayerSelectionViewModel> MatchPlayerSelections(int matchId, DateTime? matchDate, int clubId)
        {
            //var clubPlayerOrders = _db.ClubPlayerOrders.Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted && x.ClubId == clubId)
            //    .Include(x => x.Player)
            //    .Include(x => x.Player.Position)
            //    .Select(x => new MatchPlayerSelectionViewModel()
            //    {
            //        ClubTypeId = x.ClubTypeId,
            //        PlayerId = x.PlayerId,
            //        PlayerNumber = x.PlayerNumber,
            //        PlayerFirstName = x.Player.FirstName,
            //        PlayerLastName = x.Player.LastName,
            //        PlayerFatherName = x.Player.FatherName,
            //        PlayerBirthDate = x.Player.BirthDate,
            //        PositionLabel = x.Player.Position.Label,
            //        CitizenshipId = x.Player.CitizenshipId,
            //        Captain = false,
            //        Selected = false
            //    }).ToList();

            if (!matchDate.HasValue)
                return new List<MatchPlayerSelectionViewModel>();

            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter matchIdParam = new SqlParameter("matchId", matchId);
            SqlParameter matchDateParam = new SqlParameter("matchDate", matchDate);
            var clubPlayerOrders =
                _db.Database.SqlQuery<MatchPlayerSelectionViewModel>("ClubProtokolPlayers @clubId, @matchId, @matchDate", clubIdParam, matchIdParam, matchDateParam).ToList();


            var matchClubPlayers = _db.MatchClubPlayerShifts.Where(x => !x.IsDeleted && x.MatchId == matchId && x.ClubId == clubId);

            foreach (var item in matchClubPlayers)
            {
                var selectedPlayer = clubPlayerOrders.FirstOrDefault(x => x.PlayerId == item.PlayerId);

                if (selectedPlayer != null)
                {
                    selectedPlayer.Selected = true;
                    selectedPlayer.MainStaff = item.MinuteIn == 0;
                }
            }


            #region Club Captain

            var captainPlayerId = _db.MatchClubPlayers.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == _user.UserClubId &&
                x.Captain == 1).Select(x => x.PlayerId).FirstOrDefault();

            var captain = clubPlayerOrders.FirstOrDefault(x => x.PlayerId == captainPlayerId);
            if (captain != null)
            {
                captain.Captain = true;
            }

            #endregion

            return clubPlayerOrders;
        }


        private (bool confirmStatus, string errorMessage) ClubMatchConfirmStatus(int clubId, int matchId)
        {
            var match = _db.Matches.Where(x => x.Id == matchId)
                .Select(x => new
                {
                    x.HomeClubId,
                    x.GuestClubId,
                    x.MatchDate,
                    x.HomeClubConfirm,
                    x.HomeClubConfirmedDate,
                    x.HomeClubExpConfirmAllow,
                    x.GuestClubConfirm,
                    x.GuestClubConfirmedDate,
                    x.GuestClubExpConfirmAllow
                })
                .FirstOrDefault();

            if (match == null)
                return (false, "Səhv müraciət! 0x012");

            var currentDate = DateTime.Now;

            if (match.HomeClubId == clubId)
            {
                if (currentDate.AddMinutes(75) <= match.MatchDate)
                {
                    if (match.HomeClubConfirm.HasValue && match.HomeClubConfirm.Value)
                    {
                        return (true, "Təsdiqlənmiş oyun üzrə seçim etmək olmaz!");
                    }

                    return (false, "");
                }
                else if (match.HomeClubExpConfirmAllow)
                {
                    return (false, "");
                }
                else
                {
                    return (true, "Dəyişiklik oyun vaxtından 75 dəqiqə əvvələ qədər edilə bilər.");
                }
            }
            else if (match.GuestClubId == clubId)
            {
                if (currentDate.AddMinutes(75) <= match.MatchDate)
                {
                    if (match.GuestClubConfirm.HasValue && match.GuestClubConfirm.Value)
                    {
                        return (true, "Təsdiqlənmiş oyun üzrə seçim etmək olmaz!");
                    }

                    return (false, "");
                }
                else if (match.GuestClubExpConfirmAllow)
                {
                    return (false, "");
                }
                else
                {
                    return (true, "Dəyişiklik oyun vaxtından 75 dəqiqə əvvələ qədər edilə bilər.");
                }
            }


            return (false, "Səhv müraciət! 0x011");
        }

        #endregion





        //Confirm club match persons
        public ActionResult ConfirmMatch(int matchId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var matchConfirmStatus = ClubMatchConfirmStatus(_user.UserClubId.Value, matchId);
            if (matchConfirmStatus.confirmStatus)
                return Json(new ClubMatchConfirmViewModel(0, "Siyahı artıq təsdiqlənmişdir."), JsonRequestBehavior.AllowGet);



            SqlParameter userIdParam = new SqlParameter("userId", _user.UserId);
            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            SqlParameter matchIdParam = new SqlParameter("matchId", matchId);
            var clubMatchConfirmResponse =
                _db.Database.SqlQuery<ClubMatchConfirmViewModel>("CheckClubMatchConfirmStatus @userId, @clubId, @matchId", userIdParam, clubIdParam, matchIdParam).FirstOrDefault();



            return Json(clubMatchConfirmResponse, JsonRequestBehavior.AllowGet);
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
