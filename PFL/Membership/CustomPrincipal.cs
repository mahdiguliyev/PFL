using System;
using System.Security.Principal;

namespace PFL.Membership
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties  

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public int? UserClubId { get; set; }
        #endregion

        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(',');

            foreach (var item1 in Roles)
            {
                foreach (var item2 in roles)
                {
                    if (item1.Equals(item2))
                    {
                        return true;
                    }
                }
            }

            return false;
            //if (Roles.Any(r => role.Contains(r)))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}