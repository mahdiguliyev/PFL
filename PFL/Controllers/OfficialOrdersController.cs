using System;
using System.Data.Entity;
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
    public class OfficialOrdersController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public OfficialOrdersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Turnirdə klub rəsmiləri";
            ViewBag.BaseUrl = "ClubTournamentOfficials";
        }
        // GET: Districts
        public ActionResult Officials(int clubTypeId, int clubId, int? seasonId)
        {
            var currentDate = DateTime.Now;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var clubTournamentOfficials = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => !x.IsDeleted && x.ClubId == clubId && x.ClubTypeId == clubTypeId
                //((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > currentDate.Date) ||
                // (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > currentDate.Date))
                //&& x.ClubOfficial.ContractEndDate > currentDate.Date
                && x.SeasonId == seasonId
                //&& x.ClubOfficial.SeasonId == seasonId
                )
                .OrderBy(x => x.ClubOfficialId)
                .Select(x => new ClubOfficialOrderListDto()
                {
                    Id = x.Id,
                    FirstName = x.ClubOfficial.FirstName,
                    LastName = x.ClubOfficial.LastName,
                    FatherName = x.ClubOfficial.FatherName,
                    PositionName = x.ClubOfficial.Position.Name,
                    ClubConfirm = x.ClubConfirm,
                    Rejected = x.Rejected,
                    OperatorConfirm = x.OperatorConfirm,
                    TerminationDate = x.ClubOfficial.TerminationDate,
                    OperatorConfirmDate = x.OperatorConfirmDate
                });

            ViewBag.SeasonId = seasonId;
            ViewBag.ClubTypeId = clubTypeId;

            return View(clubTournamentOfficials);
        }


        public ActionResult OfficialsPdf(int clubTypeId, int clubId, int? seasonId)
        {
            var currentDate = DateTime.Now;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var clubTournamentOfficials = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => !x.IsDeleted && x.ClubId == clubId && x.ClubTypeId == clubTypeId
                            //((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > currentDate.Date) ||
                            // (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > currentDate.Date))
                            //&& x.ClubOfficial.ContractEndDate > currentDate.Date
                            && x.SeasonId == seasonId)
                .OrderBy(x => x.ClubOfficialId)
                .Select(x => new ClubOfficialOrderListDto()
                {
                    Id = x.Id,
                    FirstName = x.ClubOfficial.FirstName,
                    LastName = x.ClubOfficial.LastName,
                    FatherName = x.ClubOfficial.FatherName,
                    PositionName = x.ClubOfficial.Position.Name,
                    ClubConfirm = x.ClubConfirm,
                    Rejected = x.Rejected,
                    OperatorConfirm = x.OperatorConfirm,
                    TerminationDate = x.ClubOfficial.TerminationDate
                });


            var club = _db.Clubs.FirstOrDefault(x => x.Id == clubId);

            ViewBag.SeasonId = seasonId;

            return new Rotativa.PartialViewAsPdf(clubTournamentOfficials)
            {
                PageSize = Size.A4,
                FileName = club?.Name.Replace(" ", "_") + (clubTypeId == 1 ? "_A_Komandasi_Resmileri" : "_B_Komandasi_Resmileri") + ".pdf"
            };
        }


        public ActionResult Row(int id)
        {
            int year = DateTime.Now.Year;

            var official = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);// && x.Year == year);

            return View(official);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Confirmation(int id, bool confirm)
        {
            var officialOrder = _db.ClubOfficialOrders
                                    .Include(x => x.ClubOfficial)
                                    .Include(x => x.ClubOfficial.Position)
                                    .FirstOrDefault(x => x.Id == id);
            ViewBag.confirm = confirm;

            return View(officialOrder);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult Confirmation(OperatorConfirmationDto operatorConfirmation)
        {

            var clubOfficialOrder = _db.ClubOfficialOrders.FirstOrDefault(x => x.Id == operatorConfirmation.Id);

            if (clubOfficialOrder == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (operatorConfirmation.OperatorConfirm)
            {
                clubOfficialOrder.OperatorConfirm = true;
                clubOfficialOrder.Rejected = false;
                clubOfficialOrder.OperatorConfirmById = _user.UserId;
                clubOfficialOrder.OperatorConfirmDate = DateTime.Now;
                clubOfficialOrder.LastUpdateById = _user.UserId;
                clubOfficialOrder.LastUpdatedDate = DateTime.Now;
            }
            else
            {
                clubOfficialOrder.OperatorConfirm = false;
                clubOfficialOrder.Rejected = true;
                clubOfficialOrder.ClubConfirm = false;
                clubOfficialOrder.LastUpdateById = _user.UserId;
                clubOfficialOrder.LastUpdatedDate = DateTime.Now;

                var officialOrderRejection = new ClubOfficialOrderRejection()
                {
                    ClubOfficialOrderId = operatorConfirmation.Id,
                    RejectionNote = operatorConfirmation.RejectionNote,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.ClubOfficialOrderRejections.Add(officialOrderRejection);
            }

            _db.SaveChanges();


            //return RedirectToAction("PlayerRequests");

            return Json(new { ok = true, Id = clubOfficialOrder.Id });
        }






        //public ActionResult Create(int clubId, int tournamentId, int clubTypeId)
        //{
        //    ViewBag.ClubId = clubId;
        //    ViewBag.TournamentId = tournamentId;
        //    ViewBag.ClubTypeId = clubTypeId;

        //    //ViewBag.ClubOfficials = _db.ClubOfficials.Where(x => x.ClubId == clubId).ToList();

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(ClubOfficialOrderDto clubTournamentOfficialViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!_user.UserClubId.HasValue)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //        int year = DateTime.Now.Year;

        //        var alreadyExist = _db.ClubOfficialOrders.Any(x =>
        //            !x.IsDeleted && x.ClubId == _user.UserClubId &&
        //            x.Year == year &&
        //            x.ClubOfficialId == clubTournamentOfficialViewModel.ClubOfficialId);

        //        if (alreadyExist)
        //            return Json(new { ok = false, error = "Eyni oyunçu iki dəfə daxil edilə bilməz!" },
        //                JsonRequestBehavior.AllowGet);

        //        ClubOfficialOrder clubOfficialOrder =
        //            Mapper.Map<ClubOfficialOrderDto, ClubOfficialOrder>(clubTournamentOfficialViewModel);

        //        clubOfficialOrder.CreatedById = _user.UserId;
        //        clubOfficialOrder.CreationDate = DateTime.Now;
        //        _db.ClubOfficialOrders.Add(clubOfficialOrder);
        //        _db.SaveChanges();

        //        return Json(new { ok = true });
        //    }

        //    return Json(new { ok = false, error = "Model düzgün göndərilməyib." },
        //        JsonRequestBehavior.AllowGet);
        //}


        //// POST: TournamentTypes/Undo/5
        //[HttpDelete, ActionName("Delete")]
        //public ActionResult Delete(int id)
        //{
        //    ClubOfficialOrder clubOfficialOrder = _db.ClubOfficialOrders.Find(id);
        //    if (clubOfficialOrder != null)
        //    {
        //        clubOfficialOrder.LastUpdateById = _user.UserId;
        //        clubOfficialOrder.LastUpdatedDate = DateTime.Now;
        //        clubOfficialOrder.IsDeleted = true;
        //        _db.Entry(clubOfficialOrder).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return Json(new { ok = true });
        //    }

        //    return Json(new { ok = false, error = "ID doğru deyil!" });
        //}


        [HttpGet]
        public ActionResult TerminateOrder(int id)
        {
            var officialOrder = _db.ClubOfficialOrders.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            var terminationDto = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.Club)
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new OfficialTerminationDto()
                {
                    OfficialOrderId = x.Id,
                    FirstName = x.ClubOfficial.FirstName,
                    LastName = x.ClubOfficial.LastName,
                    FatherName = x.ClubOfficial.FatherName,
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
        public ActionResult TerminateOrder(OfficialTerminationDto terminationDto)
        {
            var officialOrder = _db.ClubOfficialOrders.FirstOrDefault(x =>
                x.Id == terminationDto.OfficialOrderId && !x.IsDeleted);

            if (officialOrder == null)
                return Json(new { ok = false, error = "Səhv müraciət! .01" }, JsonRequestBehavior.AllowGet);

            var clubOfficial = _db.ClubOfficials
                .Where(x => x.Id == officialOrder.ClubOfficialId && x.ClubId == officialOrder.ClubId && !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (clubOfficial == null)
                return Json(new { ok = false, error = "Səhv müraciət! .02" }, JsonRequestBehavior.AllowGet);

            clubOfficial.TerminationDate = terminationDto.TerminationDate;
            clubOfficial.LastUpdateById = _user.UserId;
            clubOfficial.LastUpdatedDate = DateTime.Now;

            _db.SaveChanges();

            return Json(new { ok = true, Id = officialOrder.Id, Terminated = clubOfficial.TerminationDate < DateTime.Now, TerminationDateStr = clubOfficial.TerminationDate?.ToString("dd.MM.yyyy") });
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
