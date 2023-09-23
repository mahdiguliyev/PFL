using System;

namespace PFL.Models.ViewModels
{
    public class CustomSerializeModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] RoleName { get; set; }

        public int? UserClubId { get; set; }
    }
}