using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class OfficialTournamentPenalty : BaseModel
    {
        public int Id { get; set; }

        [Column("match_club_official_id")]
        public int MatchClubOfficialId { get; set; }

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


        public virtual MatchClubOfficial MatchClubOfficial { get; set; }

        public virtual User UserCreatedBy { get; set; }

        public virtual User UserLastUpdateBy { get; set; }
    }
}