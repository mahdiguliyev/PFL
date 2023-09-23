using System;

namespace PFL.Entities.EntityModels.ProcModels
{
    public class PlayerStatCard
    {

        public int TourNumber { get; set; }

        public string HomeClubName { get; set; }

        public int HomeClubScore { get; set; }

        public string GuestClubName { get; set; }

        public int GuestClubScore { get; set; }

        public int CardMinute { get; set; }

        public int? CardMinutePlus { get; set; }

        public string ClubName { get; set; }

        public DateTime? MatchDate { get; set; }


    }
}