using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels
{
    public class MatchResultLog : BaseModelLog
    {
        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("player_id")]
        public long? PlayerId { get; set; }

        public int? Goal { get; set; }

        [Column("goal_type_id")]
        public int? GoalTypeId { get; set; }

        public int? Minute { get; set; }

        [Column("minute_plus")]
        public int? MinutePlus { get; set; }
    }
}