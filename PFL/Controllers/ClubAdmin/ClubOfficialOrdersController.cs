using System;
using System.Data.Entity;
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
    public class ClubOfficialOrdersController : Controller
    {

        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public ClubOfficialOrdersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Turnirdə klub rəsmiləri";
            ViewBag.BaseUrl = "ClubTournamentOfficials";
        }
        // GET: Districts
        public ActionResult Officials(int clubTypeId, int? seasonId)
        {
            var currentDate = DateTime.Now;
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            //int year = currentDate.Year;

            //if (currentDate.Month < 7)
            //    year = year - 1;

            var clubOfficialOrders = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .Where(x => !x.IsDeleted && x.ClubId == _user.UserClubId && x.ClubTypeId == clubTypeId &&
                            ((x.ClubOfficial.TerminationDate != null && x.ClubOfficial.TerminationDate > currentDate.Date) ||
                             (x.ClubOfficial.TerminationDate == null && x.ClubOfficial.ContractEndDate > currentDate.Date)) && x.SeasonId == seasonId)
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



            return View(clubOfficialOrders);
        }

        public ActionResult Row(int officialOrderId)
        {

            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //int year = DateTime.Now.Year;
            var seasonId = SeasonHelper.GetCurrentSeasonId();

            var player = _db.ClubOfficialOrders
                .Include(x => x.ClubOfficial)
                .Include(x => x.ClubOfficial.Position)
                .FirstOrDefault(x => x.Id == officialOrderId && x.ClubId == _user.UserClubId && x.SeasonId == seasonId && !x.IsDeleted);


            return View(player);
        }


        public ActionResult Create(int clubTypeId)
        {
            if (!_user.UserClubId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ClubId = _user.UserClubId;
            ViewBag.ClubTypeId = clubTypeId;

            //ViewBag.ClubOfficials = _db.ClubOfficials.Where(x => x.ClubId == clubId).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClubOfficialOrderDto clubOfficialOrderDto)
        {
            if (ModelState.IsValid)
            {
                if (!_user.UserClubId.HasValue)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                int year = DateTime.Now.Year;
                var seasonId = SeasonHelper.GetCurrentSeasonId();

                var alreadyExist = _db.ClubOfficialOrders.Any(x =>
                    !x.IsDeleted && x.ClubId == _user.UserClubId &&
                    //x.Year == year &&
                    x.SeasonId == seasonId &&
                    x.ClubOfficialId == clubOfficialOrderDto.ClubOfficialId);

                if (alreadyExist)
                    return Json(new { ok = false, error = "Eyni rəsmi şəxs iki dəfə daxil edilə bilməz!" },
                        JsonRequestBehavior.AllowGet);

                ClubOfficialOrder clubOfficialOrder =
                    Mapper.Map<ClubOfficialOrderDto, ClubOfficialOrder>(clubOfficialOrderDto);

                clubOfficialOrder.ClubId = _user.UserClubId.Value;
                clubOfficialOrder.Year = year;
                clubOfficialOrder.SeasonId = SeasonHelper.GetCurrentSeasonId();
                clubOfficialOrder.ClubConfirm = true;
                clubOfficialOrder.ClubConfirmById = _user.UserId;
                clubOfficialOrder.ClubConfirmDate = DateTime.Now;

                clubOfficialOrder.CreatedById = _user.UserId;
                clubOfficialOrder.CreationDate = DateTime.Now;
                _db.ClubOfficialOrders.Add(clubOfficialOrder);
                _db.SaveChanges();

                return Json(new { ok = true, Id = clubOfficialOrder.Id });
            }

            return Json(new { ok = false, error = "Model düzgün göndərilməyib." },
                JsonRequestBehavior.AllowGet);
        }


        // POST: TournamentTypes/Undo/5
        [HttpDelete, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            ClubOfficialOrder clubOfficialOrder = _db.ClubOfficialOrders.Find(id);
            if (clubOfficialOrder != null)
            {
                clubOfficialOrder.LastUpdateById = _user.UserId;
                clubOfficialOrder.LastUpdatedDate = DateTime.Now;
                clubOfficialOrder.IsDeleted = true;


                var clubOfficialOrderRejections =
                    _db.ClubOfficialOrderRejections.Where(x => x.ClubOfficialOrderId == id);

                foreach (var item in clubOfficialOrderRejections)
                {
                    item.LastUpdateById = _user.UserId;
                    item.LastUpdatedDate = DateTime.Now;
                    item.IsDeleted = true;
                }


                //_db.Entry(clubOfficialOrder).State = EntityState.Deleted;
                _db.SaveChanges();
                return Json(new { ok = true, Id = clubOfficialOrder.Id });
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
