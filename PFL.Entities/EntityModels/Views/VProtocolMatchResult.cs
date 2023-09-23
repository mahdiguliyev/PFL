using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_protocol_match_results")]
    public class VProtocolMatchResult
    {
        [Key]
        public int MatchResultId { get; set; }
        public int MatchId { get; set; }

        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public long? PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
        public int? GoalTypeId { get; set; }
        public string GoalType { get; set; }
    }
}