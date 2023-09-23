using System;
using System.Collections.Generic;
using System.Web.Security;
using PFL.Entities.EntityModels;


namespace PFL.Membership
{
    public class CustomMembershipUser : MembershipUser
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Role> Roles { get; set; }

        public int? UserClubId { get; set; }


        public CustomMembershipUser(User user) : base("CustomMembership", user.UserName, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
        }
    }
}