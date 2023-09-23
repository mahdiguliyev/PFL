using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class MatchesController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public MatchesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Oyunlar";
            ViewBag.BaseUrl = "Matches";
        }


        public ActionResult List(int tournamentId, int tourNumber)
        {
            //var matches = _db.TournamentTours.Include(x => x.Match).Include(x => x.Match.HomeClub)
            //    .Include(x => x.Match.GuestClub)
            //    .Where(x => x.TournamentId == tournamentId && x.TourNumber == tourNumber)
            //    .Select(x => x.Match).ToList();


            //var matches = (from m in _db.Matches.Include(x => x.HomeClub).Include(x => x.GuestClub)
            //               join tt in _db.TournamentTours on m.Id equals tt.MatchId
            //               where tt.TournamentId == tournamentId && tt.TourNumber == tourNumber
            //               select new { matches = m, m.HomeClub, m.GuestClub }).ToList().Select(x => x.matches);

            List<int?> plusGoalTypes = new List<int?>() { 1, 2, 5 };

            var matches = _db.TournamentTours
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub)
                .Include(x => x.Match.GuestClub)
                .Where(x => x.TournamentId == tournamentId && x.TourNumber == tourNumber && x.MatchId != null) //  && x.Match.IsDeleted
                .Select(x => new AdminMatchListDto()
                {
                    Id = x.Match.Id,
                    HomeClubId = x.Match.HomeClubId,
                    HomeClubName = x.Match.HomeClub.Name,
                    GuestClubId = x.Match.GuestClubId,
                    GuestClubName = x.Match.GuestClub.Name,
                    HomeClubScore = x.Match.MatchResults.Count(z => !z.IsDeleted && ((z.ClubId == x.Match.HomeClubId && plusGoalTypes.Contains(z.GoalTypeId)) || (z.ClubId == x.Match.GuestClubId && z.GoalTypeId == 3))),
                    GuestClubScore = x.Match.MatchResults.Count(z => !z.IsDeleted && ((z.ClubId == x.Match.GuestClubId && plusGoalTypes.Contains(z.GoalTypeId)) || (z.ClubId == x.Match.HomeClubId && z.GoalTypeId == 3))),
                    MatchDate = x.Match.MatchDate,
                    HomeClubConfirm = x.Match.HomeClubConfirm,
                    HomeClubConfirmedDate = x.Match.HomeClubConfirmedDate,
                    HomeClubExpConfirmAllow = x.Match.HomeClubExpConfirmAllow,
                    GuestClubConfirm = x.Match.GuestClubConfirm,
                    GuestClubConfirmedDate = x.Match.GuestClubConfirmedDate,
                    GuestClubExpConfirmAllow = x.Match.GuestClubExpConfirmAllow,
                    RefereeConfirm = x.Match.RefereeConfirm,
                    RefereeConfirmedDate = x.Match.RefereeConfirmedDate
                })
                .ToList();



            ViewBag.TourNumber = tourNumber;
            ViewBag.TournamentId = tournamentId;





            return View(matches);
        }

        public ActionResult Row(int matchId)
        {
            //var matches = (from m in _db.Matches.Include(x => x.HomeClub).Include(x => x.GuestClub)
            //               join tt in _db.TournamentTours on m.Id equals tt.MatchId
            //               where m.Id == matchId
            //               select new { matches = m, m.HomeClub, m.GuestClub })
            //    .FirstOrDefault();

            //return View(matches?.matches);

            List<int?> plusGoalTypes = new List<int?>() { 1, 2, 5 };

            var match = _db.TournamentTours
                .Include(x => x.Match)
                .Include(x => x.Match.HomeClub)
                .Include(x => x.Match.GuestClub)
                .Where(x => x.MatchId == matchId)
                .Select(x => new AdminMatchListDto()
                {
                    Id = x.Match.Id,
                    HomeClubId = x.Match.HomeClubId,
                    HomeClubName = x.Match.HomeClub.Name,
                    GuestClubId = x.Match.GuestClubId,
                    GuestClubName = x.Match.GuestClub.Name,
                    HomeClubScore = x.Match.MatchResults.Count(z =>
                        !z.IsDeleted &&
                        ((z.ClubId == x.Match.HomeClubId && plusGoalTypes.Contains(z.GoalTypeId)) ||
                        (z.ClubId == x.Match.GuestClubId && z.GoalTypeId == 3))),
                    GuestClubScore = x.Match.MatchResults.Count(z =>
                        !z.IsDeleted &&
                        ((z.ClubId == x.Match.GuestClubId && plusGoalTypes.Contains(z.GoalTypeId)) ||
                        (z.ClubId == x.Match.HomeClubId && z.GoalTypeId == 3))),
                    MatchDate = x.Match.MatchDate,
                    HomeClubConfirm = x.Match.HomeClubConfirm,
                    HomeClubConfirmedDate = x.Match.HomeClubConfirmedDate,
                    HomeClubExpConfirmAllow = x.Match.HomeClubExpConfirmAllow,
                    GuestClubConfirm = x.Match.GuestClubConfirm,
                    GuestClubConfirmedDate = x.Match.GuestClubConfirmedDate,
                    GuestClubExpConfirmAllow = x.Match.GuestClubExpConfirmAllow,
                    RefereeConfirm = x.Match.RefereeConfirm,
                    RefereeConfirmedDate = x.Match.RefereeConfirmedDate
                })
                .FirstOrDefault();

            return View(match);
        }



        public ActionResult Create(int tournamentId, int tourNumber, int? matchId)
        {
            var tournamentTour = _db.TournamentTours
                .FirstOrDefault(x => x.TournamentId == tournamentId && x.TourNumber == tourNumber);

            if (tournamentTour is null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            if (matchId.HasValue)
            {
                var match = (from m in _db.Matches
                             join tt in _db.TournamentTours on m.Id equals tt.MatchId
                             join t in _db.Tournaments on tt.TournamentId equals t.Id
                             where t.Id == tournamentId && tt.TourNumber == tourNumber && m.Id == matchId
                             select m).FirstOrDefault();

                if (match == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                var clubs = _db.Clubs.Where(x => x.Id == match.HomeClubId || x.Id == match.GuestClubId)
                    .Select(x => new { x.Id, x.Name });

                ViewBag.HomeClubId = new SelectList(clubs, "Id", "Name", match.HomeClubId);
                ViewBag.GuestClubId = new SelectList(clubs, "Id", "Name", match.GuestClubId);




                //referees
                var referees = _db.Referees.Where(x => x.Id == match.RefereeId ||
                                                       x.Id == match.RefereeAssistant1Id ||
                                                       x.Id == match.RefereeAssistant2Id ||
                                                       x.Id == match.FourthRefereeId ||
                                                       x.Id == match.AdditionalReferee1Id ||
                                                       x.Id == match.AdditionalReferee2Id ||
                                                       x.Id == match.RefereeVarId ||
                                                       x.Id == match.RefereeAvarId ||
                                                       x.Id == match.AffaRepresentativeId ||
                                                       x.Id == match.RefereeInspectorId)
                    .Select(x => new { Id = x.Id, Name = x.LastName + " " + x.FirstName }).ToList();

                ViewBag.RefereeId = new SelectList(referees, "Id", "Name", match.RefereeId);
                ViewBag.RefereeAssistant1Id = new SelectList(referees, "Id", "Name", match.RefereeAssistant1Id);
                ViewBag.RefereeAssistant2Id = new SelectList(referees, "Id", "Name", match.RefereeAssistant2Id);
                ViewBag.FourthRefereeId = new SelectList(referees, "Id", "Name", match.FourthRefereeId);
                ViewBag.AdditionalReferee1Id = new SelectList(referees, "Id", "Name", match.AdditionalReferee1Id);
                ViewBag.AdditionalReferee2Id = new SelectList(referees, "Id", "Name", match.AdditionalReferee2Id);
                ViewBag.RefereeVarId = new SelectList(referees, "Id", "Name", match.RefereeVarId);
                ViewBag.RefereeAvarId = new SelectList(referees, "Id", "Name", match.RefereeAvarId);
                ViewBag.AffaRepresentativeId = new SelectList(referees, "Id", "Name", match.AffaRepresentativeId);
                ViewBag.RefereeInspectorId = new SelectList(referees, "Id", "Name", match.RefereeInspectorId);

                ViewBag.StadiumId = new SelectList(_db.Stadiums, "Id", "Name", match.StadiumId);

                ViewBag.TournamentTourId = tournamentTour.Id;

                var matchCreateDto = Mapper.Map<Match, MatchCreateDto>(match);

                matchCreateDto.MatchTime = matchCreateDto.MatchDate;

                return View(matchCreateDto);
            }
            else
            {
                var selectedMatches = (from m in _db.Matches
                                       join tt in _db.TournamentTours on m.Id equals tt.MatchId
                                       where tt.TournamentId == tournamentId && tt.TourNumber == tourNumber
                                       select m).ToList();

                var selectedClubIds = new List<int>();

                foreach (var item in selectedMatches)
                {
                    if (item.HomeClubId.HasValue)
                        selectedClubIds.Add(item.HomeClubId.Value);

                    if (item.GuestClubId.HasValue)
                        selectedClubIds.Add(item.GuestClubId.Value);
                }

                var tournamentClubs = _db.TournamentClubs
                    .Where(x => x.TournamentId == tournamentId && !selectedClubIds.Contains(x.ClubId))
                    .Select(x => new { x.Club.Id, x.Club.Name });

                ViewBag.HomeClubId = new SelectList(tournamentClubs, "Id", "Name");
                ViewBag.GuestClubId = new SelectList(tournamentClubs, "Id", "Name");

                //ViewBag.TournamentId = new SelectList(_db.Tournaments, "Id", "Name");
                //ViewBag.RefereeId = new SelectList(_db.Referees.Where(x=>x.Id == matc), "Id", "FirstName");

                ViewBag.TournamentTourId = tournamentTour.Id;

                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchCreateDto matchCreateDto)
        {
            if (ModelState.IsValid)
            {

                var tournamentTour = _db.TournamentTours.FirstOrDefault(x => x.Id == matchCreateDto.TournamentTourId);

                if (tournamentTour == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


                Match match;


                if (matchCreateDto.Id == 0)
                {
                    var freeTournamentTour = _db.TournamentTours.FirstOrDefault(x =>
                        x.TournamentId == tournamentTour.TournamentId && x.TourNumber == tournamentTour.TourNumber && x.MatchId == null);

                    if (freeTournamentTour == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    if (!matchCreateDto.HomeClubId.HasValue || !matchCreateDto.GuestClubId.HasValue)
                        return Json(new { ok = false, error = "Klublar seçilməyib." });


                    if (matchCreateDto.HomeClubId == matchCreateDto.GuestClubId)
                        return Json(new { ok = false, error = "Ev və Qonaq klublar eyni seçilə bilməz." });


                    match = Mapper.Map<MatchCreateDto, Match>(matchCreateDto);


                    if (matchCreateDto.MatchTime != null && matchCreateDto.MatchDate != null)
                        match.MatchDate = matchCreateDto.MatchDate.Value.Date +
                                          matchCreateDto.MatchTime.Value.TimeOfDay;

                    match.CreatedById = _user.UserId;
                    match.CreationDate = DateTime.Now;
                    match.HomeClubConfirm = false;
                    match.GuestClubConfirm = false;




                    if (match.RefereeId == 0)
                        match.RefereeId = null;

                    if (match.RefereeAssistant1Id == 0)
                        match.RefereeAssistant1Id = null;

                    if (match.RefereeAssistant2Id == 0)
                        match.RefereeAssistant2Id = null;

                    if (match.FourthRefereeId == 0)
                        match.FourthRefereeId = null;

                    if (match.AdditionalReferee1Id == 0)
                        match.AdditionalReferee1Id = null;

                    if (match.AdditionalReferee2Id == 0)
                        match.AdditionalReferee2Id = null;

                    if (match.RefereeVarId == 0 || match.RefereeVarId == null)
                        match.RefereeVarId = null;

                    if (match.RefereeAvarId == 0 || match.RefereeAvarId == null)
                        match.RefereeAvarId = null;

                    if (match.AffaRepresentativeId == 0)
                        match.AffaRepresentativeId = null;

                    if (match.RefereeInspectorId == 0)
                        match.RefereeInspectorId = null;

                    if (match.StadiumId == 0)
                        match.StadiumId = null;

                    try
                    {
                        _db.Matches.Add(match);
                        _db.SaveChanges();

                        freeTournamentTour.MatchId = match.Id;
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return Json(new { ok = false, error = "Sistem xətası. 0x001" });
                    }


                    //return Json(new { ok = true, Id = freeTournamentTour.MatchId });
                }
                else
                {
                    match = (from m in _db.Matches
                             join tt in _db.TournamentTours on m.Id equals tt.MatchId
                             where //tt.Id == matchCreateDto.TournamentTourId && 
                                   m.Id == matchCreateDto.Id
                             select m).FirstOrDefault();

                    if (match == null)
                        return Json(new { ok = false, error = "Səhv müraciət" });

                    if(match.RefereeConfirm)
                    return Json(new { ok = false, error = "Təsdiqlənmiş oyunun nəticəsinə dəyişiklik etməyə icazə verilmir. 0x022" });


                    if (!matchCreateDto.HomeClubId.HasValue || !matchCreateDto.GuestClubId.HasValue)
                        return Json(new { ok = false, error = "Klublar seçilməyib." });


                    if (matchCreateDto.HomeClubId == matchCreateDto.GuestClubId)
                        return Json(new { ok = false, error = "Ev və Qonaq klublar eyni seçilə bilməz." });

                    match.HomeClubId = matchCreateDto.HomeClubId.Value;
                    match.GuestClubId = matchCreateDto.GuestClubId.Value;
                    //match.StadiumId = matchCreateDto.StadiumId;
                    match.HomeClubTechnicalDefeat = matchCreateDto.HomeClubTechnicalDefeat;
                    match.HomeClubTechnicalDefeatNote = matchCreateDto.HomeClubTechnicalDefeatNote;
                    match.GuestClubTechnicalDefeat = matchCreateDto.GuestClubTechnicalDefeat;
                    match.GuestClubTechnicalDefeatNote = matchCreateDto.GuestClubTechnicalDefeatNote;

                    if (matchCreateDto.MatchTime != null && matchCreateDto.MatchDate != null)
                        match.MatchDate = matchCreateDto.MatchDate.Value.Date +
                                          matchCreateDto.MatchTime.Value.TimeOfDay;
                    //match.MatchDate = matchCreateDto.MatchDate;


                    if (matchCreateDto.RefereeId > 0)
                        match.RefereeId = matchCreateDto.RefereeId;

                    if (matchCreateDto.RefereeAssistant1Id > 0)
                        match.RefereeAssistant1Id = matchCreateDto.RefereeAssistant1Id;

                    if (matchCreateDto.RefereeAssistant2Id > 0)
                        match.RefereeAssistant2Id = matchCreateDto.RefereeAssistant2Id;

                    if (matchCreateDto.FourthRefereeId > 0)
                        match.FourthRefereeId = matchCreateDto.FourthRefereeId;

                    if (matchCreateDto.AdditionalReferee1Id > 0)
                        match.AdditionalReferee1Id = matchCreateDto.AdditionalReferee1Id;

                    if (matchCreateDto.AdditionalReferee2Id > 0)
                        match.AdditionalReferee2Id = matchCreateDto.AdditionalReferee2Id;

                    if (matchCreateDto.RefereeVarId > 0 || matchCreateDto.RefereeVarId == null)
                        match.RefereeVarId = matchCreateDto.RefereeVarId;

                    if (matchCreateDto.RefereeAvarId > 0 || matchCreateDto.RefereeAvarId == null)
                        match.RefereeAvarId = matchCreateDto.RefereeAvarId;

                    if (matchCreateDto.AffaRepresentativeId > 0)
                        match.AffaRepresentativeId = matchCreateDto.AffaRepresentativeId;

                    if (matchCreateDto.RefereeInspectorId > 0)
                        match.RefereeInspectorId = matchCreateDto.RefereeInspectorId;

                    if (matchCreateDto.StadiumId > 0)
                        match.StadiumId = matchCreateDto.StadiumId;


                    match.LastUpdateById = _user.UserId;
                    match.LastUpdatedDate = DateTime.Now;

                    try
                    {
                        _db.SaveChanges();
                        //return Json(new { ok = true, Id = match.Id });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { ok = false, error = "Sistem xətası. 0x002" });
                    }

                }






                if (matchCreateDto.HomeClubTechnicalDefeat || matchCreateDto.GuestClubTechnicalDefeat)
                {
                    ////////removing old result
                    var mrs = _db.MatchResults.Where(x => x.MatchId == match.Id);
                    foreach (var mr in mrs)
                    {
                        _db.Entry(mr).State = EntityState.Deleted;
                    }
                    _db.SaveChanges();



                    MatchResult mr1 = new MatchResult()
                    {
                        MatchId = match.Id,
                        ClubId = matchCreateDto.HomeClubTechnicalDefeat ? match.HomeClubId : match.GuestClubId,
                        PlayerId = null,
                        Goal = 1,
                        GoalTypeId = 5,
                        Minute = 0,
                        MinutePlus = 0,
                        CreatedById = _user.UserId,
                        CreationDate = DateTime.Now
                    };

                    MatchResult mr2 = new MatchResult()
                    {
                        MatchId = match.Id,
                        ClubId = matchCreateDto.HomeClubTechnicalDefeat ? match.HomeClubId : match.GuestClubId,
                        PlayerId = null,
                        Goal = 1,
                        GoalTypeId = 5,
                        Minute = 0,
                        MinutePlus = 0,
                        CreatedById = _user.UserId,
                        CreationDate = DateTime.Now
                    };

                    MatchResult mr3 = new MatchResult()
                    {
                        MatchId = match.Id,
                        ClubId = matchCreateDto.HomeClubTechnicalDefeat ? match.HomeClubId : match.GuestClubId,
                        PlayerId = null,
                        Goal = 1,
                        GoalTypeId = 5,
                        Minute = 0,
                        MinutePlus = 0,
                        CreatedById = _user.UserId,
                        CreationDate = DateTime.Now
                    };

                    _db.MatchResults.Add(mr1);
                    _db.MatchResults.Add(mr2);
                    _db.MatchResults.Add(mr3);
                    _db.SaveChanges();
                }
                else
                {
                    //////var mrs = _db.MatchResults.Where(x => x.MatchId == match.Id);
                    //////foreach (var mr in mrs)
                    //////{
                    //////    _db.Entry(mr).State = EntityState.Deleted;
                    //////}

                    //////_db.SaveChanges();
                }

                return Json(new { ok = true, Id = match.Id });


            }

            ViewBag.LeftClubId = new SelectList(_db.Clubs, "Id", "Name", matchCreateDto.HomeClubId);
            ViewBag.RightClubId = new SelectList(_db.Clubs, "Id", "Name", matchCreateDto.GuestClubId);

            ViewBag.TournamentTourId = matchCreateDto.TournamentTourId;
            ViewBag.TournamentId = new SelectList(_db.Tournaments, "Id", "Name", matchCreateDto.TournamentTourId);
            return View(matchCreateDto);
        }


        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Match match = _db.Matches.Find(id);
        //    if (match == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.LeftClubId = new SelectList(_db.Clubs, "Id", "Name", match.HomeClubId);
        //    ViewBag.RightClubId = new SelectList(_db.Clubs, "Id", "Name", match.GuestClubId);
        //    //ViewBag.TournamentId = new SelectList(_db.Tournaments, "Id", "Name", match.Tournament.Id);
        //    return View("Create", match);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,TournamentId,HomeClubId,GuestClubId,HomeClubScore,GuestClubScore,TourNumber,MatchDate")] Match match)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        match.LastUpdateById = _user.UserId;
        //        match.LastUpdatedDate = DateTime.Now;

        //        _db.Entry(match).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return new HttpStatusCodeResult(HttpStatusCode.OK);
        //    }
        //    ViewBag.LeftClubId = new SelectList(_db.Clubs, "Id", "Name", match.HomeClubId);
        //    ViewBag.RightClubId = new SelectList(_db.Clubs, "Id", "Name", match.GuestClubId);
        //    //ViewBag.TournamentId = new SelectList(_db.Tournaments, "Id", "Name", match.Tournament.Id);
        //    return View("Create", match);
        //}



        [HttpGet]
        public ActionResult AccessClubOrders(int matchId, int clubId)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (match.HomeClubId == clubId)
            {
                match.HomeClubConfirm = false;
                match.HomeClubExpConfirmAllow = true;
                _db.SaveChanges();
            }
            else if (match.GuestClubId == clubId)
            {
                match.GuestClubConfirm = false;
                match.GuestClubExpConfirmAllow = true;
                _db.SaveChanges();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccessRefereeEdit(int matchId)
        {
            var match = _db.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            match.RefereeConfirm = false;
            _db.SaveChanges();


            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
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