using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PFL.Entities.EntityModels;
using PFL.Entities.EntityModels.ProcModels;
using PFL.Membership;
using PFL.Models;
using PFL.Utils;

namespace PFL.Controllers
{
    public class Select2Controller : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public Select2Controller()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
        }




        public JsonResult Tournaments(string searchText)
        {
            var tournaments = _db.Tournaments
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = "(" + x.SeasonStartYear + " - " + x.SeasonEndYear + ") " + x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                tournaments = tournaments.Where(x => x.text.Contains(searchText));
            }


            Select2Model select2PlayerModel = new Select2Model()
            {
                results = tournaments.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }




        public JsonResult TournamentClubs(string searchText, int tournamentId)
        {
            var clubs = _db.TournamentClubs
                .Include(x => x.Club)
                .Where(x => !x.IsDeleted && x.TournamentId == tournamentId)
                .OrderByDescending(x => x.Id)
                .Select(x => new Select2Data()
                {
                    id = x.ClubId,
                    text = x.Club.Name
                });


            if (!string.IsNullOrEmpty(searchText))
            {
                clubs = clubs.Where(x => x.text.Contains(searchText));
            }


            Select2Model select2PlayerModel = new Select2Model()
            {
                results = clubs.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Clubs(string searchText)
        {
            var clubs = _db.Clubs
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                clubs = clubs.Where(x => x.text.Contains(searchText));
            }

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = clubs.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }




        public JsonResult ClubPlayers(int clubId, string searchText)
        {
            //var players = (from t in _db.Transfers
            //               join p in _db.Players on t.PlayerId equals p.Id
            //               where t.ClubToId == clubId && t.DateTo > DateTime.Now && t.DateFrom < DateTime.Now && !p.IsDeleted && !t.IsDeleted && t.CreationDate = 
            //               select new Select2PlayerData()
            //               {
            //                   id = p.Id,
            //                   text = p.FirstName + " " + p.LastName + " " + p.FatherName
            //               });


            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    players = players.Where(x => x.text.Contains(searchText));
            //}

            searchText = string.IsNullOrEmpty(searchText) ? "" : searchText.ToLower();


            SqlParameter clubIdParam = new SqlParameter("clubId", _user.UserClubId);
            var players =
                _db.Database.SqlQuery<ClubPlayer>("ClubPlayersList @clubId", clubIdParam)
                    .Select(x => new Select2PlayerData()
                    {
                        id = x.PlayerId,
                        text = x.FirstName + " " + x.LastName + " " + x.FatherName
                    });



            Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
            {
                results = players.ToList().Where(x=>x.text.ToLower().Contains(searchText)).ToList(),
                pagination = new Pagination() { more = false }
            };



            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Players(string searchText)
        {
            var players = _db.Players
                .Include(x => x.Position)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Position.Id).Select(x => new Select2PlayerData()
                {
                    id = x.Id,
                    text = x.PlayerNumber + " - " + x.FirstName + " " + x.LastName + " " + x.FatherName
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                players = players.Where(x => x.text.Contains(searchText));
            }

            Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
            {
                results = players.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult OfficialPositions(string searchText)
        {
            var officialPositions = _db.OfficialPositions
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                officialPositions = officialPositions.Where(x => x.text.Contains(searchText));
            }

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = officialPositions.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClubOfficials(int clubId, string searchText)
        {
            //int seasonId = SeasonHelper.GetCurrentSeasonId();
            var currentDate = DateTime.Now;

            var clubOfficials = _db.ClubOfficials
                .Include(x => x.Position)
                .Where(x => !x.IsDeleted && x.ClubId == clubId && x.ClubConfirm && x.OperatorConfirm && !x.Rejected &&
                            ((x.TerminationDate != null && x.TerminationDate > currentDate.Date) ||
                             (x.TerminationDate == null && x.ContractEndDate > currentDate.Date)))// && x.SeasonId == seasonId)
                .OrderBy(x => x.PositionId)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = x.Position.Name + " - " + x.FirstName + " " + x.LastName + " " + x.FatherName
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                clubOfficials = clubOfficials.Where(x => x.text.Contains(searchText));
            }

            Select2Model select2PlayerModel = new Select2Model()
            {
                results = clubOfficials.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Countries(string searchText)
        {
            var items = _db.Countries
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Sort).ThenBy(x => x.Name)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.text.Contains(searchText));
            }


            Select2Model select2PlayerModel = new Select2Model()
            {
                results = items.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Stadiums(string searchText)
        {
            var items = _db.Stadiums
                .Where(x => !x.IsDeleted && !x.IsClosed)
                .OrderByDescending(x => x.Name)
                .Select(x => new Select2Data()
                {
                    id = x.Id,
                    text = x.Name
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.text.Contains(searchText));
            }


            Select2Model select2PlayerModel = new Select2Model()
            {
                results = items.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Referees(int? refereeTypeId, string searchText)
        {
            var refereesQuery = _db.Referees.Where(x => !x.IsDeleted && !x.IsClosed);

            if (refereeTypeId.HasValue)
            {
                refereesQuery = refereesQuery.Where(x => x.RefereeTypeId == refereeTypeId);
            }
            else
            {
                refereesQuery = refereesQuery.Where(x => x.RefereeTypeId == 1);
            }


            var items = refereesQuery
                        .OrderBy(x => x.FirstName)
                        .ThenBy(x => x.LastName)
                        .Select(x => new Select2Data()
                        {
                            id = x.Id,
                            text = x.LastName + " " + x.FirstName + " " + x.FatherName
                        });

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.text.Contains(searchText));
            }


            Select2Model select2RefereeModel = new Select2Model()
            {
                results = items.ToList(),
                pagination = new Pagination() { more = false }
            };


            return Json(select2RefereeModel, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult ActiveTournaments(string searchText)
        //{
        //    var query = _db.Tournaments.Where(x => !x.IsDeleted && x.Completed != null && !x.Completed.Value);

        //    var items = query
        //        .OrderByDescending(x => x.TournamentTypeId)
        //        .ThenByDescending(x=>x.SeasonStartYear)
        //        .Select(x => new Select2Data()
        //        {
        //            id = x.Id,
        //            text = "(" + x.SeasonStartYear + " - " + x.SeasonEndYear + ") " + x.Name
        //        });

        //    if (!string.IsNullOrEmpty(searchText))
        //    {
        //        items = items.Where(x => x.text.Contains(searchText));
        //    }


        //    Select2Model select2RefereeModel = new Select2Model()
        //    {
        //        results = items.ToList(),
        //        pagination = new Pagination() { more = false }
        //    };


        //    return Json(select2RefereeModel, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult Matches(string searchText)
        {
            var query = _db.TournamentTours
                .Include(x => x.Tournament)
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub)
                .Include(x => x.Match.GuestClub)
                .Where(x => !x.IsDeleted && x.MatchId != null
                            && !x.Tournament.IsDeleted && x.Tournament.Completed != null &&
                            !x.Tournament.Completed.Value
                            && x.Match.HomeClubId != null && x.Match.GuestClubId != null && x.Match.MatchDate != null
                            && x.Match.RefereeId != null && (x.Tournament.Completed != null && !x.Tournament.Completed.Value));


            var items = query
                .OrderByDescending(x => x.Match.MatchDate)
                .Select(x => new
                {
                    Id = x.MatchId,
                    MatchDate = x.Match.MatchDate,
                    text = x.Match.HomeClub.Name + " - " + x.Match.GuestClub.Name + " (" + x.Tournament.Name + " " + x.Tournament.SeasonStartYear + "/" + x.Tournament.SeasonEndYear + " - " + x.TourNumber + " tur) "
                });

            if (!string.IsNullOrEmpty(searchText))
            {
                if (DateTime.TryParse(searchText, out DateTime matchDate))
                {
                    items = items.Where(x => x.MatchDate.Value.Date == matchDate);
                }
                else
                {
                    items = items.Where(x => x.text.Contains(searchText));
                }
            }


            List<Select2Data> itemList = items.ToList().Select(x => new Select2Data()
            {
                id = x.Id.Value,
                text = x.MatchDate?.ToString("dd.MM.yyyy HH:mm") + " - " + x.text
            }).ToList();




            Select2Model select2RefereeModel = new Select2Model()
            {
                results = itemList,
                pagination = new Pagination() { more = false }
            };


            return Json(select2RefereeModel, JsonRequestBehavior.AllowGet);
        }



        public JsonResult MatchPlayers(int matchId, string searchText)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);

            var playersQuery = _db.MatchClubPlayers.Where(x => !x.IsDeleted && x.MatchId == matchId);


            var players = playersQuery
                .OrderBy(x => x.Player.FirstName)
                .ThenBy(x => x.Player.LastName)
                .Select(x => new
                {
                    x.ClubId,
                    Select2Player = new Select2PlayerData
                    {
                        id = x.Id,
                        text = x.Player.FirstName + " " + x.Player.LastName + " " +
                           x.Player.FatherName //+ " (" + x.Player.BirthDate?.ToString("dd.MM.yyyy") + ")"
                    }

                });

            if (!string.IsNullOrEmpty(searchText))
            {
                players = players.Where(x => x.Select2Player.text.Contains(searchText));
            }

            var select2Data = players.Select(x => x.Select2Player).ToList();

            Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
            {
                results = select2Data,
                pagination = new Pagination() { more = false }
            };

            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MatchOfficials(int matchId, string searchText)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);
            if (match == null)
                return Json(new { ok = false, error = "Səhv müraciət!" }, JsonRequestBehavior.AllowGet);

            var officialsQuery = _db.MatchClubOfficials.Where(x => !x.IsDeleted && x.MatchId == matchId);


            var officials = officialsQuery
                .OrderBy(x => x.ClubOfficial.FirstName)
                .ThenBy(x => x.ClubOfficial.LastName)
                .Select(x => new
                {
                    x.ClubId,
                    Select2Player = new Select2PlayerData
                    {
                        id = x.Id,
                        text = x.ClubOfficial.FirstName + " " + x.ClubOfficial.LastName + " " +
                               x.ClubOfficial.FatherName //+ " (" + x.Player.BirthDate?.ToString("dd.MM.yyyy") + ")"
                    }

                });

            if (!string.IsNullOrEmpty(searchText))
            {
                officials = officials.Where(x => x.Select2Player.text.Contains(searchText));
            }

            var select2Data = officials.Select(x => x.Select2Player).ToList();

            Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
            {
                results = select2Data,
                pagination = new Pagination() { more = false }
            };

            return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        }
    }
}