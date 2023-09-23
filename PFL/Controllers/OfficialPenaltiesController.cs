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
    public class OfficialPenaltiesController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public OfficialPenaltiesController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Rəsmi şəxslərin cərimələri";
            ViewBag.BaseUrl = "OfficialPenalties";
        }
        // GET: Districts
        public ActionResult Index(int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var officialPenalties = _db.VOfficialPenalties.Where(x => x.SeasonId == seasonId).OrderBy(x => x.AdminReview).ThenBy(x => x.Payed).ToPagedList(pageIndex, pageSize);

            ViewBag.SeasonId = seasonId;

            return View(officialPenalties);
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
        public ActionResult CustomCreate([Bind(Include = "MatchClubOfficialId,PenaltyReason,PassMatchCount,PenaltyPriceAmount,AdminReview")] OfficialTournamentPenalty officialTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                officialTournamentPenalty.CreatedById = _user.UserId;
                officialTournamentPenalty.CreationDate = DateTime.Now;

                _db.OfficialTournamentPenalties.Add(officialTournamentPenalty);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(officialTournamentPenalty);
        }


        // GET: Districts/Edit/5
        public ActionResult CustomEdit(int id)
        {
            OfficialTournamentPenalty officialTournamentPenalty = _db.OfficialTournamentPenalties
                                                                    .Include(x => x.MatchClubOfficial)
                                                                    .Include(x => x.MatchClubOfficial.Match)
                                                                    .Include(x => x.MatchClubOfficial.Match.HomeClub)
                                                                    .Include(x => x.MatchClubOfficial.Match.GuestClub)
                                                                    .Include(x => x.MatchClubOfficial.ClubOfficial)
                                                                    .FirstOrDefault(x => x.Id == id);
            if (officialTournamentPenalty == null)
            {
                return HttpNotFound();
            }

            if (officialTournamentPenalty.AdminReview)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View("CustomCreate", officialTournamentPenalty);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            OfficialTournamentPenalty officialTournamentPenalty = _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == id);
            if (officialTournamentPenalty == null)
            {
                return HttpNotFound();
            }

            if (officialTournamentPenalty.AdminReview)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(officialTournamentPenalty);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomEdit([Bind(Include = "Id,MatchClubOfficialId,PenaltyReason,PassMatchCount,PenaltyPriceAmount,AdminReview")] OfficialTournamentPenalty officialTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                OfficialTournamentPenalty _officialTournamentPenalty = _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == officialTournamentPenalty.Id);
                if (_officialTournamentPenalty == null)
                {
                    return HttpNotFound();
                }

                if (_officialTournamentPenalty.AdminReview)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                _officialTournamentPenalty.MatchClubOfficialId = officialTournamentPenalty.MatchClubOfficialId;
                _officialTournamentPenalty.PenaltyReason = officialTournamentPenalty.PenaltyReason;
                _officialTournamentPenalty.PassMatchCount = officialTournamentPenalty.PassMatchCount;
                _officialTournamentPenalty.PenaltyPriceAmount = officialTournamentPenalty.PenaltyPriceAmount;
                _officialTournamentPenalty.AdminReview = officialTournamentPenalty.AdminReview;
                _officialTournamentPenalty.LastUpdateById = _user.UserId;
                _officialTournamentPenalty.LastUpdatedDate = DateTime.Now;
                _db.Entry(_officialTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("CustomCreate", officialTournamentPenalty);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PassMatchCount,PenaltyPriceAmount,AdminReview")] OfficialTournamentPenalty officialTournamentPenalty)
        {
            if (ModelState.IsValid)
            {
                OfficialTournamentPenalty _officialTournamentPenalty = _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == officialTournamentPenalty.Id);
                if (_officialTournamentPenalty == null)
                {
                    return HttpNotFound();
                }

                if (!_officialTournamentPenalty.AdminReview)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                _officialTournamentPenalty.PassMatchCount = officialTournamentPenalty.PassMatchCount;
                _officialTournamentPenalty.PenaltyPriceAmount = officialTournamentPenalty.PenaltyPriceAmount;
                _officialTournamentPenalty.AdminReview = officialTournamentPenalty.AdminReview;
                _officialTournamentPenalty.LastUpdateById = _user.UserId;
                _officialTournamentPenalty.LastUpdatedDate = DateTime.Now;
                _db.Entry(_officialTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(officialTournamentPenalty);
        }

        [HttpGet]
        public ActionResult SetAsPayed(int id)
        {
            OfficialTournamentPenalty officialTournamentPenalty = _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == id);
            if (officialTournamentPenalty == null && officialTournamentPenalty.AdminReview)
            {
                return HttpNotFound();
            }

            officialTournamentPenalty.LastUpdateById = _user.UserId;
            officialTournamentPenalty.LastUpdatedDate = DateTime.Now;
            officialTournamentPenalty.Payed = true;
            _db.SaveChanges();


            return RedirectToAction("Index");
        }




        [HttpGet]
        public ActionResult Delete(int id)
        {
            var officialTournamentPenalty = _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == id);

            if (officialTournamentPenalty != null)
            {
                officialTournamentPenalty.LastUpdateById = _user.UserId;
                officialTournamentPenalty.LastUpdatedDate = DateTime.Now;
                officialTournamentPenalty.IsDeleted = true;
                _db.Entry(officialTournamentPenalty).State = EntityState.Modified;
                _db.SaveChanges();
            }


            return RedirectToAction("Index");
        }



        #region Edit after confirm

        [HttpGet]
        public ActionResult EditAfterConfirm(int id)
        {
            PenaltyEditAfterConfirmDto dto = _db.OfficialTournamentPenalties
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

            OfficialTournamentPenalty officialTournamentPenalty =
                _db.OfficialTournamentPenalties.FirstOrDefault(x => x.Id == request.Id && !x.IsDeleted);

            if (officialTournamentPenalty == null)
                return new HttpNotFoundResult();


            officialTournamentPenalty.PassMatchCount = request.PassMatchCount;
            officialTournamentPenalty.PenaltyPriceAmount = request.PenaltyPriceAmount;
            officialTournamentPenalty.LastUpdateById = _user.UserId;
            officialTournamentPenalty.LastUpdatedDate = DateTime.Now;

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
