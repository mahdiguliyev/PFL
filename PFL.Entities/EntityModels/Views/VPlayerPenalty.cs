using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_player_penalties")]
    public class VPlayerPenalty
    {

        public int Id { get; set; }

        [Column("season_id")]
        public int SeasonId { get; set; }

        [Column("tournament_name")]
        public string TournamentName { get; set; }

        [Column("tour_number")]
        public int TourNumber { get; set; }

        [Column("club_name")]
        public string ClubName { get; set; }

        [Column("player_first_name")]
        public string PlayerFirstName { get; set; }

        [Column("player_last_name")]
        public string PlayerLastName { get; set; }

        [Column("card_type_id")]
        public int? CardTypeId { get; set; }

        [Column("card_type_name")]
        public string CardTypeName { get; set; }

        [Column("penalty_reason")]
        public string PenaltyReason { get; set; }

        [Column("pass_match_count")]
        public int PassMatchCount { get; set; }

        public bool Payed { get; set; }

        [Column("admin_review")]
        public bool AdminReview { get; set; }

        [Column("is_manual")]
        public bool IsManual { get; set; }

        [Column("penalty_price_amount")]
        public int PenaltyPriceAmount { get; set; }
    }
}