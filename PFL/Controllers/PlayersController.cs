using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Entities.EntityModels.ProcModels;
using PFL.Entities.EntityModels.Views;
using PFL.Membership;
using PFL.Models.DTO;
using PFL.Models.Enums;
using PFL.Models.ViewModels;
using PFL.Utils;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class PlayersController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;


        public PlayersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Futbolçular";
            ViewBag.BaseUrl = "Players";
        }

        // GET: Players
        public ActionResult Index(string playerName, int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            var playersQuery = _db.VPlayers.Where(x => !x.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(playerName))
            {
                string[] searchKeys = playerName.Split(' ');

                if (searchKeys.Length > 5)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                foreach (var item in searchKeys)
                {

                    playersQuery =
                        playersQuery.Where(x => (x.FirstName.ToLower() + x.LastName.ToLower() + x.FatherName.ToLower() + x.PlayerNumber).Contains(item));
                }
            }
            var players = playersQuery.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize);

            ViewBag.PlayerNameSearchText = playerName;

            return View(players);
        }

        //// GET: Players/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Player player = _db.Players.Find(id);
        //    if (player == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(player);
        //}

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name");
            //ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name");
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name");
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name");
            ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlayerCreateViewModel playerCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                //----
                //----Player birinci sefer yarananda transfers cevdeveline setr dusmelidir
                //----Bunun ucun requestde from_club da gelmelidir
                //----

                Player player = Mapper.Map<PlayerCreateViewModel, Player>(playerCreateViewModel);


                if (playerCreateViewModel.PhotoUpload != null && playerCreateViewModel.PhotoUpload.ContentLength > 0)
                {
                    var uploadDir = "/Files/Photos/";
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var photoPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerCreateViewModel.PhotoUpload.FileName);
                    var photoUrl = Path.Combine(uploadDir, playerCreateViewModel.PhotoUpload.FileName);
                    playerCreateViewModel.PhotoUpload.SaveAs(photoPath);
                    player.PhotoUrl = photoUrl.Replace("\\", "/");
                }

                player.CurrentClub = null;
                player.CreatedById = _user.UserId;
                player.CreationDate = DateTime.Now;

                _db.Players.Add(player);
                _db.SaveChanges();

                Transfer transfer = new Transfer()
                {
                    ClubFromId = playerCreateViewModel.FromClubId,
                    ClubFromName = playerCreateViewModel.FromClubName,
                    ClubToId = playerCreateViewModel.CurrentClubId,
                    PlayerId = player.Id,
                    ContractTypeId = playerCreateViewModel.ContractTypeId,
                    DateFrom = playerCreateViewModel.ContractBeginDate,
                    DateTo = playerCreateViewModel.ContractEndDate,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };


                if (!string.IsNullOrEmpty(transfer.ClubFromName))
                {
                    transfer.ClubFromId = null;
                }

                _db.Transfers.Add(transfer);
                _db.SaveChanges();



                return RedirectToAction("Index");
            }

            ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.CurrentClubId);
            //ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.FromClubId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", playerCreateViewModel.ContractTypeId);
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name", playerCreateViewModel.CitizenshipId);
            ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", playerCreateViewModel.PositionTypeId);

            return View(playerCreateViewModel);
        }


        // GET: Players/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PlayerCreateViewModel player = _db.Players
                .Where(x => x.Id == id)
                .Select(x => new PlayerCreateViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName,
                    BirthDate = x.BirthDate,
                    CitizenshipId = x.CitizenshipId,
                    CurrentClubId = x.CurrentClubId,
                    CurrentClubName = x.CurrentClub.Name,
                    IsReversePlayer = x.IsReversePlayer,
                    ContractTypeId = x.ContractTypeId,
                    PlayerNumber = x.PlayerNumber,
                    PositionTypeId = x.PositionTypeId,
                    PhotoPath = x.PhotoUrl
                })
                .FirstOrDefault();


            if (player == null)
            {
                return HttpNotFound();
            }

            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", player.ContractTypeId);
            ViewBag.CitizenshipId = new SelectList(_db.Countries.Where(x => x.Id == player.CitizenshipId), "Id", "Name", player.CitizenshipId);
            ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", player.PositionTypeId);


            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayerCreateViewModel playerCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                Player player = _db.Players.FirstOrDefault(x => x.Id == playerCreateViewModel.Id);

                if (player == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



                //player = Mapper.Map(playerCreateViewModel, player);
                player.FirstName = playerCreateViewModel.FirstName;
                player.LastName = playerCreateViewModel.LastName;
                player.FatherName = playerCreateViewModel.FatherName;
                player.BirthDate = playerCreateViewModel.BirthDate;
                player.CitizenshipId = playerCreateViewModel.CitizenshipId;
                player.IsReversePlayer = playerCreateViewModel.IsReversePlayer;
                player.PlayerNumber = playerCreateViewModel.PlayerNumber;
                player.PositionTypeId = playerCreateViewModel.PositionTypeId;


                if (playerCreateViewModel.PhotoUpload != null && playerCreateViewModel.PhotoUpload.ContentLength > 0)
                {
                    var uploadDir = "/Files/Photos/";
                    var fileGuid = Guid.NewGuid().ToString().Replace("-", "") + "-";
                    var photoPath = Path.Combine(Server.MapPath(uploadDir), fileGuid + playerCreateViewModel.PhotoUpload.FileName);
                    var photoUrl = Path.Combine(uploadDir, playerCreateViewModel.PhotoUpload.FileName);
                    playerCreateViewModel.PhotoUpload.SaveAs(photoPath);
                    player.PhotoUrl = photoUrl.Replace("\\", "/");
                }


                player.LastUpdateById = _user.UserId;
                player.LastUpdatedDate = DateTime.Now;


                _db.Entry(player).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.CurrentClubId);
            ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.FromClubId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", playerCreateViewModel.ContractTypeId);
            ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x => x.Sort).ThenBy(x => x.Name), "Id", "Name", playerCreateViewModel.CitizenshipId);
            ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", playerCreateViewModel.PositionTypeId);

            return View(playerCreateViewModel);
        }



        // POST: TournamentTypes/Delete/5
        [HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = _db.Players.Find(id);
            if (player != null)
            {
                player.LastUpdateById = _user.UserId;
                player.LastUpdatedDate = DateTime.Now;
                player.IsDeleted = true;
                _db.Entry(player).State = EntityState.Modified;
                _db.SaveChanges();

                return Json(new { ok = true, Id = player.Id });
            }

            return Json(new { ok = false, error = "Oyunçu tapılmadı" });
        }

        //[HttpGet, ActionName("Undo")]
        //public ActionResult Undo(int id)
        //{
        //    Player player = _db.Players.Find(id);
        //    if (player != null)
        //    {
        //        player.LastUpdateById = _user.UserId;
        //        player.LastUpdatedDate = DateTime.Now;
        //        player.IsDeleted = false;
        //        _db.Entry(player).State = EntityState.Modified;
        //        _db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}










        #region Player requests

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult PlayerRequests(string playerName, int? clubId, int? page, int? seasonId)
        {
            int pageIndex = page ?? 1;
            int pageSize = Constants.PageSize;

            if (!seasonId.HasValue)
                seasonId = SeasonHelper.GetCurrentSeasonId();

            var clubPlayerRequestQuery = _db.ClubPlayerRequests
                .Where(x => !x.IsDeleted && x.ClubConfirm && x.SeasonId == seasonId)
                .Include(x => x.RequestClub)
                .Include(x => x.FromClub);

            if (clubId.HasValue)
            {
                clubPlayerRequestQuery = clubPlayerRequestQuery.Where(x => x.RequestClubId == clubId);
                ViewBag.ClubId = clubId;
            }

            if (!string.IsNullOrEmpty(playerName))
            {
                string[] searchKeys = playerName.Split(' ');

                if (searchKeys.Length > 5)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                foreach (var item in searchKeys)
                {

                    clubPlayerRequestQuery =
                        clubPlayerRequestQuery.Where(x => (x.PlayerFirstName.ToLower() + x.PlayerLastName.ToLower() + x.PlayerFatherName.ToLower() + x.PlayerNumber).Contains(item));
                }
            }

            var clubPlayerRequest = clubPlayerRequestQuery.OrderByDescending(x => x.ClubConfirm).ToPagedList(pageIndex, pageSize);

            ViewBag.PlayerNameSearchText = playerName;
            ViewBag.SeasonId = seasonId;

            return View(clubPlayerRequest);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult PlayerRequestDetail(int id)
        {
            var clubPlayerRequest = _db.ClubPlayerRequests
                                        .Include(x => x.RequestClub)
                                        .Include(x => x.FromClub)
                                        .Include(x => x.Country)
                                        .Include(x => x.ContractType)
                                        .Include(x => x.Position)
                                        .FirstOrDefault(x => x.Id == id);

            if (clubPlayerRequest == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var requestRejections = _db.ClubPlayerRequestRejections.Where(x => x.ClubPlayerRequestId == id).OrderByDescending(x => x.CreationDate).ToList();

            ViewBag.RequestRejections = requestRejections;



            return View(clubPlayerRequest);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult PlayerRequestConfirmation(PlayerRequestOperatorDto playerRequestOperator)
        {

            var clubPlayerRequest = _db.ClubPlayerRequests.FirstOrDefault(x => x.Id == playerRequestOperator.Id);

            if (clubPlayerRequest == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (playerRequestOperator.OperatorConfirm)
            {
                clubPlayerRequest.OperatorConfirm = true;
                clubPlayerRequest.Rejected = false;
                clubPlayerRequest.OperatorConfirmById = _user.UserId;
                clubPlayerRequest.OperatorConfirmDate = DateTime.Now;
                clubPlayerRequest.LastUpdateById = _user.UserId;
                clubPlayerRequest.LastUpdatedDate = DateTime.Now;
            }
            else
            {
                clubPlayerRequest.OperatorConfirm = false;
                clubPlayerRequest.Rejected = true;
                clubPlayerRequest.ClubConfirm = false;
                clubPlayerRequest.LastUpdateById = _user.UserId;
                clubPlayerRequest.LastUpdatedDate = DateTime.Now;

                var requestRejection = new ClubPlayerRequestRejection()
                {
                    ClubPlayerRequestId = playerRequestOperator.Id,
                    RejectionNote = playerRequestOperator.RejectionNote,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now
                };

                _db.ClubPlayerRequestRejections.Add(requestRejection);
            }

            _db.SaveChanges();


            //return RedirectToAction("PlayerRequestDetail", new { id = playerRequestOperator.Id });
            return RedirectToAction("PlayerRequests", new { clubId = clubPlayerRequest.RequestClubId });
        }

        #endregion




        #region Transfers

        [HttpGet]
        public ActionResult Transfers(int playerId)
        {
            var player = _db.Players.FirstOrDefault(x => x.Id == playerId);

            if (player == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var playerTransfers = _db.Transfers
                .Include(x => x.ClubFrom)
                .Include(x => x.ClubTo)
                .Include(x => x.ContractType)
                .Where(x => x.PlayerId == playerId && !x.IsDeleted)
                .ToList();


            PlayerTransfersViewModel playerTransfersViewModel = new PlayerTransfersViewModel()
            {
                PlayerId = player.Id,
                PlayerFirstName = player.FirstName,
                PlayerLastName = player.LastName,
                PlayerFatherName = player.FatherName,
                Transfers = playerTransfers
            };

            return View(playerTransfersViewModel);
        }



        [HttpGet]
        public ActionResult NewTransfer(int playerId)
        {
            var player = _db.Players.FirstOrDefault(x => x.Id == playerId);

            if (player == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            //var playerLastTransfer = _db.Transfers.Where(x => x.ClubToId == player.CurrentClubId).Max(x => x.DateFrom);
            ViewBag.PlayerId = playerId;
            ViewBag.PlayerFullName = player.FirstName + " " + player.LastName;


            //ViewBag.ClubFromId = new SelectList(_db.Clubs, "Id", "Name", player.CurrentClubId);
            //ViewBag.ClubToId = new SelectList(_db.Clubs, "Id", "Name");


            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name");


            return View();
        }


        [HttpPost]
        public ActionResult NewTransfer(Transfer transferRequest)
        {
            if (ModelState.IsValid)
            {

                var player = _db.Players.FirstOrDefault(x => x.Id == transferRequest.PlayerId);

                if (player == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


                Transfer transfer = new Transfer
                {
                    PlayerId = transferRequest.PlayerId,
                    ClubFromId = transferRequest.ClubFromId,
                    ClubFromName = transferRequest.ClubFromId == null ? transferRequest.ClubFromName : "",
                    ClubToId = transferRequest.ClubToId,
                    ClubToName = transferRequest.ClubToId == null ? transferRequest.ClubToName : "",
                    ContractTypeId = transferRequest.ContractTypeId,
                    DateFrom = transferRequest.DateFrom,
                    DateTo = transferRequest.DateTo,
                    CreatedById = _user.UserId,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                };

                player.CurrentClubId = transferRequest.ClubToId;
                _db.Entry(player).State = EntityState.Modified;

                _db.Transfers.Add(transfer);
                _db.SaveChanges(); ;

                return RedirectToAction("Transfers", new { playerId = transfer.PlayerId });
            }


            ViewBag.ClubFromId = new SelectList(_db.Clubs, "Id", "Name", transferRequest.ClubFromId);
            ViewBag.ClubToId = new SelectList(_db.Clubs, "Id", "Name", transferRequest.ClubToId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", transferRequest.ContractTypeId);


            return View(transferRequest);
        }


        [HttpGet]
        public ActionResult EditTransfer(int id)
        {
            var transfer = _db.Transfers.FirstOrDefault(x => x.Id == id);

            if (transfer == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var player = _db.Players.FirstOrDefault(x => x.Id == transfer.PlayerId);

            ViewBag.PlayerId = transfer.PlayerId;
            ViewBag.PlayerFullName = player?.FirstName + " " + player?.LastName;


            ViewBag.ClubFromId = new SelectList(_db.Clubs, "Id", "Name", transfer.ClubFromId);
            ViewBag.ClubToId = new SelectList(_db.Clubs, "Id", "Name", transfer.ClubToId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", transfer.ContractTypeId);


            return View("NewTransfer", transfer);
        }

        [HttpPost]
        public ActionResult EditTransfer(Transfer transferRequest)
        {
            if (ModelState.IsValid)
            {
                var transfer = _db.Transfers.FirstOrDefault(x => x.Id == transferRequest.Id);
                if (transfer == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                transfer.ClubFromId = transferRequest.ClubFromId;
                transfer.ClubFromName = transferRequest.ClubFromId == null ? transferRequest.ClubFromName : "";
                transfer.ClubToId = transferRequest.ClubToId;
                transfer.ClubToName = transferRequest.ClubToId == null ? transferRequest.ClubToName : "";
                transfer.ContractTypeId = transferRequest.ContractTypeId;
                transfer.DateFrom = transferRequest.DateFrom;
                transfer.DateTo = transferRequest.DateTo;
                transfer.LastUpdateById = _user.UserId;
                transfer.LastUpdatedDate = DateTime.Now;

                _db.Entry(transfer).State = EntityState.Modified;



                if (_db.Transfers.Count(x => x.PlayerId == transfer.PlayerId && x.DateFrom > transfer.DateFrom) == 0)
                {
                    var player = _db.Players.FirstOrDefault(x => x.Id == transferRequest.PlayerId);
                    if (player != null)
                        player.CurrentClubId = transfer.ClubToId;

                    _db.Entry(player).State = EntityState.Modified;
                }


                _db.SaveChanges();

                return RedirectToAction("Transfers", new { playerId = transfer.PlayerId });
            }


            ViewBag.ClubFromId = new SelectList(_db.Clubs, "Id", "Name", transferRequest.ClubFromId);
            ViewBag.ClubToId = new SelectList(_db.Clubs, "Id", "Name", transferRequest.ClubToId);
            ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", transferRequest.ContractTypeId);


            return View(transferRequest);
        }


        //[HttpDelete]
        [HttpGet]
        public ActionResult DeleteTransfer(int id)
        {
            var transfer = _db.Transfers.FirstOrDefault(x => x.Id == id);
            if (transfer == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            transfer.LastUpdateById = _user.UserId;
            transfer.LastUpdatedDate = DateTime.Now;
            transfer.IsDeleted = true;
            _db.Entry(transfer).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Transfers", new { playerId = transfer.PlayerId });
        }



        #endregion





        #region Player Statistics

        [CustomAuthorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Statistics(long playerId)
        {
            var playerInfo = _db.Players
                .Include(x => x.CurrentClub)
                .Include(x => x.Country)
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName,
                    BirthDate = x.BirthDate,
                    Citizenship = x.Country.Name,
                    CurrentClub = x.CurrentClub.Name,
                    PlayerNumber = x.PlayerNumber,
                    Position = x.Position.Name,
                    PhotoUrl = x.PhotoUrl
                }).FirstOrDefault();


            if (playerInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            //var playerStatistics = _db.VPlayerStatistics.ToList();
            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            var playerStatistics =
                _db.Database.SqlQuery<VPlayerStatistic>("PlayerStatistics @playerId", playerIdParam).ToList();


            PlayerStatisticsViewModel playerStatisticsViewModel = new PlayerStatisticsViewModel()
            {
                PlayerInfo = playerInfo,
                PlayerStatistics = playerStatistics
            };



            ViewBag.IsPdf = false;

            return View(playerStatisticsViewModel);
        }


        [CustomAuthorize(Roles = "admin")]
        [HttpGet]
        public ActionResult StatisticsPdf(long playerId)
        {
            var playerInfo = _db.Players
                .Include(x => x.CurrentClub)
                .Include(x => x.Country)
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName,
                    BirthDate = x.BirthDate,
                    Citizenship = x.Country.Name,
                    CurrentClub = x.CurrentClub.Name,
                    PlayerNumber = x.PlayerNumber,
                    Position = x.Position.Name,
                    PhotoUrl = x.PhotoUrl
                }).FirstOrDefault();


            if (playerInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            //var playerStatistics = _db.VPlayerStatistics.ToList();
            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            var playerStatistics =
                _db.Database.SqlQuery<VPlayerStatistic>("PlayerStatistics @playerId", playerIdParam).ToList();


            PlayerStatisticsViewModel playerStatisticsViewModel = new PlayerStatisticsViewModel()
            {
                PlayerInfo = playerInfo,
                PlayerStatistics = playerStatistics
            };



            ViewBag.IsPdf = true;

            return new Rotativa.ViewAsPdf("Statistics", playerStatisticsViewModel)
            {
                PageOrientation=Rotativa.Options.Orientation.Landscape
            };
        }



        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatMatches(long playerId, int tournamentId)
        {
            ViewBag.PlayerId = playerId;
            ViewBag.TournamentId = tournamentId;

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatMatches @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;
            ViewBag.IsPdf = false;


            return View(playerStatMatches);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatMatchesPdf(long playerId, int tournamentId)
        {
            ViewBag.PlayerId = playerId;
            ViewBag.TournamentId = tournamentId;

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatMatches @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;
            ViewBag.IsPdf = true;


            return new Rotativa.ViewAsPdf("StatMatches", playerStatMatches);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatMainStaff(long playerId, int tournamentId)
        {
            ViewBag.PlayerId = playerId;
            ViewBag.TournamentId = tournamentId;


            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatMainStaff @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;
            ViewBag.IsPdf = false;


            return View(playerStatMatches);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatMainStaffPdf(long playerId, int tournamentId)
        {

            ViewBag.PlayerId = playerId;
            ViewBag.TournamentId = tournamentId;


            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatMainStaff @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;
            ViewBag.IsPdf = true;


            return new Rotativa.ViewAsPdf("StatMainStaff", playerStatMatches);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatSecondStaffIn(long playerId, int tournamentId)
        {

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatSecondStaffIn @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;


            return View(playerStatMatches);
        }



        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatOut(long playerId, int tournamentId)
        {

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatOut @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;


            return View(playerStatMatches);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatSecondStaff(long playerId, int tournamentId)
        {

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatMatches =
                _db.Database.SqlQuery<PlayerStatMatch>("PlayerStatSecondStaff @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;


            return View(playerStatMatches);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatGoals(long playerId, int tournamentId)
        {

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);
            var playerStatGoals =
                _db.Database.SqlQuery<PlayerStatGoal>("PlayerStatGoals @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;


            return View(playerStatGoals);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "admin")]
        public ActionResult StatCards(long playerId, int tournamentId, PenaltyCardTypeEnum penaltyCardType)
        {

            var playerInfo = _db.Players
                .Where(x => x.Id == playerId)
                .Select(x => new PlayerInfo()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FatherName = x.FatherName
                }).FirstOrDefault();

            var tournamentInfo = _db.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (playerInfo == null && tournamentInfo == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            SqlParameter playerIdParam = new SqlParameter("playerId", playerId);
            SqlParameter tournamentIdParam = new SqlParameter("tournamentId", tournamentId);

            string procName = "";
            string statName = "";

            switch (penaltyCardType)
            {
                case PenaltyCardTypeEnum.Yellow:
                    procName = "PlayerStatYellowCards";
                    statName = "Sarı kart statistikası";
                    break;
                case PenaltyCardTypeEnum.Yellow2:
                    procName = "PlayerStatYellow2Cards";
                    statName = "2-ci sarı kart statistikası";
                    break;
                case PenaltyCardTypeEnum.Red:
                    procName = "PlayerStatRedCards";
                    statName = "Qırmızı kart statistikası";
                    break;
            }


            var playerStatCards =
                _db.Database.SqlQuery<PlayerStatCard>(procName + " @playerId, @tournamentId", playerIdParam, tournamentIdParam).ToList();


            ViewBag.PlayerInfo = playerInfo;
            ViewBag.TournamentInfo = tournamentInfo;
            ViewBag.StatName = statName;


            return View(playerStatCards);
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
