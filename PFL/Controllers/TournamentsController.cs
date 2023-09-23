using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Entities.EntityModels.Views;
using PFL.Membership;
using PFL.Models.DTO;
using PFL.Models.Enums;
using PFL.Models.ViewModels;
using PFL.Utils;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin,referee")]
    public class TournamentsController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public TournamentsController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Turnirlər";
            ViewBag.BaseUrl = "Tournaments";
        }



        // GET: Tournaments
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var tournaments = _db.Tournaments
                                .Include(t => t.TournamentType)
                                .OrderByDescending(x => x.Id)
                                .ToPagedList(pageIndex, pageSize);
            return View(tournaments);
        }

        // GET: Tournaments/Details/5
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = _db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }

            return View(tournament);
        }

        // GET: Tournaments/Create
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.TournamentTypeId = new SelectList(_db.TournamentTypes.Where(x => !x.IsDeleted), "Id", "Description");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Create(Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                tournament.SeasonId = SeasonHelper.GetCurrentSeasonId();
                tournament.CreatedById = _user.UserId;
                tournament.CreationDate = DateTime.Now;
                tournament.Status = 1;
                tournament.Completed = false;

                _db.Tournaments.Add(tournament);
                _db.SaveChanges();


                return RedirectToAction("Index");
            }

            ViewBag.TournamentTypeId = new SelectList(_db.TournamentTypes.Where(x => !x.IsDeleted), "Id", "Name", tournament.TournamentTypeId);
            return View(tournament);
        }




        // GET: Tournaments/Edit/5
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = _db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.TournamentTypeId = new SelectList(_db.TournamentTypes.Where(x => !x.IsDeleted), "Id", "Description", tournament.TournamentTypeId);
            return View("Create", tournament);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,TournamentTypeId,SeasonStartYear,SeasonEndYear,YellowCardLimit,YoungPlayerAge21Limit,YoungPlayerAge21Count,YoungPlayerAge19Limit,YoungPlayerAge19Count,LegionerLimit,PlayerCount,ReservePlayerCount,Period")] Tournament tournamentDto)
        {
            if (ModelState.IsValid)
            {
                var tournament = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentDto.Id);

                if (tournament == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                tournament.Name = tournamentDto.Name;
                tournament.TournamentTypeId = tournamentDto.TournamentTypeId;
                tournament.SeasonStartYear = tournamentDto.SeasonStartYear;
                tournament.SeasonEndYear = tournamentDto.SeasonEndYear;
                tournament.YellowCardLimit = tournamentDto.YellowCardLimit;
                tournament.YoungPlayerAge21Limit = tournamentDto.YoungPlayerAge21Limit;
                tournament.YoungPlayerAge21Count = tournamentDto.YoungPlayerAge21Count;
                tournament.YoungPlayerAge19Limit = tournamentDto.YoungPlayerAge19Limit;
                tournament.YoungPlayerAge19Count = tournamentDto.YoungPlayerAge19Count;
                tournament.LegionerLimit = tournamentDto.LegionerLimit;
                tournament.PlayerCount = tournamentDto.PlayerCount;
                tournament.ReservePlayerCount = tournamentDto.ReservePlayerCount;
                tournament.Period = tournamentDto.Period;

                tournament.Status = 1;
                tournament.Completed = false;

                _db.Entry(tournament).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TournamentTypeId = new SelectList(_db.TournamentTypes.Where(x => !x.IsDeleted), "Id", "Name", tournamentDto.TournamentTypeId);
            return View("Create", tournamentDto);
        }



        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Complete(int id)
        {
            var tournament = _db.Tournaments.FirstOrDefault(x => x.Id == id);

            if (tournament == null)
                return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);

            if (_db.TournamentTours.Any(x => x.TournamentId == id && x.MatchId == null && !x.IsDeleted))
                return Json(new { ok = false, error = "Turnirdə yekunlaşdırılmamış oyunlar olduğundan turniri yekunlaşdırıla bilməz." }, JsonRequestBehavior.AllowGet);


            tournament.Completed = true;
            _db.SaveChanges();

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }


        // POST: Tournaments/Delete/5
        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournament tournament = _db.Tournaments.Find(id);
            if (tournament != null)
            {
                tournament.LastUpdateById = _user.UserId;
                tournament.LastUpdatedDate = DateTime.Now;
                tournament.IsDeleted = true;
                _db.Entry(tournament).State = EntityState.Modified;
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        // POST: TournamentTypes/Undo/5
        [HttpGet, ActionName("Undo")]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Undo(int id)
        {
            Tournament tournament = _db.Tournaments.Find(id);
            if (tournament != null)
            {
                tournament.LastUpdateById = _user.UserId;
                tournament.LastUpdatedDate = DateTime.Now;
                tournament.IsDeleted = false;
                _db.Entry(tournament).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [CustomAuthorize(Roles = "admin")]
        public ActionResult Clubs(int tournamentId)
        {
            var clubs = _db.TournamentClubs
                .Include(x => x.Club)
                .Include(x => x.Club.District)
                .Where(x => !x.IsDeleted && x.TournamentId == tournamentId)
                .Select(x => x.Club);


            ViewBag.HasConfirmed = _db.TournamentTours.Any(x => x.TournamentId == tournamentId && !x.IsDeleted);


            return View(clubs.ToList());
        }

        [CustomAuthorize(Roles = "admin")]
        public ActionResult RowClub(int tournamentClubId)
        {
            var clubs = _db.TournamentClubs
                .Include(x => x.Club)
                .Include(x => x.Club.District)
                .Where(x => !x.IsDeleted && x.Id == tournamentClubId)
                .Select(x => x.Club)
                .FirstOrDefault();

            return View(clubs);
        }





        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult AddClub(int tournamentId)
        {
            ViewBag.TournamentId = tournamentId;
            ViewBag.ClubId = new SelectList(_db.Clubs.Where(x => !x.IsDeleted), "Id", "Name");
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult AddClub([Bind(Include = "TournamentId, ClubId")] TournamentClub tournamentClub)
        {
            if (_db.TournamentClubs.Count(x =>
                    !x.IsDeleted &&
                    x.TournamentId == tournamentClub.TournamentId && x.ClubId == tournamentClub.ClubId) > 0)
                return Json(new { ok = false, error = "Eyni klub turnirə iki dəfə daxil edilə bilməz!" });

            if (_db.TournamentTours.Any(x => x.TournamentId == tournamentClub.TournamentId && !x.IsDeleted))
                return Json(new { ok = false, error = "Siyahı təsdiqləndikdən sonra dəyişiklik və ya əlavə etmək olmaz!" });


            tournamentClub.CreationDate = DateTime.Now;
            tournamentClub.CreatedById = _user.UserId;

            _db.TournamentClubs.Add(tournamentClub);
            _db.SaveChanges();

            //return new HttpStatusCodeResult(HttpStatusCode.Created);
            return Json(new { ok = true, Id = tournamentClub.Id });
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult RemoveClub(int tournamentId, int clubId)
        {
            TournamentClub tournamentClub =
                _db.TournamentClubs.FirstOrDefault(x => !x.IsDeleted && x.TournamentId == tournamentId && x.ClubId == clubId);

            if (tournamentClub != null)
            {
                //tournamentClub.LastUpdatedDate = DateTime.Now;
                //tournamentClub.LastUpdateById = _user.UserId;
                //tournamentClub.IsDeleted = true;

                _db.TournamentClubs.Remove(tournamentClub);
                _db.SaveChanges();

                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }



        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult ConfirmTournamentClubs(int tournamentId)
        {
            Tournament tournament = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);

            if (tournament == null)
                return HttpNotFound();

            int clubCount = _db.TournamentClubs.Count(x => !x.IsDeleted && x.TournamentId == tournamentId);

            //TournamentTours cedveline insert etmeden once hemin cedvelde datalarin oldugunu yoxlamaq lazimdi. varsa yazmiriq!
            if (_db.TournamentTours.Any(x => x.TournamentId == tournamentId && !x.IsDeleted))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);


            int tourCount = (clubCount - 1) * tournament.Period;
            int matchCountInTour = (clubCount + (clubCount % 2)) / 2;

            for (int i = 1; i <= tourCount; i++)
            {
                for (int j = 1; j <= matchCountInTour; j++)
                {
                    TournamentTour tournamentTour = new TournamentTour()
                    {
                        TourNumber = i,
                        TournamentId = tournamentId,
                        CreationDate = DateTime.Now,
                        CreatedById = _user.UserId
                    };
                    _db.TournamentTours.Add(tournamentTour);
                }
            }

            _db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Table(int tournamentId)
        {
            //var tournamentTable = _db.VTournamentTables.Where(x => x.TournamentId == tournamentId);
            SqlParameter paramTournamentId = new SqlParameter("tournamentId", tournamentId);
            var tournamentTable =
                _db.Database.SqlQuery<VTournamentTable>("TournamentTable @tournamentId", paramTournamentId).OrderByDescending(x => x.WinCount * 3 + x.ScorelessCount).ThenByDescending(x=>x.GoalCountTo-x.GoalCountFrom).ToList();

            return View(tournamentTable);

            #region TournamentTableViewExample

            //List<VTournamentTable> tournamentTableViewModels = new List<VTournamentTable>()
            //{
            //    new VTournamentTable()
            //    {
            //        ClubId = 4,
            //        ClubName = "Keşlə PFK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 5,
            //        ClubName = "Neftçi PFK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 6,
            //        ClubName = "Qarabağ FK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 7,
            //        ClubName = "Qəbələ İK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 8,
            //        ClubName = "Sabah",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 9,
            //        ClubName = "Sumqayıt FK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 10,
            //        ClubName = "Səbail PFK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    },
            //    new VTournamentTable()
            //    {
            //        ClubId = 1002,
            //        ClubName = "Zirə PFK",
            //        PlayedGame = 4,
            //        DefeatCount = 3,
            //        GoalCountFrom = 3,
            //        GoalCountTo = 4,
            //        GoalDifference = 1,
            //        Score = 3,
            //        ScorelessCount = 0,
            //        WinCount = 1
            //    }
            //};

            //return View(tournamentTableViewModels);

            #endregion


        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult PlayerTable(int tournamentId)
        {

            var tournamentPlayerTable = _db.VTournamentPlayerTables.Where(x => x.TournamentId == tournamentId)
                .OrderByDescending(x => x.PlayerGoalCount).ToList();

            return View(tournamentPlayerTable);

            #region PlayerTableViewExample

            //List<VTournamentPlayerTable> tournamentPlayerTableViewModels = new List<VTournamentPlayerTable>()
            //{
            //    new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Mədətov Mahir",
            //        ClubName = "Qarabağ FK",
            //        PlayerGoalCount = 7
            //    },
            //    new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Devich Marko",
            //        ClubName = "Sabah",
            //        PlayerGoalCount = 7
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Dabo Baqali",
            //        ClubName = "Neftçi PFK",
            //        PlayerGoalCount = 7
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Abbasov Mirabdulla",
            //        ClubName = "Neftçi PFK",
            //        PlayerGoalCount = 6
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Adeniyi Segun James",
            //        ClubName = "Qəbələ İK",
            //        PlayerGoalCount = 6
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Emeqara İnnosent",
            //        ClubName = "Qarabağ FK",
            //        PlayerGoalCount = 5
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Mahmudov Emin",
            //        ClubName = "Neftçi PFK",
            //        PlayerGoalCount = 5
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Josef-Monros Steven",
            //        ClubName = "Qəbələ İK",
            //        PlayerGoalCount = 5
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Ozobiç Filip",
            //        ClubName = "Qarabağ FK",
            //        PlayerGoalCount = 5
            //    },new VTournamentPlayerTable()
            //    {
            //        PlayerFullName = "Quintana Soso Daniel",
            //        ClubName = "Qarabağ FK",
            //        PlayerGoalCount = 4
            //    }
            //};

            //return View(tournamentPlayerTableViewModels);

            #endregion


        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult CardStatistics(int tournamentId)
        {

            //var tournamentClubs =
            //    _db.TournamentClubs.Where(x => !x.IsDeleted && x.TournamentId == tournamentId).Select(x => new { TournamentId = x.TournamentId, ClubId = x.ClubId, ClubName = x.Club.Name }).ToList();

            //var tournamentCardStatistics = _db.VTournamentCardStatistics.Where(x => x.TournamentId == tournamentId).ToList();


            //var statistic = new List<VTournamentCardStatistic>();

            //foreach (var item in tournamentClubs)
            //{
            //    var tournamentClub = tournamentCardStatistics.FirstOrDefault(x => x.ClubId == item.ClubId);

            //    var statModel = new VTournamentCardStatistic()
            //    {
            //        TournamentId = item.TournamentId,
            //        ClubId = item.ClubId,
            //        ClubName = item.ClubName
            //    };

            //    if (tournamentClub != null)
            //    {
            //        //statModel.ClubName = tournamentClub.ClubName;
            //        statModel.ClubGoalCount = tournamentClub.ClubGoalCount;
            //        statModel.YellowCardCount = tournamentClub.YellowCardCount;
            //        statModel.DoubleYellowCardCount = tournamentClub.DoubleYellowCardCount;
            //        statModel.RedCardCount = tournamentClub.RedCardCount;
            //    }
            //    statistic.Add(statModel);
            //}


            SqlParameter paramTournamentId = new SqlParameter("tournamentId", tournamentId);
            var statistics =
                _db.Database.SqlQuery<VTournamentCardStatistic>("TournamentCardStatistics @tournamentId", paramTournamentId).OrderByDescending(x => x.YellowCardCount).ToList();

            return View(statistics);

            #region CardStatisticsViewExample

            //List<VTournamentCardStatistic> cardStatisticViewModels = new List<VTournamentCardStatistic>()
            //{
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Keşlə PFK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 49,
            //        DoubleYellowCardCount = 2,
            //        RedCardCount = 0
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Neftçi PFK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 35,
            //        DoubleYellowCardCount = 4,
            //        RedCardCount = 0
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Qarabağ FK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 30,
            //        DoubleYellowCardCount = 1,
            //        RedCardCount = 0
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Qəbələ İK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 39,
            //        DoubleYellowCardCount = 3,
            //        RedCardCount = 1
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Sabah",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 37,
            //        DoubleYellowCardCount = 4,
            //        RedCardCount = 2
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Sumqayıt FK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 34,
            //        DoubleYellowCardCount = 3,
            //        RedCardCount = 0
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Səbail PFK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 31,
            //        DoubleYellowCardCount = 4,
            //        RedCardCount = 0
            //    },
            //    new VTournamentCardStatistic()
            //    {
            //        ClubName = "Zirə PFK",
            //        ClubGoalCount = 17,
            //        YellowCardCount = 50,
            //        DoubleYellowCardCount = 1,
            //        RedCardCount = 1
            //    }
            //};

            //return View(cardStatisticViewModels);

            #endregion

        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult ClubCardStatistics(int tournamentId, int clubId)
        {
            //var tournamentPlayers = _db.ClubTournamentPlayers
            //    .Include(x => x.Player)
            //    .Where(x =>
            //    !x.IsDeleted &&
            //    x.TournamentId == tournamentId &&
            //    x.ClubId == clubId)
            //    .Select(x => x.Player)
            //    .ToList();

            var tournament = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);
            if (tournament == null)
                return new HttpNotFoundResult("Səhv müraciət!");


            SqlParameter paramTournamentId = new SqlParameter("tournamentId", tournamentId);
            SqlParameter paramClubId = new SqlParameter("clubId", clubId);


            var tournamentPlayers = _db.Database
                .SqlQuery<ClubPlayerOrderByTournament>("ClubPlayerOrderByTournament @tournamentId, @clubId",
                    paramTournamentId, paramClubId).ToList();

            //_db.ClubPlayerOrders
            //.Include(x => x.Player)
            //.Where(x =>
            //!x.IsDeleted &&
            //x.ClubConfirm && x.OperatorConfirm && !x.Rejected &&
            //x.CreationDate >= new DateTime(tournament.SeasonStartYear, 7, 1) &&
            //x.CreationDate < new DateTime(tournament.SeasonEndYear, 6, 30) &&
            //x.ClubId == clubId)
            //.Select(x => x.Player)
            //.ToList();

            SqlParameter param1TournamentId = new SqlParameter("tournamentId", tournamentId);
            SqlParameter param1ClubId = new SqlParameter("clubId", clubId);

            var tournamentTours =
                _db.Database.SqlQuery<TourMatchViewModel>("TourNumbersForClub @tournamentId, @clubId", param1TournamentId, param1ClubId).ToList();



            var playerCardStatistics = (from tt in _db.TournamentTours
                                        join mcp in _db.MatchClubPlayers on tt.MatchId equals mcp.MatchId into mcps
                                        from mcp in mcps.DefaultIfEmpty()
                                        where !mcp.IsDeleted &&
                                              tt.TournamentId == tournamentId &&
                                              mcp.ClubId == clubId
                                        select new PlayerCardStatisticViewModel()
                                        {
                                            TourNumber = tt.TourNumber,
                                            MatchId = mcp.MatchId,
                                            MatchDate = mcp.Match.MatchDate,
                                            PlayerId = mcp.PlayerId,
                                            YellowMinute = mcp.YellowMinute,
                                            YellowOffgameTypeId = mcp.YellowOffgameTypeId,
                                            Yellow2Minute = mcp.Yellow2Minute,
                                            Yellow2OffgameTypeId = mcp.Yellow2OffgameTypeId,
                                            RedMinute = mcp.RedMinute,
                                            RedOffgameTypeId = mcp.RedOffgameTypeId,
                                        }).ToList();

            //public PenaltyCardType PenaltyCardType { get; set; }

            ViewBag.TournamentPlayers = tournamentPlayers;
            ViewBag.TournamentTours = tournamentTours;

            return View(playerCardStatistics);
        }



        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Tours(int tournamentId, int? tourNumber)
        {

            ToursViewModel toursViewModel = new ToursViewModel()
            {
                TournamentId = tournamentId
            };

            var tourNumbers = _db.TournamentTours.Where(x => x.TournamentId == tournamentId).Select(x => new Tour
            { TourNumber = x.TourNumber, TourLabel = x.TourLabel })
                .Distinct().ToList();

            toursViewModel.TourNumbers = tourNumbers;

            if (tourNumber == null)
            {
                var _tourNumbers = _db.TournamentTours.Where(x => x.TournamentId == tournamentId && x.MatchId != null)
                    .Select(x => x.TourNumber).Distinct().ToList();

                tourNumber = _tourNumbers.Count == 0 ? 1 : _tourNumbers.Max();
            }

            toursViewModel.CurrentTourNumber = tourNumber.Value;


            return View("Tours", toursViewModel);
        }


        //[CustomAuthorize(Roles = "admin,referee")]
        [AllowAnonymous]
        public ActionResult MatchDocument(int matchId, bool isPdf = false)
        {
            //var documentDetail = _db.TournamentTours
            //    .Include(x => x.Tournament)
            //    .Include(x => x.Match)
            //    .Include(x => x.Match.HomeClub)
            //    .Include(x => x.Match.GuestClub)
            //    .Include(x => x.Match.MatchClubPlayers)
            //    .Include(x => x.Match.Referee)
            //    .Include(x => x.Match.Referee.District)
            //    .FirstOrDefault(x => x.MatchId == matchId);
            ViewBag.IsPdf = isPdf;


            MatchDocumentViewModel matchDocumentViewModel = new MatchDocumentViewModel();

            #region Match Info
            //public bool HomeClubTechnicalDefeat { get; set; }
            //public string HomeClubTechnicalDefeatNote { get; set; }
            //public bool GuestClubTechnicalDefeat { get; set; }
            //public string GuestClubTechnicalDefeatNote { get; set; }
        var matchInfo = _db.TournamentTours
                .Include(x => x.Tournament)
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub)
                .Include(x => x.Match.GuestClub)
                .Include(x => x.Match.Stadium)
                .Include("Match.Stadium.District")
                .Where(x => x.MatchId == matchId)
                .Select(x => new MatchInfo()
                {
                    SeasonId = x.Tournament.SeasonId,
                    TournamentId = x.TournamentId,
                    TournamentName = x.Tournament.Name,
                    TournamentTypeId = x.Tournament.TournamentTypeId,
                    TournamentSeasonStartYear = x.Tournament.SeasonStartYear,
                    TournamentSeasonEndYear = x.Tournament.SeasonEndYear,
                    TourNumber = x.TourNumber,
                    TourLabel = x.TourLabel,
                    HomeClubTechnicalDefeat=x.Match.HomeClubTechnicalDefeat,
                    HomeClubTechnicalDefeatNote = x.Match.HomeClubTechnicalDefeatNote,
                    GuestClubTechnicalDefeat = x.Match.GuestClubTechnicalDefeat,
                    GuestClubTechnicalDefeatNote = x.Match.GuestClubTechnicalDefeatNote,
                    HomeClubId = x.Match.HomeClubId,
                    HomeClubName = x.Match.HomeClub.Name,
                    GuestClubId = x.Match.GuestClubId,
                    GuestClubName = x.Match.GuestClub.Name,
                    MatchDate = x.Match.MatchDate,
                    Place = x.Match.Stadium.District.Name,//"Bakı",
                    Stadium = x.Match.Stadium.Name,
                    StadiumAudienceCount = x.Match.AudienceCount,
                    HalfTimeExtra = x.Match.HalfExtraTime,
                    FullTimeExtra = x.Match.FullExtraTime,
                    Half2TimeExtra = x.Match.Half2ExtraTime,
                    Full2TimeExtra = x.Match.Full2ExtraTime,
                    CupNote = x.Match.CupNote,
                    RefereeNote = x.Match.RefereeNote,
                    HomeClubNote = x.Match.HomeClubNote,
                    GuestClubNote = x.Match.GuestClubNote,
                    YoungPlayerAge21Limit = x.Tournament.YoungPlayerAge21Limit,
                    YoungPlayerAge19Limit = x.Tournament.YoungPlayerAge19Limit
                })
                .FirstOrDefault();

            matchDocumentViewModel.MatchInfo = matchInfo;

            #endregion


            #region  Match Officials

            var matchOfficials = _db.Matches
                .Include(x => x.Referee)
                .Include(x => x.RefereeAssistant1)
                .Include(x => x.RefereeAssistant2)
                .Include(x => x.FourthReferee)
                .Include(x => x.AdditionalReferee1)
                .Include(x => x.AdditionalReferee2)
                .Include(x => x.RefereeVar)
                .Include(x => x.RefereeAvar)
                .Where(x => x.Id == matchId)
                .Select(x =>
                new MatchOfficials()
                {
                    Referee = new MatchOfficial()
                    {
                        FirstName = x.Referee.FirstName,
                        LastName = x.Referee.LastName,
                        Grade = x.Referee.Grade,
                        City = x.Referee.District.Name
                    },
                    RefereeAssistant1 = new MatchOfficial()
                    {
                        FirstName = x.RefereeAssistant1.FirstName,
                        LastName = x.RefereeAssistant1.LastName,
                        Grade = x.RefereeAssistant1.Grade,
                        City = x.RefereeAssistant1.District.Name
                    },
                    RefereeAssistant2 = new MatchOfficial()
                    {
                        FirstName = x.RefereeAssistant2.FirstName,
                        LastName = x.RefereeAssistant2.LastName,
                        Grade = x.RefereeAssistant2.Grade,
                        City = x.RefereeAssistant2.District.Name
                    },
                    FourthReferee = new MatchOfficial()
                    {
                        FirstName = x.FourthReferee.FirstName,
                        LastName = x.FourthReferee.LastName,
                        Grade = x.FourthReferee.Grade,
                        City = x.FourthReferee.District.Name
                    },
                    AdditionalReferee1 = new MatchOfficial()
                    {
                        FirstName = x.AdditionalReferee1.FirstName,
                        LastName = x.AdditionalReferee1.LastName,
                        Grade = x.AdditionalReferee1.Grade,
                        City = x.AdditionalReferee1.District.Name
                    },
                    AdditionalReferee2 = new MatchOfficial()
                    {
                        FirstName = x.AdditionalReferee2.FirstName,
                        LastName = x.AdditionalReferee2.LastName,
                        Grade = x.AdditionalReferee2.Grade,
                        City = x.AdditionalReferee2.District.Name
                    },
                    RefereeVar = new MatchOfficial()
                    {
                        FirstName = x.RefereeVar.FirstName,
                        LastName = x.RefereeVar.LastName,
                        Grade = x.RefereeVar.Grade,
                        City = x.RefereeVar.District.Name
                    },
                    RefereeAvar = new MatchOfficial()
                    {
                        FirstName = x.RefereeAvar.FirstName,
                        LastName = x.RefereeAvar.LastName,
                        Grade = x.RefereeAvar.Grade,
                        City = x.RefereeAvar.District.Name
                    },
                    AffaRepresentative = new MatchOfficial()
                    {
                        FirstName = x.AffaRepresentative.FirstName,
                        LastName = x.AffaRepresentative.LastName,
                        Grade = x.AffaRepresentative.Grade,
                        City = x.AffaRepresentative.District.Name
                    },
                    RefereeInspector = new MatchOfficial()
                    {
                        FirstName = x.RefereeInspector.FirstName,
                        LastName = x.RefereeInspector.LastName,
                        Grade = x.RefereeInspector.Grade,
                        City = x.RefereeInspector.District.Name
                    },
                })
                .FirstOrDefault();

            matchDocumentViewModel.MatchOfficials = matchOfficials;

            #endregion


            #region Match Score

            var goals = _db.MatchResults
                .Include(x => x.Match)
                .Where(x => x.MatchId == matchId && !x.IsDeleted)
                .Select(x => new
                {
                    HomeClubId = x.Match.HomeClubId,
                    GuestClubId = x.Match.GuestClubId,
                    ClubId = x.ClubId,
                    Minute = x.Minute,
                    GoalTypeId = x.GoalTypeId
                });


            MatchScore matchScoreHalf = new MatchScore();
            MatchScore matchScoreFull = new MatchScore();
            List<int?> plusGoalTypes = new List<int?>() { 1, 2, 5 };

            foreach (var item in goals)
            {
                //HomeScore
                if ((item.HomeClubId == item.ClubId && plusGoalTypes.Contains(item.GoalTypeId)) || (item.GuestClubId == item.ClubId && item.GoalTypeId == 3))
                {
                    matchScoreFull.HomeClubScore++;
                    if (item.Minute <= 45)
                    {
                        matchScoreHalf.HomeClubScore++;
                    }
                }

                //GuestScore
                if ((item.GuestClubId == item.ClubId && plusGoalTypes.Contains(item.GoalTypeId)) || (item.HomeClubId == item.ClubId && item.GoalTypeId == 3))
                {
                    matchScoreFull.GuestClubScore++;
                    if (item.Minute <= 45)
                    {
                        matchScoreHalf.GuestClubScore++;
                    }
                }
            }

            MatchScoreInfo matchScoreInfo = new MatchScoreInfo()
            {
                HalfTimeScore = matchScoreHalf,
                FullTimeScore = matchScoreFull
            };

            matchDocumentViewModel.MatchScoreInfo = matchScoreInfo;


            #endregion

            #region Goals

            var matchGoals = _db.VProtocolMatchResults.Where(x => x.MatchId == matchId).OrderBy(x => x.Minute)
                .ThenBy(x => x.MinutePlus)
                .Select(x => new MatchGoal()
                {
                    ClubId = x.ClubId,
                    ClubName = x.ClubName,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.PlayerNumber,
                    PlayerFirstName = x.PlayerFirstName,
                    PlayerLastName = x.PlayerLastName,
                    Minute = x.Minute,
                    MinutePlus = x.MinutePlus,
                    GoalTypeId = x.GoalTypeId,
                    GoalType = x.GoalType
                })
                .ToList();
            //_db.MatchResults
            //.Include(x => x.Player)
            //.Include(x => x.Club)
            //.Include(x => x.GoalType)
            //.Where(x => !x.IsDeleted && x.MatchId == matchId)
            //.OrderBy(x => x.Minute)
            //.ThenBy(x => x.MinutePlus)
            //.Select(x => new MatchGoal()
            //{
            //    ClubId = x.ClubId,
            //    ClubName = x.Club.Name,
            //    PlayerId = x.PlayerId,
            //    PlayerNumber = _db.ClubPlayerOrders.Where(z => z.ClubId == x.ClubId && z.PlayerId == x.PlayerId).OrderByDescending(z => z.Id).FirstOrDefault().PlayerNumber,//x.Player.PlayerNumber,
            //    PlayerFirstName = x.Player.FirstName,
            //    PlayerLastName = x.Player.LastName,
            //    Minute = x.Minute,
            //    MinutePlus = x.MinutePlus,
            //    GoalTypeId = x.GoalTypeId,
            //    GoalType = x.GoalType.Name
            //}).ToList();


            //if (matchInfo.TournamentId > 24)
            //{
            //    var playerIds = new List<long?>();

            //    playerIds.AddRange(matchGoals.Select(x => x.PlayerId).ToList());

            //    var playerNumbers = _db.ClubPlayerOrders.Where(x =>
            //            x.CreationDate >= new DateTime(matchInfo.TournamentSeasonStartYear, 1, 1) &&
            //            x.CreationDate < new DateTime(matchInfo.TournamentSeasonEndYear, 1, 1) && playerIds.Contains(x.PlayerId))
            //        .Select(x => new { x.PlayerId, x.PlayerNumber })
            //        .ToList();

            //    foreach (var matchGoal in matchGoals)
            //    {
            //        var playerNumber = playerNumbers.FirstOrDefault(x => x.PlayerId == matchGoal.PlayerId);
            //        if (playerNumber != null)
            //            matchGoal.PlayerNumber = playerNumber.PlayerNumber;
            //    }
            //}


            matchDocumentViewModel.MatchGoals = matchGoals;

            #endregion

            #region Penalty Goals

            var matchPenaltyGoals = _db.VProtocolMatchPenaltyResults.Where(x => x.MatchId == matchId)
                .OrderBy(x => x.PenaltyOrder)
                .Select(x => new MatchPenaltyGoal()
                {
                    ClubId = x.ClubId,
                    ClubName = x.ClubName,
                    PlayerId = x.PlayerId,
                    PlayerNumber = x.PlayerNumber,
                    PlayerFirstName = x.PlayerFirstName,
                    PlayerLastName = x.PlayerLastName,
                    PenaltyOrder = x.PenaltyOrder,
                    //MinutePlus = x.MinutePlus,
                    GoalTypeId = x.GoalTypeId,
                    GoalType = x.GoalType
                })
                .ToList();


            matchDocumentViewModel.MatchPenaltyGoals = matchPenaltyGoals;

            #endregion

            #region Player shifts

            var playerShifts = _db.VProtocolPlayerShifts.Where(x => x.MatchId == matchId)
                .Select(x => new MatchPlayerShiftDto()
                {
                    ClubId = x.ClubId,
                    PlayerIdIn = x.PlayerIdIn,
                    PlayerNumberIn = x.PlayerNumberIn,
                    FirstNameIn = x.FirstNameIn,
                    LastNameIn = x.LastNameIn,
                    PlayerIdOut = x.PlayerIdOut,
                    PlayerNumberOut = x.PlayerNumberOut,
                    FirstNameOut = x.FirstNameOut,
                    LastNameOut = x.LastNameOut,
                    Minute = x.Minute,
                    MinutePlus = x.MinutePlus
                })
                .ToList();
            //_db.MatchClubPlayerShifts
            //.Include(x => x.Club)
            //.Include(x => x.Player)
            //.Where(x => !x.IsDeleted && x.MatchId == matchId && x.PlayerOut != null)
            //.Select(x => new MatchPlayerShiftDto()
            //{
            //    ClubId = x.ClubId,
            //    PlayerIdIn = x.PlayerId,
            //    PlayerNumberIn = _db.ClubPlayerOrders.Where(z => z.ClubId == x.ClubId && z.PlayerId == x.PlayerId).OrderByDescending(z => z.Id).FirstOrDefault().PlayerNumber,//x.Player.PlayerNumber,
            //    FirstNameIn = x.Player.FirstName,
            //    LastNameIn = x.Player.LastName,
            //    PlayerIdOut = x.PlayerIdOut,
            //    PlayerNumberOut = x.PlayerOut.PlayerNumber,
            //    FirstNameOut = x.PlayerOut.FirstName,
            //    LastNameOut = x.PlayerOut.LastName,
            //    Minute = x.MinuteIn,
            //    MinutePlus = x.MinuteInPlus
            //})
            //.OrderBy(x => x.Minute)
            //.ToList();



            //if (matchInfo.TournamentId > 24)
            //{
            //    var playerIds = new List<long?>();

            //    playerIds.AddRange(playerShifts.Select(x => x.PlayerIdIn).ToList());
            //    playerIds.AddRange(playerShifts.Select(x => x.PlayerIdIn).ToList());

            //    var playerNumbers = _db.ClubPlayerOrders.Where(x =>
            //            x.CreationDate >= new DateTime(matchInfo.TournamentSeasonStartYear, 1, 1) &&
            //            x.CreationDate < new DateTime(matchInfo.TournamentSeasonEndYear, 1, 1) && playerIds.Contains(x.PlayerId))
            //        .Select(x => new { x.PlayerId, x.PlayerNumber })
            //        .ToList();

            //    foreach (var playerShift in playerShifts)
            //    {
            //        var playerNumberIn = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerIdIn);
            //        if (playerNumberIn != null)
            //            playerShift.PlayerNumberIn = playerNumberIn.PlayerNumber;

            //        var playerNumberOut = playerNumbers.FirstOrDefault(x => x.PlayerId == playerShift.PlayerIdOut);
            //        if (playerNumberOut != null)
            //            playerShift.PlayerNumberOut = playerNumberOut.PlayerNumber;
            //    }
            //}


            matchDocumentViewModel.MatchPlayerShifts = playerShifts;

            #endregion


            #region Player Cards

            var penaltyCards = _db.VProtocolPlayerCards.Where(x => x.MatchId == matchId).ToList();
            //_db.MatchClubPlayers
            //.Include(x => x.Club)
            //.Include(x => x.Player)
            //.Include(x => x.CardReasonYellow1)
            //.Include(x => x.CardReasonYellow2)
            //.Include(x => x.CardReasonRed)
            //.Where(x => !x.IsDeleted &&
            //            x.MatchId == matchId &&
            //            (x.YellowMinute != null || x.Yellow2Minute != null || x.RedMinute != null || x.OffGameTypeYellow != null || x.OffGameTypeYellow2 != null || x.OffGameTypeRed != null))
            //.Select(x => new
            //{
            //    ClubName = x.Club.Name,
            //    PlayerId = x.PlayerId,
            //    PlayerNumber = _db.ClubPlayerOrders.Where(z=>z.ClubId==x.ClubId && z.PlayerId == x.PlayerId).OrderByDescending(z=>z.Id).FirstOrDefault().PlayerNumber,//x.Player.PlayerNumber,
            //    FirstName = x.Player.FirstName,
            //    LastName = x.Player.LastName,
            //    YellowMinute = x.YellowMinute,
            //    YellowMinutePlus = x.YellowMinutePlus,
            //    YellowOffGameTypeName = x.OffGameTypeYellow.Name,
            //    YellowReason = x.CardReasonYellow1.Name,
            //    Yellow2Minute = x.Yellow2Minute,
            //    Yellow2MinutePlus = x.Yellow2MinutePlus,
            //    Yellow2OffGameTypeName = x.OffGameTypeYellow2.Name,
            //    Yellow2Reason = x.CardReasonYellow2.Name,
            //    RedMinute = x.RedMinute,
            //    RedMinutePlus = x.RedMinutePlus,
            //    RedOffGameTypeName = x.OffGameTypeRed.Name,
            //    RedReason = x.CardReasonRed.Name
            //}).ToList();


            List<MatchPenaltyCardDto> matchPenaltyCards = new List<MatchPenaltyCardDto>();
            foreach (var item in penaltyCards)
            {
                if (item.YellowMinute != null || item.YellowOffGameTypeName != null)
                {
                    matchPenaltyCards.Add(new MatchPenaltyCardDto()
                    {
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

                if (item.Yellow2Minute != null || item.Yellow2OffGameTypeName != null)
                {
                    matchPenaltyCards.Add(new MatchPenaltyCardDto()
                    {
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

                if (item.RedMinute != null || item.RedOffGameTypeName != null)
                {
                    matchPenaltyCards.Add(new MatchPenaltyCardDto()
                    {
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


            //if (matchInfo.TournamentId > 24)
            //{
            //    var playerIds = new List<long>();

            //    playerIds.AddRange(matchPenaltyCards.Select(x => x.PlayerId).ToList());

            //    var playerNumbers = _db.ClubPlayerOrders.Where(x =>
            //            x.CreationDate >= new DateTime(matchInfo.TournamentSeasonStartYear, 7, 1) &&
            //            x.CreationDate < new DateTime(matchInfo.TournamentSeasonEndYear, 6, 30) && playerIds.Contains(x.PlayerId))
            //        .Select(x => new { x.PlayerId, x.PlayerNumber })
            //        .ToList();

            //    foreach (var matchPenaltyCard in matchPenaltyCards)
            //    {
            //        var playerNumber = playerNumbers.FirstOrDefault(x => x.PlayerId == matchPenaltyCard.PlayerId);
            //        if (playerNumber != null)
            //            matchPenaltyCard.PlayerNumber = playerNumber.PlayerNumber;
            //    }
            //}


            List<MatchPenaltyCardDto> orderedMatchPenaltyCards = new List<MatchPenaltyCardDto>();

            orderedMatchPenaltyCards.AddRange(matchPenaltyCards.Where(x => x.Minute < 46).OrderBy(x => (x.Minute ?? 0) + (x.MinutePlus ?? 0)));
            orderedMatchPenaltyCards.AddRange(matchPenaltyCards.Where(x => x.Minute > 45).OrderBy(x => (x.Minute ?? 0) + (x.MinutePlus ?? 0)));
            orderedMatchPenaltyCards.AddRange(matchPenaltyCards.Where(x => x.Minute == null));

            matchDocumentViewModel.MatchPenaltyCards = orderedMatchPenaltyCards.ToList();

            #endregion



            #region Home Club Protokol

            ViewBag.MatchPlayerSelectionViewModel = SortHelper.SortProtokolPlayerList(ClubMatchPlayers(matchInfo.HomeClubId, matchId, matchInfo.MatchDate), matchInfo.YoungPlayerAge21Limit);

            #endregion

            #region Guest Club Protokol

            ViewBag.MatchPlayerSelectionViewModelGuest = SortHelper.SortProtokolPlayerList(ClubMatchPlayers(matchInfo.GuestClubId, matchId, matchInfo.MatchDate), matchInfo.YoungPlayerAge21Limit);

            #endregion


            #region Official section home club protokol

            var homeClubOfficialOrders = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted &&
                            x.ClubId == matchInfo.HomeClubId &&
                            x.ClubOfficial.ContractBeginDate <= matchInfo.MatchDate &&
                            ((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > matchInfo.MatchDate) ||
                             (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > matchInfo.MatchDate))
                            && x.SeasonId==matchInfo.SeasonId)
                .OrderBy(x => x.ClubOfficial.Position.Sort);


            var matchHomeClubOfficialSelectionViewModels = homeClubOfficialOrders.Select(x => new MatchOfficialSelectionViewModel()
            {
                ClubOfficialId = x.ClubOfficialId,
                OfficialFirstName = x.ClubOfficial.FirstName,
                OfficialLastName = x.ClubOfficial.LastName,
                OfficialFatherName = x.ClubOfficial.FatherName,
                OfficialPositionName = x.ClubOfficial.Position.Name,
                ClubTypeId = x.ClubTypeId,
                Selected = false
            }).ToList();

            var matchHomeClubOfficials = _db.MatchClubOfficials.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == matchInfo.HomeClubId);

            foreach (var item in matchHomeClubOfficials)
            {
                var selectedOfficial = matchHomeClubOfficialSelectionViewModels.FirstOrDefault(x => x.ClubOfficialId == item.ClubOfficialId);

                if (selectedOfficial != null)
                {
                    selectedOfficial.Selected = true;
                    selectedOfficial.MatchOfficialZoneTypeId = item.MatchOfficialZoneTypeId;
                }
            }

            ViewBag.MatchHomeClubOfficialSelectionViewModels = matchHomeClubOfficialSelectionViewModels.ToList();


            #endregion


            #region Official section guest club protokol

            var guestClubOfficialOrders = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted &&
                            x.ClubId == matchInfo.GuestClubId &&
                            x.ClubOfficial.ContractBeginDate <= matchInfo.MatchDate &&
                            ((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > matchInfo.MatchDate) ||
                             (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > matchInfo.MatchDate))
                            && x.SeasonId == matchInfo.SeasonId)
                .OrderBy(x => x.ClubOfficial.Position.Sort);


            var matchGuestClubOfficialSelectionViewModels = guestClubOfficialOrders.Select(x => new MatchOfficialSelectionViewModel()
            {
                ClubOfficialId = x.ClubOfficialId,
                OfficialFirstName = x.ClubOfficial.FirstName,
                OfficialLastName = x.ClubOfficial.LastName,
                OfficialFatherName = x.ClubOfficial.FatherName,
                OfficialPositionName = x.ClubOfficial.Position.Name,
                ClubTypeId = x.ClubTypeId,
                Selected = false
            }).ToList();

            var matchGuestClubOfficials = _db.MatchClubOfficials.Where(x =>
                !x.IsDeleted &&
                x.MatchId == matchId &&
                x.ClubId == matchInfo.GuestClubId);

            foreach (var item in matchGuestClubOfficials)
            {
                var selectedOfficial = matchGuestClubOfficialSelectionViewModels.FirstOrDefault(x => x.ClubOfficialId == item.ClubOfficialId);

                if (selectedOfficial != null)
                {
                    selectedOfficial.Selected = true;
                    selectedOfficial.MatchOfficialZoneTypeId = item.MatchOfficialZoneTypeId;
                }
            }

            ViewBag.MatchGuestClubOfficialSelectionViewModels = matchGuestClubOfficialSelectionViewModels.ToList();


            #endregion

            ViewBag.MatchId = matchId;

            return View(matchDocumentViewModel);
        }


        //TODO In ClubProtokolPlayers proc also return captain and selected fields
        private List<MatchPlayerSelectionViewModel> ClubMatchPlayers(int? clubId, int matchId, DateTime? matchDate)
        {
            ////var clubPlayerOrders = from cpo in _db.ClubPlayerOrders.Where(x => x.ClubConfirm && x.OperatorConfirm && !x.Rejected && !x.IsDeleted && x.ClubId == matchInfo.HomeClubId)
            ////                       select cpo;
            ////var matchPlayerSelectionViewModel = clubPlayerOrders
            ////    .Include(x => x.Player)
            ////    .Include(x => x.Player.Position)
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
            ////        Selected = false
            ////    }).ToList();



            if (!matchDate.HasValue || !clubId.HasValue)
                return new List<MatchPlayerSelectionViewModel>();

            SqlParameter clubIdParam = new SqlParameter("clubId", clubId);
            SqlParameter matchIdParam = new SqlParameter("matchId", matchId);
            SqlParameter matchDateParam = new SqlParameter("matchDate", matchDate);
            var matchPlayerSelectionViewModel =
                _db.Database.SqlQuery<MatchPlayerSelectionViewModel>("ClubProtokolPlayers @clubId, @matchId, @matchDate", clubIdParam, matchIdParam, matchDateParam).ToList();


            var matchClubPlayers = _db.MatchClubPlayerShifts.Where(x => !x.IsDeleted && x.MatchId == matchId && x.ClubId == clubId);


            foreach (var item in matchClubPlayers)
            {
                var selectedPlayer = matchPlayerSelectionViewModel.FirstOrDefault(x => x.PlayerId == item.PlayerId);

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
                x.ClubId == clubId &&
                x.Captain == 1).Select(x => x.PlayerId).FirstOrDefault();

            var captain = matchPlayerSelectionViewModel.FirstOrDefault(x => x.PlayerId == captainPlayerId);
            if (captain != null)
            {
                captain.Captain = true;
            }

            #endregion

            return matchPlayerSelectionViewModel;
        }


        //[CustomAuthorize(Roles = "admin,referee")]
        [AllowAnonymous]
        public ActionResult MatchDocumentPdf(int matchId)
        {
            //MatchDocument action-ina redirect etdikde CustomAutorize-da null olur user. Sebeb Global.asax-da token cookie-nin redirection-da null olmasidir.
            //Bu sebebden muveqqeti meselenin helline kimi kod kopyalanir.

            return new Rotativa.ActionAsPdf("MatchDocument", new { matchId = matchId, isPdf = true });
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
