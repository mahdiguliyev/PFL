using System;

namespace PFL.Models.ViewModels
{
    public class RefereeMatchViewModel
    {
        public string TournamentTypeName { get; set; }

        public int TournamentId { get; set; }
        public string TournamentName { get; set; }

        public int SeasonStartYear { get; set; }
        public int SeasonEndYear { get; set; }

        public int TourNumber { get; set; }

        public int? MatchId { get; set; }

        public int? HomeClubId { get; set; }
        public string HomeClubName { get; set; }

        public int? GuestClubId { get; set; }
        public string GuestClubName { get; set; }

        public string Stadium { get; set; }

        public DateTime? MatchDate { get; set; }



    }
}