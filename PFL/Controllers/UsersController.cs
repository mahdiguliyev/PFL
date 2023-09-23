using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using BLL;
using PagedList;
using PFL.Entities.EntityModels;
using PFL.Membership;
using PFL.Models.DTO;

namespace PFL.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly PFLContext _db;
        private readonly CustomPrincipal _user;

        public UsersController()
        {
            this._db = new PFLContext();
            var user = System.Web.HttpContext.Current.User;
            _user = user.Identity.IsAuthenticated ? (CustomPrincipal)user : null;
            ViewBag.Title = "Sistem istifadəçiləri";
            ViewBag.BaseUrl = "Users";
        }

        public ActionResult Index()
        {
            var usersCount = _db.VUsersCounts.ToList();

            ViewBag.AdminUserCount = usersCount.FirstOrDefault(x=>x.RoleName == "admin")?.UserCount ?? 0;
            ViewBag.ClubUserCount = usersCount.FirstOrDefault(x=>x.RoleName == "club-admin")?.UserCount ?? 0;
            ViewBag.RefereeUserCount = usersCount.FirstOrDefault(x=>x.RoleName == "referee")?.UserCount ?? 0;

            return View();
        }

        // GET: Users
        public ActionResult List(string userType,int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 30; //Constants.PageSize;




            var usersQuery = _db.Users.Include(x => x.Roles).Where(x=>!x.IsDeleted);

            if (userType == "admin")
            {
                usersQuery = usersQuery.Where(x => x.Roles.Select(z => z.Name).Contains("admin"));
            }
            else if (userType == "club")
            {
                usersQuery = usersQuery.Where(x => x.Roles.Select(z => z.Name).Contains("club-admin"));
            }
            else if (userType == "referee")
            {
                usersQuery = usersQuery.Where(x => x.Roles.Select(z => z.Name).Contains("referee"));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var users = usersQuery.OrderBy(x => x.Id).ToPagedList(pageIndex, pageSize);

            ViewBag.UserType = userType;

            return View(users);
        }

        // GET: Users/Create
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(UserAdminDto userAdminDto)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<UserAdminDto, User>(userAdminDto);
                Role adminRole = _db.Roles.FirstOrDefault(x => x.Id == 2);

                user.Id = Guid.NewGuid();
                user.Roles.Add(adminRole);

                user.Password = SHAHelper.SHA512Generator(userAdminDto.Password);

                user.CreatedById = _user.UserId;
                user.CreationDate = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userAdminDto);
        }



        public ActionResult EditAdmin(Guid id)
        {
            User user = _db.Users.Include(x=>x.Roles).FirstOrDefault(x => x.Id == id);

            if (user == null)
                return HttpNotFound("İstifadəçi tapılmadı");

            //if (!user.Roles.Select(x=>x.Name).Contains("admin"))
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            UserAdminDto userAdminDto = Mapper.Map<User, UserAdminDto>(user);


            return View("CreateAdmin", userAdminDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(UserAdminDto userAdminDto)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.FirstOrDefault(x => x.Id == userAdminDto.Id);
                if (user == null)
                    return HttpNotFound("İstifadəçi tapılmadı");

                user.UserName = userAdminDto.UserName;
                user.Password = SHAHelper.SHA512Generator(userAdminDto.Password);
                user.FirstName = userAdminDto.FirstName;
                user.LastName = userAdminDto.LastName;
                user.FatherName = userAdminDto.FatherName;
                user.Email = userAdminDto.Email;
                user.IsActive = userAdminDto.IsActive;
                
                
                user.LastUpdateById = _user.UserId;
                user.LastUpdatedDate = DateTime.Now;
                //_db.Entry(User).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("CreateAdmin", userAdminDto);
        }




        // GET: Users/Create
        public ActionResult CreateClubAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClubAdmin(UserClubAdminDto userClubAdminDto)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<UserClubAdminDto, User>(userClubAdminDto);
                Role adminRole = _db.Roles.FirstOrDefault(x => x.Id == 4);

                user.Id = Guid.NewGuid();
                user.Roles.Add(adminRole);

                user.Password = SHAHelper.SHA512Generator(userClubAdminDto.Password);

                user.CreatedById = _user.UserId;
                user.CreationDate = DateTime.Now;
                _db.Users.Add(user);
                

                ClubUser clubUser = new ClubUser()
                {
                    ClubId = userClubAdminDto.ClubId,
                    UserId = user.Id,
                    CreationDate = DateTime.Now,
                    CreatedById = _user.UserId
                };

                _db.ClubUsers.Add(clubUser);

                _db.SaveChanges();


                return RedirectToAction("Index");
            }

            return View(userClubAdminDto);
        }



        //// GET: Users/Create
        //public ActionResult EditClubAdmin(Guid id)
        //{
        //    User user = _db.Users.Include(x => x.Roles).FirstOrDefault(x => x.Id == id);

        //    if (user == null)
        //        return HttpNotFound("İstifadəçi tapılmadı");

        //    if (!user.Roles.Select(x => x.Name).Contains("admin"))
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


        //    UserClubAdminDto userAdminDto = Mapper.Map<User, UserClubAdminDto>(user);


        //    return View("CreateClubAdmin", userAdminDto);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditClubAdmin(UserClubAdminDto userClubAdminDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = Mapper.Map<UserClubAdminDto, User>(userClubAdminDto);
        //        Role adminRole = _db.Roles.FirstOrDefault(x => x.Id == 4);

        //        user.Id = Guid.NewGuid();
        //        user.Roles.Add(adminRole);

        //        user.Password = SHAHelper.SHA512Generator(userClubAdminDto.Password);

        //        user.CreatedById = _user.UserId;
        //        user.CreationDate = DateTime.Now;
        //        _db.Users.Add(user);


        //        ClubUser clubUser = new ClubUser()
        //        {
        //            ClubId = userClubAdminDto.ClubId,
        //            UserId = user.Id,
        //            CreationDate = DateTime.Now,
        //            CreatedById = _user.UserId
        //        };

        //        _db.ClubUsers.Add(clubUser);

        //        _db.SaveChanges();


        //        return RedirectToAction("Index");
        //    }

        //    return View(userClubAdminDto);
        //}



        public ActionResult CreateReferee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReferee(UserRefereeDto userRefereeDto)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<UserRefereeDto, User>(userRefereeDto);
                Role adminRole = _db.Roles.FirstOrDefault(x => x.Id == 3);

                user.Id = Guid.NewGuid();
                user.Roles.Add(adminRole);

                user.Password = SHAHelper.SHA512Generator(userRefereeDto.Password);

                user.CreatedById = _user.UserId;
                user.CreationDate = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userRefereeDto);
        }





        [HttpGet, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = _db.Users.Find(id);
            if (user != null)
            {
                user.LastUpdateById = _user.UserId;
                user.LastUpdatedDate = DateTime.Now;
                user.IsActive = false;
                user.IsDeleted = true;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet, ActionName("Undo")]
        public ActionResult Undo(Guid id)
        {
            User user = _db.Users.Find(id);
            if (user != null)
            {
                user.LastUpdateById = _user.UserId;
                user.LastUpdatedDate = DateTime.Now;
                user.IsActive = true;
                user.IsDeleted = false;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
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
