using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_official_penalties")]
    public class VOfficialPenalty
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public string TournamentName { get; set; }
        public int TourNumber { get; set; }
        public string ClubName { get; set; }
        //public int MatchClubOfficialId { get; set; }
        public string OfficialFirstName { get; set; }
        public string OfficialLastName { get; set; }
        public string PenaltyReason { get; set; }
        public int PassMatchCount { get; set; }
        public int PenaltyPriceAmount { get; set; }
        public bool Payed { get; set; }
        public bool AdminReview { get; set; }
    }
}