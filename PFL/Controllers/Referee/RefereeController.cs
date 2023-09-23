using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models;
using PFL.Models.DTO;
using PFL.Models.Enums;
using PFL.Models.ViewModels;
using PFL.Utils;

namespace PFL.Controllers.Referee
{
    [CustomAuthorize(Roles = "referee")]
    public class RefereeController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public RefereeController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Hakim";
            ViewBag.BaseUrl = "Referee";
        }


        public ActionResult Matches(int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            User user = _db.Users.FirstOrDefault(x => x.Id == _user.UserId);

            if (user != null && !user.RefereeId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentDate = DateTime.Now;

            var matches = _db.TournamentTours
                .Include(x => x.Tournament)
                .Include(x => x.Tournament.TournamentType)
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub).Include(x => x.Match.GuestClub)
                .Where(x => x.Match.RefereeId == user.RefereeId && x.MatchId != null && x.Tournament.SeasonId == seasonId)
                .OrderByDescending(x => x.MatchId)
                .Select(x => new RefereeMatchViewModel
                {
                    TournamentTypeName = x.Tournament.TournamentType.Name,
                    TournamentId = x.TournamentId,
                    TournamentName = x.Tournament.Name,
                    SeasonStartYear = x.Tournament.SeasonStartYear,
                    SeasonEndYear = x.Tournament.SeasonEndYear,
                    TourNumber = x.TourNumber,
                    MatchId = x.MatchId,
                    HomeClubId = x.Match.HomeClubId,
                    HomeClubName = x.Match.HomeClub.Name,
                    GuestClubId = x.Match.GuestClubId,
                    GuestClubName = x.Match.GuestClub.Name,
                    Stadium = x.Match.Stadium.Name,
                    MatchDate = x.Match.MatchDate
                })
                .ToPagedList(pageIndex, pageSize);

            //var refereeMatches = new List<RefereeMatchViewModel>();

            //foreach (var match in matches)
            //{
            //    refereeMatches.Add(new RefereeMatchViewModel()
            //    {
            //        TournamentTypeName = match.TournamentTypeName,
            //        TournamentId = match.TournamentId,
            //        TournamentName = match.TournamentName,
            //        SeasonStartYear = match.SeasonStartYear,
            //        SeasonEndYear = match.SeasonEndYear,
            //        TourNumber = match.TourNumber,
            //        MatchId = match.MatchId,
            //        HomeClubId = match.HomeClubId,
            //        HomeClubName = match.HomeClubName,
            //        GuestClubId = match.GuestClubId,
            //        GuestClubName = match.GuestClubName,
            //        Stadium = match.Stadium,
            //        MatchDate = match.MatchDate
            //    });
            //}


            ViewBag.SeasonId = seasonId;

            return View(matches);
        }



        private RefereeMatchViewModel Match(int matchId)
        {

            User user = _db.Users.FirstOrDefault(x => x.Id == _user.UserId);

            if (user != null && !user.RefereeId.HasValue)
                return null;

            var match = _db.TournamentTours
                .Include(x => x.Tournament)
                .Include(x => x.Tournament.TournamentType)
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub).Include(x => x.Match.GuestClub)
                .Where(x => x.Match.RefereeId == user.RefereeId && x.MatchId != null && x.MatchId == matchId)
                .Select(x => new RefereeMatchViewModel
                {
                    TournamentTypeName = x.Tournament.TournamentType.Name,
                    TournamentId = x.TournamentId,
                    TournamentName = x.Tournament.Name,
                    SeasonStartYear = x.Tournament.SeasonStartYear,
                    SeasonEndYear = x.Tournament.SeasonEndYear,
                    TourNumber = x.TourNumber,
                    MatchId = x.MatchId,
                    HomeClubId = x.Match.HomeClubId,
                    HomeClubName = x.Match.HomeClub.Name,
                    GuestClubId = x.Match.GuestClubId,
                    GuestClubName = x.Match.GuestClub.Name,
                    Stadium = x.Match.Stadium.Name,
                    MatchDate = x.Match.MatchDate
                }).FirstOrDefault();

            return match;
        }

        [HttpGet]
        public ActionResult MatchDetail(int matchId)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.RefereeConfirm = match.RefereeConfirm;



            return View(this.Match(matchId));
        }

        public ActionResult MatchMainInfo(int matchId)
        {


            List<int?> plusGoalTypes = new List<int?>() { 1, 2, 5 };

            var mainInfo = _db.Matches
                    .Include(x => x.HomeClub)
                    .Include(x => x.GuestClub)
                    .Include(x => x.MatchResults)
                    .Select(x => new RefereeMatchMainInfoViewModel()
                    {
                        MatchId = x.Id,
                        HomeClubName = x.HomeClub.Name,
                        HomeClubScore = x.MatchResults.Count(z => (z.ClubId == x.HomeClubId && plusGoalTypes.Contains(z.GoalTypeId)) || (z.ClubId == x.GuestClubId && z.GoalTypeId == 3)),
                        GuestClubName = x.GuestClub.Name,
                        GuestClubScore = x.MatchResults.Count(z => (z.ClubId == x.GuestClubId && plusGoalTypes.Contains(z.GoalTypeId)) || (z.ClubId == x.HomeClubId && z.GoalTypeId == 3)),
                        RefereeId = x.RefereeId,
                        RefereeFullName = x.Referee.FirstName + " " + x.Referee.LastName,
                        RefereeAssistant1Id = x.RefereeAssistant1Id,
                        RefereeAssistant1FullName = x.RefereeAssistant1.FirstName + " " + x.RefereeAssistant1.LastName,
                        RefereeAssistant2Id = x.RefereeAssistant2Id,
                        RefereeAssistant2FullName = x.RefereeAssistant2.FirstName + " " + x.RefereeAssistant2.LastName,
                        FourthRefereeId = x.FourthRefereeId,
                        FourthRefereeFullName = x.FourthReferee.FirstName + " " + x.FourthReferee.LastName,
                        AdditionalReferee1Id = x.AdditionalReferee1Id,
                        AdditionalReferee1FullName = x.AdditionalReferee1.FirstName + " " + x.AdditionalReferee1.LastName,
                        AdditionalReferee2Id = x.AdditionalReferee2Id,
                        AdditionalReferee2FullName = x.AdditionalReferee2.FirstName + " " + x.AdditionalReferee2.LastName,
                        AffaRepresentative = x.AffaRepresentative.FirstName + " " + x.AffaRepresentative.LastName,
                        RefereeInspector = x.RefereeInspector.FirstName + " " + x.RefereeInspector.LastName,
                        HalfExtraTime = x.HalfExtraTime ?? 0,
                        Half2ExtraTime = x.Half2ExtraTime ?? 0,
                        FullExtraTime = x.FullExtraTime ?? 0,
                        Full2ExtraTime = x.Full2ExtraTime ?? 0,
                        Stadium = x.Stadium.Name,
                        AudienceCount = x.AudienceCount
                    })
                    .FirstOrDefault(x => x.MatchId == matchId);

            return View(mainInfo);
        }




        [HttpGet]
        public JsonResult MatchPlayers(int matchId, int? clubId, bool? played, bool? playing, string searchText)
        {
            //var playersQuery = _db.MatchClubPlayerShifts
            //    .Include(x => x.Player)
            //    .Include(x => x.Player.Position)
            //    .Where(x => !x.IsDeleted && x.MatchId == matchId);

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);

            int seasonId = SeasonHelper.GetCurrentSeasonId();


            var playersQuery = _db.ClubPlayerOrders
                .Include(x => x.Player)
                .Include(x => x.Player.Position)
                .Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted && x.SeasonId == seasonId);//bura clubPlayerOrders ucun date uzre sert qoymaq

            if (clubId.HasValue)
            {
                if (!(clubId.Value == match.HomeClubId || clubId.Value == match.GuestClubId))
                    return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);


                playersQuery = playersQuery.Where(x => x.ClubId == clubId);
            }
            else
            {
                playersQuery = playersQuery.Where(x => x.ClubId == match.HomeClubId || x.ClubId == match.GuestClubId);
            }


            if (played.HasValue)
            {
                if (played.Value)
                {
                    //bu hissede played value-suna gore oyunda olanlar ve olmayanlarin siyahisi gelecek. Eyni zamanda oyundan cixmislarin siyahisi oyuna daxil edilecekler siyahisinda gelmeyecek.

                    if (playing.HasValue && playing.Value)
                    {
                        var playedPlayerIdsQuery = _db.MatchClubPlayerShifts
                            .Where(x => x.MatchId == matchId && x.MinuteIn != null && x.MinuteOut == null).Select(x => x.PlayerId);

                        playersQuery = playersQuery.Where(x => playedPlayerIdsQuery.Contains(x.PlayerId));
                    }
                    else
                    {
                        var playedPlayerIdsQuery = _db.MatchClubPlayerShifts
                            .Where(x => x.MatchId == matchId && x.MinuteIn != null).Select(x => x.PlayerId);

                        playersQuery = playersQuery.Where(x => playedPlayerIdsQuery.Contains(x.PlayerId));
                    }
                }
                else
                {
                    var playedPlayerIdsQuery = _db.MatchClubPlayerShifts
                        .Where(x => x.MatchId == matchId && x.MinuteIn == null).Select(x => x.PlayerId);

                    playersQuery = playersQuery.Where(x => playedPlayerIdsQuery.Contains(x.PlayerId));
                }
            }




            var players = playersQuery
                .OrderBy(x => x.Player.Position.Id)
                .Select(x => new
                {
                    x.ClubId,
                    Select2Player = new Select2PlayerData
                    {
                        id = x.PlayerId,
                        text = x.PlayerNumber + " - " + x.Player.FirstName + " " + x.Player.LastName + " " +
                           x.Player.FatherName //+ " (" + x.Player.BirthDate?.ToString("dd.MM.yyyy") + ")"
                    }

                });

            if (clubId != null)
            {
                players = players.Where(x => x.ClubId == clubId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                players = players.Where(x => x.Select2Player.text.Contains(searchText));
            }

            var select2Data = players.Select(x => x.Select2Player).ToList();

            //foreach (var item in players)
            //{
            //    select2Data.Add(new Select2PlayerData()
            //    {
            //        id = item.PlayerId,
            //        text = item.PlayerNumber + " - " + item.FirstName + " " + item.LastName + " " + item.FatherName + " (" + item.BirthDate?.ToString("dd.MM.yyyy") + ")"
            //    });
            //}

            Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
            {
                results = select2Data,
                pagination = new Pagination() { more = false }
            };









            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MatchClubs(int matchId)
        {
            var match = _db.Matches
                .Include(x => x.HomeClub)
                .Include(x => x.GuestClub)
                .FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return null;


            List<Select2Data> select2Data = new List<Select2Data>();

            if (match.HomeClubId.HasValue)
            {
                select2Data.Add(new Select2Data()
                {
                    id = match.HomeClubId.Value,
                    text = match.HomeClub.Name
                });
            }

            if (match.GuestClubId.HasValue)
            {
                select2Data.Add(new Select2Data()
                {
                    id = match.GuestClubId.Value,
                    text = match.GuestClub.Name
                });
            }


            Select2Model select2PlayerModel = new Select2Model()
            {
                results = select2Data,
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }











        [HttpPost]
        public JsonResult SaveHalfExtra(int matchId, int halfExtra)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return Json(new { ok = false, error = "Maç düzgün seçilməyib." });

            if (RefereeConfirmStatus(match.Id))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            match.HalfExtraTime = halfExtra;
            match.LastUpdatedDate = DateTime.Now;
            match.LastUpdateById = _user.UserId;

            _db.SaveChanges();

            return Json(new { ok = true });
        }

        [HttpPost]
        public JsonResult SaveFullExtra(int matchId, int fullExtra)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return Json(new { ok = false, error = "Maç düzgün seçilməyib." });

            if (RefereeConfirmStatus(match.Id))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            match.FullExtraTime = fullExtra;
            match.LastUpdatedDate = DateTime.Now;
            match.LastUpdateById = _user.UserId;

            _db.SaveChanges();

            return Json(new { ok = true });
        }


        [HttpPost]
        public JsonResult SaveHalf2Extra(int matchId, int halfExtra)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return Json(new { ok = false, error = "Maç düzgün seçilməyib." });

            if (RefereeConfirmStatus(match.Id))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            match.Half2ExtraTime = halfExtra;
            match.LastUpdatedDate = DateTime.Now;
            match.LastUpdateById = _user.UserId;

            _db.SaveChanges();

            return Json(new { ok = true });
        }

        [HttpPost]
        public JsonResult SaveFull2Extra(int matchId, int fullExtra)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return Json(new { ok = false, error = "Maç düzgün seçilməyib." });

            if (RefereeConfirmStatus(match.Id))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            match.Full2ExtraTime = fullExtra;
            match.LastUpdatedDate = DateTime.Now;
            match.LastUpdateById = _user.UserId;

            _db.SaveChanges();

            return Json(new { ok = true });
        }



        [HttpPost]
        public JsonResult SaveAudienceCount(int matchId, int audienceCount)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return Json(new { ok = false, error = "Maç düzgün seçilməyib." });

            if (RefereeConfirmStatus(match.Id))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            match.AudienceCount = audienceCount;
            match.LastUpdatedDate = DateTime.Now;
            match.LastUpdateById = _user.UserId;

            _db.SaveChanges();

            return Json(new { ok = true });
        }


        [HttpGet]
        public ActionResult MatchNotes(int matchId)
        {

            var match = _db.Matches.Where(x => x.Id == matchId)
                .Select(x => new
                {
                    HomeClubName = x.HomeClub.Name,
                    GuestClubName = x.GuestClub.Name
                })
                .FirstOrDefault();

            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



            var matchNotes = _db.Matches.Where(x => x.Id == matchId)
                .Select(x => new List<MatchNoteViewModel>()
                {
                    new MatchNoteViewModel()
                    {
                        MatchId = x.Id,
                        MatchNoteType = MatchNoteType.CupNote,
                        Note = x.CupNote
                    },
                    new MatchNoteViewModel()
                    {
                        MatchId = x.Id,
                        MatchNoteType = MatchNoteType.RefereeNote,
                        Note = x.RefereeNote
                    },
                    new MatchNoteViewModel()
                    {
                        MatchId = x.Id,
                        MatchNoteType = MatchNoteType.HomeClubNote,
                        Note = x.HomeClubNote
                    },
                    new MatchNoteViewModel()
                    {
                        MatchId = x.Id,
                        MatchNoteType = MatchNoteType.GuestClubNote,
                        Note = x.GuestClubNote
                    },
                })
                .FirstOrDefault();


            MatchNoteViewModel first = null;
            foreach (var note in matchNotes)
            {
                first = note;
                break;
            }

            if (first != null)
                ViewBag.MatchId = first.MatchId;


            ViewBag.HomeClubName = match.HomeClubName;
            ViewBag.GuestClubName = match.GuestClubName;


            var tournament = _db.TournamentTours.Where(x => x.MatchId == matchId).Select(x => x.Tournament)
                .FirstOrDefault();

            ViewBag.TournamentTypeId = tournament?.TournamentTypeId ?? 0;

            if (tournament != null && tournament.TournamentTypeId != 4)
            {
                matchNotes.Remove(matchNotes.FirstOrDefault(x => x.MatchNoteType == MatchNoteType.CupNote));
            }


            return View(matchNotes);
        }


        [HttpPost]
        public ActionResult AddMatchNote(MatchNoteViewModel matchNote)
        {
            if (ModelState.IsValid)
            {
                var match = _db.Matches.FirstOrDefault(x => x.Id == matchNote.MatchId);

                if (match == null)
                    return Json(new { ok = false, error = "Maç seçimində xəta." });

                if (RefereeConfirmStatus(match.Id))
                    return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


                switch (matchNote.MatchNoteType)
                {
                    case MatchNoteType.CupNote:
                        match.CupNote = matchNote.Note;
                        break;

                    case MatchNoteType.RefereeNote:
                        match.RefereeNote = matchNote.Note;
                        break;

                    case MatchNoteType.HomeClubNote:
                        match.HomeClubNote = matchNote.Note;
                        break;

                    case MatchNoteType.GuestClubNote:
                        match.GuestClubNote = matchNote.Note;
                        break;
                }

                _db.SaveChanges();

                return Json(new { ok = true });

            }

            return Json(new { ok = false, error = "Model düzgün göndərilməyib." });
        }



        //Confirm match
        public ActionResult ConfirmMatch(int matchId)
        {
            var confirmStatus = new RefereeMatchConfirmViewModel()
            {
                Status = 1,
                Note = ""
            };

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
            {
                confirmStatus.Status = 0;
                confirmStatus.Note = "Oyun düz seçilməyib.";
            }
            else
            {
                match.RefereeConfirm = true;
                match.RefereeConfirmedDate = DateTime.Now;
                match.LastUpdateById = _user.UserId;
                match.LastUpdatedDate = DateTime.Now;
                _db.SaveChanges();
            }


            return Json(confirmStatus, JsonRequestBehavior.AllowGet);
        }



        #region Match Goals

        public ActionResult RowMatchGoal(int id)
        {
            var matchResult = _db.MatchResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .OrderBy(x => x.Minute)
                .Select(x => new RefereeMatchGoalsViewModel()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.Player.PlayerNumber,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeName = x.GoalType.Name,
                    Minute = x.Minute,
                    MinutePlus = x.MinutePlus
                })
                .FirstOrDefault(x => x.Id == id);


            return View(matchResult);
        }


        public ActionResult MatchGoals(int matchId)
        {
            var tournament = _db.TournamentTours.FirstOrDefault(x => x.MatchId == matchId)?.Tournament;
            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var matchResults = _db.MatchResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => x.MatchId == matchId)
                .OrderBy(x => x.Minute)
                .Select(x => new RefereeMatchGoalsViewModel()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.Player.PlayerNumber,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeName = x.GoalType.Name,
                    Minute = x.Minute,
                    MinutePlus = x.MinutePlus
                });


            if (tournament.Id > 24)
            {
                var playerIds = new List<long?>();

                playerIds.AddRange(matchResults.Select(x => x.PlayerId).ToList());

                var playerNumbers = _db.ClubPlayerOrders.Where(x =>
                        x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
                        x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
                    .Select(x => new { x.PlayerId, x.PlayerNumber })
                    .ToList();

                foreach (var matchResult in matchResults)
                {
                    var playerNumber = playerNumbers.FirstOrDefault(x => x.PlayerId == matchResult.PlayerId);
                    if (playerNumber != null)
                        matchResult.PlayerNumber = playerNumber.PlayerNumber;
                }
            }

            return View(matchResults);
        }




        [HttpGet]
        public ActionResult AddMatchGoal(int matchId)
        {
            ViewBag.MatchId = matchId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMatchGoal(MatchGoalDto matchGoalDto)
        {
            if (RefereeConfirmStatus(matchGoalDto.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchGoalDto.MatchId && (x.HomeClubId == matchGoalDto.ClubId || x.GuestClubId == matchGoalDto.ClubId));

            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG001" });



            if (!_db.MatchClubPlayers.Any(x => x.MatchId == match.Id && x.PlayerId == matchGoalDto.PlayerId))
                return Json(new { ok = false, error = "Səhv müraciət! 0xG002" });

            MatchResult matchResult = new MatchResult()
            {
                MatchId = matchGoalDto.MatchId,
                ClubId = matchGoalDto.ClubId,
                PlayerId = matchGoalDto.PlayerId,
                Goal = 1,
                GoalTypeId = matchGoalDto.GoalTypeId,
                Minute = matchGoalDto.Minute,
                MinutePlus = matchGoalDto.MinutePlus,
                CreationDate = DateTime.Now,
                CreatedById = _user.UserId
            };

            _db.MatchResults.Add(matchResult);
            _db.SaveChanges();


            return Json(new { ok = true, Id = matchResult.Id });
        }


        [HttpGet]
        public ActionResult EditMatchGoal(int id, int matchId)
        {
            if (RefereeConfirmStatus(matchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." }, JsonRequestBehavior.AllowGet);

            var matchResult = _db.MatchResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => x.Id == id)
                .Select(x => new MatchGoalDto()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeId = x.GoalTypeId,
                    GoalTypeName = x.GoalType.Name,
                    Minute = x.Minute,
                    MinutePlus = x.MinutePlus
                })
                .FirstOrDefault();

            if (matchResult == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG005" }, JsonRequestBehavior.AllowGet);


            ViewBag.MatchId = matchResult.MatchId;


            return View("AddMatchGoal", matchResult);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMatchGoal(MatchGoalDto matchGoalDto)
        {
            if (RefereeConfirmStatus(matchGoalDto.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchGoalDto.MatchId && (x.HomeClubId == matchGoalDto.ClubId || x.GuestClubId == matchGoalDto.ClubId));

            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG001" });



            if (!_db.MatchClubPlayers.Any(x => x.MatchId == match.Id && x.PlayerId == matchGoalDto.PlayerId))
                return Json(new { ok = false, error = "Səhv müraciət! 0xG002" });



            var matchResult = _db.MatchResults.FirstOrDefault(x => x.Id == matchGoalDto.Id);
            if (matchResult == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG003" });

            //matchResult.MatchId = matchGoalDto.MatchId;
            matchResult.ClubId = matchGoalDto.ClubId;
            matchResult.PlayerId = matchGoalDto.PlayerId;
            //matchResult.Goal = 1;
            matchResult.GoalTypeId = matchGoalDto.GoalTypeId;
            matchResult.Minute = matchGoalDto.Minute;
            matchResult.MinutePlus = matchGoalDto.MinutePlus;
            matchResult.LastUpdatedDate = DateTime.Now;
            matchResult.LastUpdateById = _user.UserId;

            _db.Entry(matchResult).State = EntityState.Modified;
            _db.SaveChanges();


            return Json(new { ok = true, Id = matchResult.Id });
        }







        [HttpDelete]
        public ActionResult DeleteMatchGoal(int id)
        {
            var goal = _db.MatchResults.Include(x => x.Match).FirstOrDefault(x => x.Id == id);
            if (goal == null)
                return Json(new { ok = false, error = "Səhv müraciət." });

            //checking if referee has access to do action
            var referee = _db.Referees.FirstOrDefault(x => x.Id == goal.Match.RefereeId);
            if (referee == null)
                return Json(new { ok = false, error = "Səhv müraciət." });

            var refereeUser = _db.Users.FirstOrDefault(x => x.Id == _user.UserId);
            if (refereeUser == null)
                return Json(new { ok = false, error = "Səhv müraciət." });

            if (referee.Id != refereeUser.RefereeId)
                return Json(new { ok = false, error = "Səhv müraciət." });




            if (RefereeConfirmStatus(goal.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            goal.IsDeleted = true;

            var goalLog = new MatchResultLog()
            {
                ClubId = goal.ClubId,
                CreatedById = goal.CreatedById,
                CreationDate = goal.CreationDate,
                Goal = goal.Goal,
                GoalTypeId = goal.GoalTypeId,
                LastUpdateById = goal.LastUpdateById,
                LastUpdatedDate = goal.LastUpdatedDate,
                IsDeleted = goal.IsDeleted,
                MatchId = goal.MatchId,
                Minute = goal.Minute,
                MinutePlus = goal.MinutePlus,
                PlayerId = goal.PlayerId,

                MainId = goal.Id,
                OperationTypeId = 1,
                LogCreatedById = _user.UserId,
                LogCreationDate = DateTime.Now
            };

            _db.MatchResultLogs.Add(goalLog);

            //_db.Entry(goal).State = EntityState.Deleted;
            _db.SaveChanges();

            return Json(new { ok = true, Id = id });
        }



        [HttpGet]
        public JsonResult GoalTypes()
        {
            var goalTypes = _db.GoalTypes
                .OrderBy(x => x.Id)
                .Select(x => new Select2Data
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = goalTypes,
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult PenaltyGoalTypes()
        {
            var goalTypes = _db.GoalTypes
                .Where(x => x.Id == 2 || x.Id == 4)
                .OrderBy(x => x.Id)
                .Select(x => new Select2Data
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = goalTypes,
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        #endregion



        #region Match Penalty Goals

        public ActionResult RowMatchPenaltyGoal(int id)
        {
            var matchResult = _db.MatchPenaltyResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .OrderBy(x => x.PenaltyOrder)
                .Select(x => new RefereeMatchPenaltyGoalsViewModel()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.Player.PlayerNumber,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeName = x.GoalType.Name,
                    PenaltyOrder = x.PenaltyOrder
                })
                .FirstOrDefault(x => x.Id == id);


            return View(matchResult);
        }


        public ActionResult MatchPenaltyGoals(int matchId)
        {
            var tournament = _db.TournamentTours.FirstOrDefault(x => x.MatchId == matchId)?.Tournament;
            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var matchResults = _db.MatchPenaltyResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => x.MatchId == matchId)
                .OrderBy(x => x.PenaltyOrder)
                .Select(x => new RefereeMatchPenaltyGoalsViewModel()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.Player.PlayerNumber,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeName = x.GoalType.Name,
                    PenaltyOrder = x.PenaltyOrder,
                });


            if (tournament.Id > 24)
            {
                var playerIds = new List<long?>();

                playerIds.AddRange(matchResults.Select(x => x.PlayerId).ToList());

                var playerNumbers = _db.ClubPlayerOrders.Where(x =>
                        x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
                        x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
                    .Select(x => new { x.PlayerId, x.PlayerNumber })
                    .ToList();

                foreach (var matchResult in matchResults)
                {
                    var playerNumber = playerNumbers.FirstOrDefault(x => x.PlayerId == matchResult.PlayerId);
                    if (playerNumber != null)
                        matchResult.PlayerNumber = playerNumber.PlayerNumber;
                }
            }

            return View(matchResults);
        }




        [HttpGet]
        public ActionResult AddMatchPenaltyGoal(int matchId)
        {
            ViewBag.MatchId = matchId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMatchPenaltyGoal(MatchPenaltyGoalDto matchGoalDto)
        {
            if (RefereeConfirmStatus(matchGoalDto.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchGoalDto.MatchId && (x.HomeClubId == matchGoalDto.ClubId || x.GuestClubId == matchGoalDto.ClubId));

            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG001" });



            if (!_db.MatchClubPlayers.Any(x => x.MatchId == match.Id && x.PlayerId == matchGoalDto.PlayerId))
                return Json(new { ok = false, error = "Səhv müraciət! 0xG002" });

            MatchPenaltyResult matchResult = new MatchPenaltyResult()
            {
                MatchId = matchGoalDto.MatchId,
                ClubId = matchGoalDto.ClubId,
                PlayerId = matchGoalDto.PlayerId,
                Goal = 1,
                GoalTypeId = matchGoalDto.GoalTypeId,
                PenaltyOrder = matchGoalDto.PenaltyOrder,
                CreationDate = DateTime.Now,
                CreatedById = _user.UserId
            };

            _db.MatchPenaltyResults.Add(matchResult);
            _db.SaveChanges();


            return Json(new { ok = true, Id = matchResult.Id });
        }


        [HttpGet]
        public ActionResult EditMatchPenaltyGoal(int id, int matchId)
        {
            if (RefereeConfirmStatus(matchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." }, JsonRequestBehavior.AllowGet);

            var matchResult = _db.MatchPenaltyResults
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => x.Id == id)
                .Select(x => new MatchPenaltyGoalDto()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Goal = x.Goal,
                    GoalTypeId = x.GoalTypeId,
                    GoalTypeName = x.GoalType.Name,
                    PenaltyOrder = x.PenaltyOrder,
                })
                .FirstOrDefault();

            if (matchResult == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG005" }, JsonRequestBehavior.AllowGet);


            ViewBag.MatchId = matchResult.MatchId;


            return View("AddMatchPenaltyGoal", matchResult);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMatchPenaltyGoal(MatchPenaltyGoalDto matchGoalDto)
        {
            if (RefereeConfirmStatus(matchGoalDto.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            var match = _db.Matches.FirstOrDefault(x => x.Id == matchGoalDto.MatchId && (x.HomeClubId == matchGoalDto.ClubId || x.GuestClubId == matchGoalDto.ClubId));

            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG001" });



            if (!_db.MatchClubPlayers.Any(x => x.MatchId == match.Id && x.PlayerId == matchGoalDto.PlayerId))
                return Json(new { ok = false, error = "Səhv müraciət! 0xG002" });



            var matchResult = _db.MatchPenaltyResults.FirstOrDefault(x => x.Id == matchGoalDto.Id);
            if (matchResult == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0xG003" });

            //matchResult.MatchId = matchGoalDto.MatchId;
            matchResult.ClubId = matchGoalDto.ClubId;
            matchResult.PlayerId = matchGoalDto.PlayerId;
            //matchResult.Goal = 1;
            matchResult.GoalTypeId = matchGoalDto.GoalTypeId;
            matchResult.PenaltyOrder = matchGoalDto.PenaltyOrder;
            matchResult.LastUpdatedDate = DateTime.Now;
            matchResult.LastUpdateById = _user.UserId;

            _db.Entry(matchResult).State = EntityState.Modified;
            _db.SaveChanges();


            return Json(new { ok = true, Id = matchResult.Id });
        }







        [HttpDelete]
        public ActionResult DeleteMatchPenaltyGoal(int id)
        {
            var goal = _db.MatchPenaltyResults.FirstOrDefault(x => x.Id == id);
            if (goal == null)
                return Json(new { ok = false, error = "Səhv müraciət." });

            if (RefereeConfirmStatus(goal.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            _db.Entry(goal).State = EntityState.Deleted;
            _db.SaveChanges();

            return Json(new { ok = true, Id = id });
        }


        #endregion


        #region Match Player Shifts

        //public ActionResult RowPlayerShift(int id)
        //{
        //    var tournament = _db.MatchClubPlayerShifts
        //        .Select(x => x.Match.TournamentTours.Take(1).FirstOrDefault().Tournament)
        //        .FirstOrDefault(x => x.Id == id);

        //    if (tournament == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    var playerShift = _db.MatchClubPlayerShifts
        //        .Include(x => x.Club)
        //        .Include(x => x.Player)
        //        .Where(x => !x.IsDeleted && x.Id == id && x.PlayerOut != null)
        //        .Select(x => new RefereePlayerShiftViewModel()
        //        {
        //            Id = x.Id,
        //            MatchId = x.MatchId,
        //            ClubId = x.ClubId,
        //            ClubName = x.Club.Name,
        //            PlayerInId = x.PlayerId,
        //            PlayerInNumber = x.Player.PlayerNumber,
        //            PlayerInFirstName = x.Player.FirstName,
        //            PlayerInLastName = x.Player.LastName,
        //            PlayerInFatherName = x.Player.FatherName,
        //            PlayerOutId = x.PlayerIdOut,
        //            PlayerOutNumber = x.PlayerOut.PlayerNumber,
        //            PlayerOutFirstName = x.PlayerOut.FirstName,
        //            PlayerOutLastName = x.PlayerOut.LastName,
        //            PlayerOutFatherName = x.PlayerOut.FatherName,
        //            MinuteIn = x.MinuteIn,
        //            MinuteInPlus = x.MinuteInPlus
        //        })
        //        .FirstOrDefault();


        //    if (playerShift == null)
        //        return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);


        //    if (tournament.Id > 24)
        //    {
        //        var playerIds = new List<long?>
        //        {
        //            playerShift.PlayerInId,
        //            playerShift.PlayerOutId
        //        };


        //        var playerNumbers = _db.ClubPlayerOrders.Where(x =>
        //            x.CreationDate >= new DateTime(tournament.SeasonStartYear, 1, 1) &&
        //            x.CreationDate < new DateTime(tournament.SeasonEndYear, 1, 1) && playerIds.Contains(x.PlayerId))
        //            .Select(x => new { x.PlayerId, x.PlayerNumber })
        //            .ToList();


        //        var playerNumberIn = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerInId);
        //        if (playerNumberIn != null)
        //            playerShift.PlayerInNumber = playerNumberIn.PlayerNumber;

        //        var playerNumberOut = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerOutId);
        //        if (playerNumberOut != null)
        //            playerShift.PlayerOutNumber = playerNumberOut.PlayerNumber;

        //    }

        //    return View(playerShift);
        //}


        public ActionResult PlayerShifts(int matchId)
        {

            var tournament = _db.TournamentTours.FirstOrDefault(x => x.MatchId == matchId)?.Tournament;
            if (tournament == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



            var playerShifts = _db.MatchClubPlayerShifts
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => !x.IsDeleted && x.MatchId == matchId && x.PlayerOut != null)
                .Select(x => new RefereePlayerShiftViewModel()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerInId = x.PlayerId,
                    PlayerInNumber = x.Player.PlayerNumber,
                    PlayerInFirstName = x.Player.FirstName,
                    PlayerInLastName = x.Player.LastName,
                    PlayerInFatherName = x.Player.FatherName,
                    PlayerOutId = x.PlayerIdOut,
                    PlayerOutNumber = x.PlayerOut.PlayerNumber,
                    PlayerOutFirstName = x.PlayerOut.FirstName,
                    PlayerOutLastName = x.PlayerOut.LastName,
                    PlayerOutFatherName = x.PlayerOut.FatherName,
                    MinuteIn = x.MinuteIn,
                    MinuteInPlus = x.MinuteInPlus
                })
                .OrderBy(x => x.MinuteIn).ThenBy(x => x.MinuteInPlus)
                .ToList();




            if (tournament.Id > 24)
            {
                var playerIds = new List<long?>();

                playerIds.AddRange(playerShifts.Select(x => x.PlayerInId).ToList());
                playerIds.AddRange(playerShifts.Select(x => x.PlayerOutId).ToList());

                var playerNumbers = _db.ClubPlayerOrders.Where(x =>
                    x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
                    x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
                    .Select(x => new { x.PlayerId, x.PlayerNumber })
                    .ToList();

                foreach (var playerShift in playerShifts)
                {
                    var playerNumberIn = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerInId);
                    if (playerNumberIn != null)
                        playerShift.PlayerInNumber = playerNumberIn.PlayerNumber;

                    var playerNumberOut = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerOutId);
                    if (playerNumberOut != null)
                        playerShift.PlayerOutNumber = playerNumberOut.PlayerNumber;
                }
            }

            return View(playerShifts);
        }

        private class DuplicateMatchPlayer
        {
            public DuplicateMatchPlayer(long playerId, int count)
            {
                PlayerId = playerId;
                Count = count;
            }
            public long PlayerId { get; set; }
            public int Count { get; set; }
        }

        [HttpDelete]
        public ActionResult DeleteShift(int id)
        {

            var shift = _db.MatchClubPlayerShifts.FirstOrDefault(x => x.Id == id);
            if (shift == null)
                return Json(new { ok = false, error = "Səhv müraciət!" });

            if (RefereeConfirmStatus(shift.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            if (shift.MinuteIn != 0)
            {
                long? shiftPlayerIdOut = shift.PlayerIdOut;

                DeleteSubShifts(shift.MatchId, shift.ClubId, shift.PlayerId);

                shift.PlayerIdOut = null;
                shift.MinuteIn = null;
                shift.MinuteInPlus = null;
                shift.MinuteOut = null;
                shift.MinuteOutPlus = null;
                shift.LastUpdateById = _user.UserId;
                shift.LastUpdatedDate = DateTime.Now;



                var parentShift = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                    x.MatchId == shift.MatchId && x.ClubId == shift.ClubId && x.PlayerId == shiftPlayerIdOut);

                if (parentShift == null)
                    return Json(new { ok = false, error = "Sistem xətası! - 1" });


                //if (parentShift.MinuteIn == 0)
                //{
                parentShift.MinuteOut = null;
                parentShift.MinuteOutPlus = null;
                parentShift.LastUpdateById = _user.UserId;
                parentShift.LastUpdatedDate = DateTime.Now;
                //}
                //else
                //{
                //    parentShift.PlayerIdOut = null;
                //    parentShift.MinuteIn = null;
                //    parentShift.MinuteInPlus = null;
                //    parentShift.MinuteOut = null;
                //    parentShift.MinuteOutPlus = null;
                //    parentShift.LastUpdateById = _user.UserId;
                //    parentShift.LastUpdatedDate = DateTime.Now;
                //}


                _db.Entry(shift).State = EntityState.Modified;
                _db.Entry(parentShift).State = EntityState.Modified;

                _db.SaveChanges();

            }
            else
            {
                return Json(new { ok = false, error = "Sistem xətası! - 2" });
            }





            //var matchShifts = _db.MatchClubPlayerShifts.Where(x =>
            //    x.MatchId == shift.MatchId && (x.PlayerIdOut != null || x.MinuteOut != null));

            //var duplicateMatchPlayers = new List<DuplicateMatchPlayer>();

            //foreach (var matchShift in matchShifts)
            //{
            //    if (matchShift.MinuteIn == 0)
            //    {
            //        matchShift.MinuteOut = null;
            //        matchShift.MinuteOutPlus = null;
            //    }

            //    if (matchShift.PlayerIdOut != null)
            //    {
            //        matchShift.PlayerIdOut = null;
            //        matchShift.MinuteIn = null;
            //        matchShift.MinuteInPlus = null;
            //        matchShift.MinuteOut = null;
            //        matchShift.MinuteOutPlus = null;
            //    }


            //    matchShift.LastUpdateById = _user.UserId;
            //    matchShift.LastUpdatedDate = DateTime.Now;


            //    ////detecting duplicates
            //    //var duplicateMatchPlayer = duplicateMatchPlayers.FirstOrDefault(x => x.PlayerId == matchShift.PlayerId);
            //    //if (duplicateMatchPlayer == null)
            //    //{
            //    //    duplicateMatchPlayers.Add(new DuplicateMatchPlayer(matchShift.PlayerId, 1));
            //    //}
            //    //else
            //    //{
            //    //    duplicateMatchPlayer.Count++;
            //    //}

            //}

            ////_db.SaveChanges();



            //foreach (var duplicateMatchPlayer in duplicateMatchPlayers.Where(x => x.Count > 1))
            //{
            //    var duplicates = _db.MatchClubPlayerShifts.Where(x =>
            //        x.MatchId == shift.MatchId && x.PlayerId == duplicateMatchPlayer.PlayerId && x.MinuteIn == null);

            //    foreach (var duplicate in duplicates)
            //    {
            //        _db.Entry(duplicate).State = EntityState.Deleted;
            //    }
            //}

            _db.SaveChanges();




            return Json(new { ok = true });
        }


        private void DeleteSubShifts(int matchId, int clubId, long playerId)
        {

            var subShift = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                x.MatchId == matchId && x.ClubId == clubId && x.PlayerIdOut == playerId);
            if (subShift != null)
            {
                subShift.PlayerIdOut = null;
                subShift.MinuteIn = null;
                subShift.MinuteInPlus = null;
                subShift.MinuteOut = null;
                subShift.MinuteOutPlus = null;

                subShift.LastUpdateById = _user.UserId;
                subShift.LastUpdatedDate = DateTime.Now;

                _db.SaveChanges();
                DeleteSubShifts(matchId, clubId, subShift.PlayerId);
            }
        }


        [HttpGet]
        public ActionResult AddPlayerShift(int matchId)
        {
            ViewBag.MatchId = matchId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlayerShift(RefereeAddPlayerShift addPlayerShift)
        {

            if (RefereeConfirmStatus(addPlayerShift.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            if (addPlayerShift.PlayerInId == addPlayerShift.PlayerOutId)
                return Json(new { ok = false, error = "Eyni oyunçu əvəzetmə edilə biləz!" },
                    JsonRequestBehavior.AllowGet);

            if (_db.MatchClubPlayerShifts.Any(x => x.MatchId == addPlayerShift.MatchId && x.ClubId == addPlayerShift.ClubId &&
                                                  x.PlayerId == addPlayerShift.PlayerInId && x.MinuteOut != null))
                return Json(new { ok = false, error = "MEYDANI TƏRK EDƏN OYUNÇU YENİDƏN DAXİL EDİLƏ BİLMƏZ!" },
                    JsonRequestBehavior.AllowGet);



            var matchPlayerOut = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                x.MatchId == addPlayerShift.MatchId && x.ClubId == addPlayerShift.ClubId &&
                x.PlayerId == addPlayerShift.PlayerOutId);

            var matchPlayerIn = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                x.MatchId == addPlayerShift.MatchId && x.ClubId == addPlayerShift.ClubId &&
                x.PlayerId == addPlayerShift.PlayerInId);


            if (matchPlayerIn == null || matchPlayerOut == null)
                return Json(new { ok = false, error = "Matç və oyunçu düz seçilməyib." },
                    JsonRequestBehavior.AllowGet);



            matchPlayerOut.MinuteOut = addPlayerShift.MinuteIn;
            matchPlayerOut.MinuteOutPlus = addPlayerShift.MinuteInPlus;
            matchPlayerOut.LastUpdatedDate = DateTime.Now;
            matchPlayerOut.LastUpdateById = _user.UserId;


            if (matchPlayerIn.MinuteOut != null)
            {
                var playerIn = new MatchClubPlayerShift()
                {
                    MatchId = matchPlayerIn.MatchId,
                    ClubId = matchPlayerIn.ClubId,
                    PlayerId = matchPlayerIn.PlayerId,
                    PlayerIdOut = matchPlayerOut.PlayerId,
                    MinuteIn = addPlayerShift.MinuteIn,
                    MinuteInPlus = addPlayerShift.MinuteInPlus,
                    CreationDate = DateTime.Now,
                    CreatedById = _user.UserId
                };
                _db.MatchClubPlayerShifts.Add(playerIn);
            }
            else
            {
                matchPlayerIn.PlayerIdOut = matchPlayerOut.PlayerId;
                matchPlayerIn.MinuteIn = addPlayerShift.MinuteIn;
                matchPlayerIn.MinuteInPlus = addPlayerShift.MinuteInPlus;
                //matchPlayerIn.Played = 1;
                matchPlayerIn.LastUpdatedDate = DateTime.Now;
                matchPlayerIn.LastUpdateById = _user.UserId;
            }




            _db.SaveChanges();


            return Json(new { ok = true },
                JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult EditPlayerShift(int id, int matchId)
        {

            var tournament = _db.TournamentTours.FirstOrDefault(x => x.MatchId == matchId)?.Tournament;
            if (tournament == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0x001" }, JsonRequestBehavior.AllowGet);



            var playerShift = _db.MatchClubPlayerShifts
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Where(x => !x.IsDeleted && x.Id == id && x.MatchId == matchId && x.PlayerOut != null)
                .Select(x => new RefereeAddPlayerShift()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    ClubId = x.ClubId,
                    ClubName = x.Club.Name,
                    PlayerInId = x.PlayerId,
                    PlayerInNumber = x.Player.PlayerNumber,
                    PlayerInFirstName = x.Player.FirstName,
                    PlayerInLastName = x.Player.LastName,
                    PlayerInFatherName = x.Player.FatherName,
                    PlayerOutId = x.PlayerIdOut,
                    PlayerOutNumber = x.PlayerOut.PlayerNumber,
                    PlayerOutFirstName = x.PlayerOut.FirstName,
                    PlayerOutLastName = x.PlayerOut.LastName,
                    PlayerOutFatherName = x.PlayerOut.FatherName,
                    MinuteIn = x.MinuteIn,
                    MinuteInPlus = x.MinuteInPlus
                })
                .FirstOrDefault();

            if (playerShift == null)
                return Json(new { ok = false, error = "Səhv müraciət! 0x002" }, JsonRequestBehavior.AllowGet);



            if (tournament.Id > 24)
            {
                var playerIds = new List<long?>();

                playerIds.Add(playerShift.PlayerInId);
                playerIds.Add(playerShift.PlayerOutId);

                var playerNumbers = _db.ClubPlayerOrders.Where(x =>
                    x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
                    x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
                    .Select(x => new { x.PlayerId, x.PlayerNumber })
                    .ToList();


                var playerNumberIn = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerInId);
                if (playerNumberIn != null)
                    playerShift.PlayerInNumber = playerNumberIn.PlayerNumber;

                var playerNumberOut = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerOutId);
                if (playerNumberOut != null)
                    playerShift.PlayerOutNumber = playerNumberOut.PlayerNumber;

            }


            ViewBag.MatchId = matchId;
            return View("AddPlayerShift", playerShift);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPlayerShift(RefereeAddPlayerShift playerShiftDto)
        {
            if (RefereeConfirmStatus(playerShiftDto.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            var playerShift = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                x.Id == playerShiftDto.Id && x.MatchId == playerShiftDto.MatchId && !x.IsDeleted);

            if (playerShift == null)
                return Json(new { ok = false, error = "Səhv müraciət! - 01" });

            var playerShiftOut = _db.MatchClubPlayerShifts.FirstOrDefault(x =>
                x.MatchId == playerShift.MatchId && x.PlayerId == playerShift.PlayerIdOut);


            if (playerShiftOut == null)
                return Json(new { ok = false, error = "Səhv müraciət! - 02" });


            playerShift.MinuteIn = playerShiftDto.MinuteIn;
            playerShift.MinuteInPlus = playerShiftDto.MinuteInPlus;
            playerShift.LastUpdateById = _user.UserId;
            playerShift.LastUpdatedDate = DateTime.Now;

            playerShiftOut.MinuteOut = playerShiftDto.MinuteIn;
            playerShiftOut.MinuteOutPlus = playerShiftDto.MinuteInPlus;
            playerShiftOut.LastUpdateById = _user.UserId;
            playerShiftOut.LastUpdatedDate = DateTime.Now;


            _db.Entry(playerShift).State = EntityState.Modified;
            _db.Entry(playerShiftOut).State = EntityState.Modified;
            _db.SaveChanges();

            return Json(new { ok = true });
        }



        #endregion




        #region Match Cards

        public ActionResult MatchCards(int matchId)
        {
            var tournament = _db.TournamentTours.Where(x => x.MatchId == matchId).Select(x => x.Tournament).FirstOrDefault();

            List<MatchPenaltyCardDto> matchPenaltyCards = new List<MatchPenaltyCardDto>();
            if (tournament != null)
            {
                var penaltyCards = _db.MatchClubPlayers
                .Include(x => x.Club)
                .Include(x => x.Player)
                .Include(x => x.CardReasonYellow1)
                .Include(x => x.CardReasonYellow2)
                .Include(x => x.CardReasonRed)
                .Where(x => !x.IsDeleted &&
                            x.MatchId == matchId &&
                            (x.YellowMinute != null ||
                             x.Yellow2Minute != null ||
                             x.RedMinute != null ||
                             x.YellowOffgameTypeId != null ||
                             x.Yellow2OffgameTypeId != null ||
                             x.RedOffgameTypeId != null))
                .Select(x => new
                {
                    MatchClubPlayerId = x.Id,
                    MatchId = x.MatchId,
                    ClubName = x.Club.Name,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.Player.PlayerNumber,
                    FirstName = x.Player.FirstName,
                    LastName = x.Player.LastName,
                    YellowMinute = x.YellowMinute,
                    YellowMinutePlus = x.YellowMinutePlus,
                    YellowOffGameTypeName = x.OffGameTypeYellow.Name,
                    YellowOffGameTypeId = x.YellowOffgameTypeId,
                    YellowReason = x.CardReasonYellow1.Name,
                    Yellow2Minute = x.Yellow2Minute,
                    Yellow2MinutePlus = x.Yellow2MinutePlus,
                    Yellow2OffGameTypeId = x.Yellow2OffgameTypeId,
                    Yellow2OffGameTypeName = x.OffGameTypeYellow2.Name,
                    Yellow2Reason = x.CardReasonYellow2.Name,
                    RedMinute = x.RedMinute,
                    RedMinutePlus = x.RedMinutePlus,
                    RedOffGameTypeId = x.RedOffgameTypeId,
                    RedOffGameTypeName = x.OffGameTypeRed.Name,
                    RedReason = x.CardReasonRed.Name
                }).ToList();


                foreach (var item in penaltyCards)
                {
                    if (item.YellowMinute != null || item.YellowOffGameTypeId != null)
                    {
                        matchPenaltyCards.Add(new MatchPenaltyCardDto()
                        {
                            MatchClubPlayerId = item.MatchClubPlayerId,
                            MatchId = item.MatchId,
                            ClubName = item.ClubName,
                            PlayerId = item.PlayerId,
                            PenaltyCardType = PenaltyCardTypeEnum.Yellow,
                            PlayerNumber = item.PlayerNumber,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Minute = item.YellowMinute,
                            MinutePlus = item.YellowMinutePlus,
                            OffGameTypeName = item.YellowOffGameTypeName,
                            Reason = item.YellowReason
                        });
                    }

                    if (item.Yellow2Minute != null || item.Yellow2OffGameTypeId != null)
                    {
                        matchPenaltyCards.Add(new MatchPenaltyCardDto()
                        {
                            MatchClubPlayerId = item.MatchClubPlayerId,
                            MatchId = item.MatchId,
                            ClubName = item.ClubName,
                            PlayerId = item.PlayerId,
                            PenaltyCardType = PenaltyCardTypeEnum.Yellow2,
                            PlayerNumber = item.PlayerNumber,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Minute = item.Yellow2Minute,
                            MinutePlus = item.Yellow2MinutePlus,
                            OffGameTypeName = item.Yellow2OffGameTypeName,
                            Reason = item.Yellow2Reason
                        });
                    }

                    if (item.RedMinute != null || item.RedOffGameTypeId != null)
                    {
                        matchPenaltyCards.Add(new MatchPenaltyCardDto()
                        {
                            MatchClubPlayerId = item.MatchClubPlayerId,
                            MatchId = item.MatchId,
                            ClubName = item.ClubName,
                            PlayerId = item.PlayerId,
                            PenaltyCardType = PenaltyCardTypeEnum.Red,
                            PlayerNumber = item.PlayerNumber,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Minute = item.RedMinute,
                            MinutePlus = item.RedMinutePlus,
                            OffGameTypeName = item.RedOffGameTypeName,
                            Reason = item.RedReason
                        });
                    }
                }


                if (tournament.Id > 24)
                {
                    var playerIds = new List<long>();

                    playerIds.AddRange(matchPenaltyCards.Select(x => x.PlayerId).ToList());

                    var playerNumbers = _db.ClubPlayerOrders.Where(x =>
                            x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
                            x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
                        .Select(x => new { x.PlayerId, x.PlayerNumber })
                        .ToList();

                    foreach (var matchPenaltyCard in matchPenaltyCards)
                    {
                        var playerNumber = playerNumbers.FirstOrDefault(x => x.PlayerId == matchPenaltyCard.PlayerId);
                        if (playerNumber != null)
                            matchPenaltyCard.PlayerNumber = playerNumber.PlayerNumber;
                    }
                }
            }

            return View(matchPenaltyCards.OrderBy(x => x.Minute).ToList());
        }


        [HttpDelete]
        public ActionResult DeleteCard(int matchPlayerId, PenaltyCardTypeEnum penaltyCardType)
        {
            var matchPlayer = _db.MatchClubPlayers.FirstOrDefault(x => x.Id == matchPlayerId);

            if (matchPlayer == null)
                return Json(new { ok = false, error = "Səhv müraciət." });


            if (RefereeConfirmStatus(matchPlayer.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            PlayerTournamentPenalty playerTournamentPenalty = null;


            switch (penaltyCardType)
            {
                case PenaltyCardTypeEnum.Yellow:
                    //check if exist and delete from PlayerTournamentPenalties
                    playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.MatchClubPlayerId == matchPlayerId && (x.PenaltyCardTypeId == (int)PenaltyCardTypeEnum.Yellow || x.PenaltyCardTypeId == (int)PenaltyCardTypeEnum.Yellow2));

                    matchPlayer.YellowMinute = null;
                    matchPlayer.YellowMinutePlus = null;
                    matchPlayer.YellowOffgameTypeId = null;
                    matchPlayer.YellowReasonId = null;
                    matchPlayer.Yellow2Minute = null;
                    matchPlayer.Yellow2MinutePlus = null;
                    matchPlayer.Yellow2OffgameTypeId = null;
                    matchPlayer.Yellow2ReasonId = null;
                    break;
                case PenaltyCardTypeEnum.Yellow2:
                    //check if exist and delete from PlayerTournamentPenalties
                    playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.MatchClubPlayerId == matchPlayerId && (x.PenaltyCardTypeId == (int)PenaltyCardTypeEnum.Yellow || x.PenaltyCardTypeId == (int)PenaltyCardTypeEnum.Yellow2));





                    matchPlayer.Yellow2Minute = null;
                    matchPlayer.Yellow2MinutePlus = null;
                    matchPlayer.Yellow2OffgameTypeId = null;
                    matchPlayer.Yellow2ReasonId = null;
                    break;
                case PenaltyCardTypeEnum.Red:
                    //check if exist and delete from PlayerTournamentPenalties
                    playerTournamentPenalty = _db.PlayerTournamentPenalties.FirstOrDefault(x => x.MatchClubPlayerId == matchPlayerId && x.PenaltyCardTypeId == (int)PenaltyCardTypeEnum.Red);

                    matchPlayer.RedMinute = null;
                    matchPlayer.RedMinutePlus = null;
                    matchPlayer.RedOffgameTypeId = null;
                    matchPlayer.RedReasonId = null;
                    break;
            }

            //check if exist and delete from PlayerTournamentPenalties




            //if request yellow 1 card exist delete player penalty for second card and check card limit
            if (penaltyCardType == PenaltyCardTypeEnum.Yellow2 && (matchPlayer.YellowMinute != null || matchPlayer.OffGameTypeYellow != null) && playerTournamentPenalty != null)
            {
                _db.Entry(playerTournamentPenalty).State = EntityState.Deleted;
                _db.SaveChanges();

                SqlParameter userId = new SqlParameter("userId", _user.UserId);
                SqlParameter matchClubPlayerId = new SqlParameter("matchClubPlayerId", matchPlayer.Id);
                SqlParameter penaltyCardTypeId = new SqlParameter("penaltyCardTypeId", PenaltyCardTypeEnum.Yellow);
                var check4YellowResult =
                    _db.Database.SqlQuery<int>("CheckCardLimit @userId, @matchClubPlayerId, @penaltyCardTypeId", userId, matchClubPlayerId, penaltyCardTypeId).First();
            }
            else
            {
                if (playerTournamentPenalty != null)
                {
                    _db.Entry(playerTournamentPenalty).State = EntityState.Deleted;
                }

                _db.Entry(matchPlayer).State = EntityState.Modified;
                _db.SaveChanges();
            }




            return Json(new { ok = true });
        }



        [HttpGet]
        public JsonResult CardReasons()
        {
            var goalTypes = _db.CardReasons
                .OrderBy(x => x.Id)
                .Select(x => new Select2Data
                {
                    id = x.Id,
                    text = x.Name
                }).ToList();

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = goalTypes,
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddMatchCard(int matchId)
        {

            ViewBag.MatchId = matchId;
            return View();
        }

        [HttpPost]
        public ActionResult AddMatchCard(RefereeAddMatchCard addMatchCard)
        {

            if (RefereeConfirmStatus(addMatchCard.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });


            var matchPlayers = _db.MatchClubPlayers.Where(x =>
                !x.IsDeleted &&
                x.MatchId == addMatchCard.MatchId && x.PlayerId == addMatchCard.PlayerId).ToList();

            if (matchPlayers.Count == 0)
                return Json(new { ok = false, error = "Maç və oyunçu düz seçilməyib." },
                    JsonRequestBehavior.AllowGet);

            if (matchPlayers.Count > 1)
                return Json(new { ok = false, error = "Sistem uyğunsuzluğu! Təkrar sətrlər." },
                    JsonRequestBehavior.AllowGet);

            var matchPlayer = matchPlayers.FirstOrDefault();

            int? minutePlus = null;
            if (addMatchCard.MinutePlus != 0)
                minutePlus = addMatchCard.MinutePlus;

            if (addMatchCard.CardType.Equals("yellow"))
            {
                if (matchPlayer.YellowMinute == null)
                {

                    if (addMatchCard.OffGameTypeId.HasValue)
                    {
                        matchPlayer.YellowMinute = null;
                        matchPlayer.YellowMinutePlus = null;
                        matchPlayer.YellowOffgameTypeId = addMatchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchPlayer.YellowMinute = addMatchCard.Minute;
                        matchPlayer.YellowMinutePlus = minutePlus;
                    }
                    matchPlayer.YellowReasonId = addMatchCard.CardReasonId;
                    _db.SaveChanges();


                    //check yellow count from other matches and add to PlayerTournamentPenalties table
                    SqlParameter userId = new SqlParameter("userId", _user.UserId);
                    SqlParameter matchClubPlayerId = new SqlParameter("matchClubPlayerId", matchPlayer.Id);
                    SqlParameter penaltyCardTypeId = new SqlParameter("penaltyCardTypeId", PenaltyCardTypeEnum.Yellow);
                    var check4YellowResult =
                        _db.Database.SqlQuery<int>("CheckCardLimit @userId, @matchClubPlayerId, @penaltyCardTypeId", userId, matchClubPlayerId, penaltyCardTypeId).First();




                }
                else if (matchPlayer.Yellow2Minute == null)
                {

                    if (addMatchCard.OffGameTypeId.HasValue)
                    {
                        matchPlayer.Yellow2Minute = null;
                        matchPlayer.Yellow2MinutePlus = null;
                        matchPlayer.Yellow2OffgameTypeId = addMatchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchPlayer.Yellow2Minute = addMatchCard.Minute;
                        matchPlayer.Yellow2MinutePlus = minutePlus;
                    }
                    matchPlayer.Yellow2ReasonId = addMatchCard.CardReasonId;
                    _db.SaveChanges();


                    //check yellow count from other matches and add to PlayerTournamentPenalties table(check table if already penalty exist
                    SqlParameter userId = new SqlParameter("userId", _user.UserId);
                    SqlParameter matchClubPlayerId = new SqlParameter("matchClubPlayerId", matchPlayer.Id);
                    SqlParameter penaltyCardTypeId = new SqlParameter("penaltyCardTypeId", PenaltyCardTypeEnum.Yellow2);
                    var check4YellowResult =
                        _db.Database.SqlQuery<int>("CheckCardLimit @userId, @matchClubPlayerId, @penaltyCardTypeId", userId, matchClubPlayerId, penaltyCardTypeId).First();
                }
                else
                {
                    return Json(new { ok = false, error = "Maçda eyni oyunçuya 2-dən artıq sarı vərəqə seçmək olmaz!" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else if (addMatchCard.CardType.Equals("red"))
            {
                if (matchPlayer.Yellow2Minute != null)
                {
                    return Json(new { ok = false, error = "Səhv müraciət! İki sarı varəqə almış oyunçuya qırmızı vərəqə seçilə bilməz." },
                        JsonRequestBehavior.AllowGet);
                }

                if (matchPlayer.RedMinute == null)
                {
                    //add PlayerTournamentPenalties table
                    if (addMatchCard.OffGameTypeId.HasValue)
                    {
                        matchPlayer.RedMinute = null;
                        matchPlayer.RedMinutePlus = null;
                        matchPlayer.RedOffgameTypeId = addMatchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchPlayer.RedMinute = addMatchCard.Minute;
                        matchPlayer.RedMinutePlus = minutePlus;
                    }
                    matchPlayer.RedReasonId = addMatchCard.CardReasonId;
                    _db.SaveChanges();


                    //check yellow count from other matches and add to PlayerTournamentPenalties table(check table if already penalty exist
                    SqlParameter userId = new SqlParameter("userId", _user.UserId);
                    SqlParameter matchClubPlayerId = new SqlParameter("matchClubPlayerId", matchPlayer.Id);
                    SqlParameter penaltyCardTypeId = new SqlParameter("penaltyCardTypeId", PenaltyCardTypeEnum.Red);
                    var check4YellowResult =
                        _db.Database.SqlQuery<int>("CheckCardLimit @userId, @matchClubPlayerId, @penaltyCardTypeId", userId, matchClubPlayerId, penaltyCardTypeId).First();
                }
                else
                {
                    return Json(new { ok = false, error = "Maçda eyni oyunçuya 1-dən artıq qırmızı vərəqə seçmək olmaz!" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { ok = false, error = "Cərimə vərəqəsinin növü seçilməyib." },
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult EditMatchCard(int id, int matchId, PenaltyCardTypeEnum cardType)
        {
            var matchCard = _db.MatchClubPlayers
                .Include(x => x.CardReasonYellow1)
                .Include(x => x.CardReasonYellow2)
                .Include(x => x.CardReasonRed)
                .Where(x => x.Id == id && x.MatchId == matchId)
                .Select(x => new RefereeAddMatchCard()
                {
                    Id = x.Id,
                    MatchId = x.MatchId,
                    PlayerId = x.PlayerId,
                    PlayerFirstName = x.Player.FirstName,
                    PlayerLastName = x.Player.LastName,
                    PlayerFatherName = x.Player.FatherName,
                    Minute = cardType == PenaltyCardTypeEnum.Yellow
                        ? x.YellowMinute
                        : (cardType == PenaltyCardTypeEnum.Yellow2
                            ? x.Yellow2Minute
                            : (cardType == PenaltyCardTypeEnum.Red ? x.RedMinute : 0)),
                    MinutePlus = cardType == PenaltyCardTypeEnum.Yellow
                        ? x.YellowMinutePlus
                        : (cardType == PenaltyCardTypeEnum.Yellow2
                            ? x.Yellow2MinutePlus
                            : (cardType == PenaltyCardTypeEnum.Red ? x.RedMinutePlus : 0)),
                    CardReasonId = cardType == PenaltyCardTypeEnum.Yellow
                        ? x.CardReasonYellow1.Id
                        : (cardType == PenaltyCardTypeEnum.Yellow2
                            ? x.CardReasonYellow2.Id
                            : (cardType == PenaltyCardTypeEnum.Red ? x.CardReasonRed.Id : 0)),
                    CardReasonName = cardType == PenaltyCardTypeEnum.Yellow
                        ? x.CardReasonYellow1.Name
                        : (cardType == PenaltyCardTypeEnum.Yellow2
                            ? x.CardReasonYellow2.Name
                            : (cardType == PenaltyCardTypeEnum.Red ? x.CardReasonRed.Name : ""))
                })
                .FirstOrDefault();

            if (matchCard != null)
            {
                if (cardType == PenaltyCardTypeEnum.Red)
                {
                    matchCard.CardType = "Qırmızı";
                }
                else
                {
                    matchCard.CardType = "Sarı";
                }
            }

            ViewBag.MatchId = matchId;
            ViewBag.CardTypeValue = cardType;
            return View("AddMatchCard", matchCard);
        }



        [HttpPost]
        public ActionResult EditMatchCard(RefereeAddMatchCard matchCard)
        {
            if (RefereeConfirmStatus(matchCard.MatchId))
                return Json(new { ok = false, error = "Oyun təsdiqlənmişdir. Dəyişikliyə icazə verilmir." });

            var matchClubPlayer =
                _db.MatchClubPlayers.FirstOrDefault(x => x.Id == matchCard.Id && x.MatchId == matchCard.MatchId);

            if (matchClubPlayer == null)
                return Json(new { ok = false, error = "Səhv müraciət!" });


            switch (matchCard.CardType)
            {
                case "Yellow":
                    if (matchCard.OffGameTypeId.HasValue)
                    {
                        matchClubPlayer.YellowMinute = null;
                        matchClubPlayer.YellowMinutePlus = null;
                        matchClubPlayer.YellowOffgameTypeId = matchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchClubPlayer.YellowMinute = matchCard.Minute;
                        matchClubPlayer.YellowMinutePlus = matchCard.MinutePlus;
                        matchClubPlayer.YellowOffgameTypeId = null;
                    }
                    matchClubPlayer.YellowReasonId = matchCard.CardReasonId;
                    break;
                case "Yellow2":
                    if (matchCard.OffGameTypeId.HasValue)
                    {
                        matchClubPlayer.Yellow2Minute = null;
                        matchClubPlayer.Yellow2MinutePlus = null;
                        matchClubPlayer.Yellow2OffgameTypeId = matchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchClubPlayer.Yellow2Minute = matchCard.Minute;
                        matchClubPlayer.Yellow2MinutePlus = matchCard.MinutePlus;
                        matchClubPlayer.Yellow2OffgameTypeId = null;
                    }
                    matchClubPlayer.Yellow2ReasonId = matchCard.CardReasonId;
                    break;
                case "Red":
                    if (matchCard.OffGameTypeId.HasValue)
                    {
                        matchClubPlayer.RedMinute = null;
                        matchClubPlayer.RedMinutePlus = null;
                        matchClubPlayer.RedOffgameTypeId = matchCard.OffGameTypeId;
                    }
                    else
                    {
                        matchClubPlayer.RedMinute = matchCard.Minute;
                        matchClubPlayer.RedMinutePlus = matchCard.MinutePlus;
                        matchClubPlayer.RedOffgameTypeId = null;
                    }
                    matchClubPlayer.RedReasonId = matchCard.CardReasonId;
                    break;
            }


            matchClubPlayer.LastUpdateById = _user.UserId;
            matchClubPlayer.LastUpdatedDate = DateTime.Now;

            _db.SaveChanges();


            return Json(new { ok = true });
        }




        #endregion


        private bool RefereeConfirmStatus(int matchId)
        {
            var match = _db.Matches.Where(x => x.Id == matchId)
                .Select(x => new
                {
                    x.RefereeConfirm,
                })
                .FirstOrDefault();

            if (match == null)
                return false;


            return match.RefereeConfirm;
        }
    }
}