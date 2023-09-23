using System.Web.Mvc;
using PFL.Membership;

namespace PFL.Controllers.ClubAdmin
{
    [CustomAuthorize(Roles = "club-admin")]
    [ClubAdminAuthorize]
    public class PlayerController : Controller
    {
        //private readonly PFLContext _db;
        //private readonly CustomPrincipal _user;


        //public PlayerController()
        //{
        //    this._db = new PFLContext();
        //    var user = System.Web.HttpContext.Current.User;
        //    _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
        //    ViewBag.Title = "Futbolçular";
        //    ViewBag.BaseUrl = "Player";
        //}

        //// GET: Players
        //public ActionResult Index(int? page)
        //{
        //    int pageIndex = page ?? 1;
        //    int pageSize = Constants.PageSize;


        //    var players = _db.Players.Include(p => p.CurrentClub)
        //        .Include(p => p.ContractType)
        //        .Include(p => p.Country).Include(p => p.Position).Where(x => x.CurrentClubId == _user.UserClubId.Value)
        //        .OrderBy(x => x.PositionTypeId).ToPagedList(pageIndex, pageSize);

        //    return View(players);
        //}

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


        //[CustomAuthorize(Roles = "club-admin")]
        //public JsonResult AllowedPlayerList()
        //{
        //    var players = _db.Players.Include(x=>x.Position).Where(x => x.CurrentClubId == _user.UserClubId).OrderBy(x=>x.Position.Id).Select(x => new { x.Id, x.FirstName, x.LastName, x.FatherName, Position = x.Position.Name, x.BirthDate, CountryName = x.Country.Name });

        //    List<Select2PlayerData> select2Data = new List<Select2PlayerData>();

        //    foreach (var item in players)
        //    {
        //        select2Data.Add(new Select2PlayerData()
        //        {
        //            id = item.Id,
        //            text = item.Position + " - " + item.FirstName + " " + item.LastName + " " + item.FatherName + " ("+item.BirthDate?.ToString("dd.MM.yyyy")+")" + " - "+item.CountryName
        //        });
        //    }

        //    Select2PlayerModel select2PlayerModel = new Select2PlayerModel()
        //    {
        //        results = select2Data,
        //        pagination = new Pagination() { more = false }
        //    };


        //    return Json(select2PlayerModel, JsonRequestBehavior.AllowGet);
        //}

        //// GET: Players/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name");
        //    ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name");
        //    ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name");
        //    ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x=>x.Sort).ThenBy(x=>x.Name), "Id", "Name");
        //    ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name");
        //    return View();
        //}

        //// POST: Players/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PlayerCreateViewModel playerCreateViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        Player player = Mapper.Map<PlayerCreateViewModel, Player>(playerCreateViewModel);


        //        if (playerCreateViewModel.PhotoUpload != null && playerCreateViewModel.PhotoUpload.ContentLength > 0)
        //        {
        //            var uploadDir = "/Files/Photos/";
        //            var photoPath = Path.Combine(Server.MapPath(uploadDir), playerCreateViewModel.PhotoUpload.FileName);
        //            var photoUrl = Path.Combine(uploadDir, playerCreateViewModel.PhotoUpload.FileName);
        //            playerCreateViewModel.PhotoUpload.SaveAs(photoPath);
        //            player.PhotoUrl = photoUrl.Replace("\\", "/");
        //        }


        //        player.CreatedById = _user.UserId;
        //        player.CreationDate = DateTime.Now;

        //        _db.Players.Add(player);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.CurrentClubId);
        //    ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.FromClubId);
        //    ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", playerCreateViewModel.ContractTypeId);
        //    ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x=>x.Sort).ThenBy(x=>x.Name), "Id", "Name", playerCreateViewModel.CitizenshipId);
        //    ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", playerCreateViewModel.PositionTypeId);

        //    return View(playerCreateViewModel);
        //}





        //// GET: Players/Edit/5
        //public ActionResult Edit(long? id)
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
        //    ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name", player.CurrentClubId);
        //    //ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", player.FromClubId);
        //    ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", player.ContractTypeId);
        //    ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x=>x.Sort).ThenBy(x=>x.Name), "Id", "Name", player.CitizenshipId);
        //    ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", player.PositionTypeId);

        //    PlayerCreateViewModel createViewModel = Mapper.Map<Player, PlayerCreateViewModel>(player);

        //    return View("Create", createViewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(PlayerCreateViewModel playerCreateViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Player player = _db.Players.FirstOrDefault(x => x.Id == playerCreateViewModel.Id);

        //        if (player == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



        //        player = Mapper.Map(playerCreateViewModel, player);


        //        if (playerCreateViewModel.PhotoUpload != null && playerCreateViewModel.PhotoUpload.ContentLength > 0)
        //        {
        //            var uploadDir = "/Files/Photos/";
        //            var photoPath = Path.Combine(Server.MapPath(uploadDir), playerCreateViewModel.PhotoUpload.FileName);
        //            var photoUrl = Path.Combine(uploadDir, playerCreateViewModel.PhotoUpload.FileName);
        //            playerCreateViewModel.PhotoUpload.SaveAs(photoPath);
        //            player.PhotoUrl = photoUrl.Replace("\\", "/");
        //        }


        //        player.LastUpdateById = _user.UserId;
        //        player.LastUpdatedDate = DateTime.Now;


        //        _db.Entry(player).State = EntityState.Modified;
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CurrentClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.CurrentClubId);
        //    ViewBag.FromClubId = new SelectList(_db.Clubs, "Id", "Name", playerCreateViewModel.FromClubId);
        //    ViewBag.ContractTypeId = new SelectList(_db.ContractTypes, "Id", "Name", playerCreateViewModel.ContractTypeId);
        //    ViewBag.CitizenshipId = new SelectList(_db.Countries.OrderBy(x=>x.Sort).ThenBy(x=>x.Name), "Id", "Name", playerCreateViewModel.CitizenshipId);
        //    ViewBag.PositionTypeId = new SelectList(_db.Positions, "Id", "Name", playerCreateViewModel.PositionTypeId);

        //    return View("Create", playerCreateViewModel);
        //}



        //// POST: TournamentTypes/Delete/5
        //[HttpGet, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Player player = _db.Players.Find(id);
        //    if (player != null)
        //    {
        //        player.LastUpdateById = _user.UserId;
        //        player.LastUpdatedDate = DateTime.Now;
        //        player.IsDeleted = true;
        //        _db.Entry(player).State = EntityState.Modified;
        //        _db.SaveChanges();
        //    }


        //    return RedirectToAction("Index");
        //}

        //// POST: TournamentTypes/Undo/5
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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
