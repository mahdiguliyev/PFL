using System;

namespace PFL.Entities.EntityModels.ProcModels
{
    public class PlayerStatMatch
    {

        public int TourNumber { get; set; }

        public string HomeClubName { get; set; }

        public int HomeClubScore { get; set; }

        public string GuestClubName { get; set; }

        public int GuestClubScore { get; set; }

        public int? PlayerInTime { get; set; }

        public int? PlayerOutTime { get; set; }

        public string ClubName { get; set; }

        public DateTime? MatchDate { get; set; }


    }
}