using System;

namespace PFL.Models.DTO
{
    public class PlayerTerminationDto : TerminationDto
    {
        public long? PlayerOrderId { get; set; }

    }

    public class OfficialTerminationDto : TerminationDto
    {
        public long? OfficialOrderId { get; set; }

    }

    public class TerminationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string ClubName { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}