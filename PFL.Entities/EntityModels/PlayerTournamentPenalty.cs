using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class PlayerTournamentPenalty : BaseModel
    {
        public int Id { get; set; }

        [Column("match_club_player_id")]
        public int MatchClubPlayerId { get; set; }

        [Column("penalty_card_type_id")]
        public int? PenaltyCardTypeId { get; set; }

        [Column("penalty_reason")]
        public string PenaltyReason { get; set; }

        [Column("pass_match_count")]
        public int PassMatchCount { get; set; }

        [Column("penalty_price_amount")]
        public int PenaltyPriceAmount { get; set; }

        [Column("payed")]
        public bool Payed { get; set; }

        [Column("admin_review")]
        public bool AdminReview { get; set; }

        [Column("is_manual")]
        public bool IsManual { get; set; }



        public virtual MatchClubPlayer MatchClubPlayer { get; set; }

        public virtual PenaltyCardType PenaltyCardType { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}