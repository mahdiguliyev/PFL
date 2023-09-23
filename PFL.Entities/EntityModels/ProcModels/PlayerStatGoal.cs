using System;

namespace PFL.Entities.EntityModels.ProcModels
{
    public class PlayerStatGoal
    {

        public int TourNumber { get; set; }

        public string HomeClubName { get; set; }

        public int HomeClubScore { get; set; }

        public string GuestClubName { get; set; }

        public int GuestClubScore { get; set; }

        public int GoalMinute { get; set; }

        public int? GoalMinutePlus { get; set; }

        public string GoalTypeName { get; set; }

        public string ClubName { get; set; }

        public DateTime? MatchDate { get; set; }


    }
}