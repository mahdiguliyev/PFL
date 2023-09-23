using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class MatchPenaltyResult : BaseModel
    {
        public int Id { get; set; }

        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("player_id")]
        public long? PlayerId { get; set; }

        public int? Goal { get; set; }

        [Column("goal_type_id")]
        public int? GoalTypeId { get; set; }

        [Column("penalty_order")]
        public int PenaltyOrder { get; set; }


        public virtual Club Club { get; set; }

        public virtual Match Match { get; set; }

        public virtual GoalType GoalType { get; set; }

        public virtual Player Player { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}