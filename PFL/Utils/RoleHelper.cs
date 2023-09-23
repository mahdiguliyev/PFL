using System;
using System.Data.Entity;
using System.Linq;
using PFL.Entities.EntityModels;


namespace PFL.Utils
{
    public class RoleHelper
    {
        public static int? UserClubId(Guid userId)
        {
            using (var db = new PFLContext())
            {

                var user = db.Users.Include(x => x.Roles).FirstOrDefault(x => x.Id == userId);

                if (user != null)

                    foreach (var userRole in user.Roles)
                    {
                        if (userRole.Name == "club-admin")
                        {
                            var userClub = db.ClubUsers
                                .FirstOrDefault(x => x.UserId == user.Id);

                            return userClub?.ClubId;
                        }
                    }

                return null;
            }
        }
    }
}