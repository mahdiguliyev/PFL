using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFL.Entities.EntityModels.Views
{
    [Table("v_protocol_player_cards")]
    public class VProtocolPlayerCard
    {
        [Key]
        public int MatchClubPlayerId { get; set; }
        public int MatchId { get; set; }
        public string ClubName { get; set; }
        public long PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? YellowMinute { get; set; }
        public int? YellowMinutePlus { get; set; }
        public string YellowOffGameTypeName { get; set; }
        public string YellowReason { get; set; }
        public int? Yellow2Minute { get; set; }
        public int? Yellow2MinutePlus { get; set; }
        public string Yellow2OffGameTypeName { get; set; }
        public string Yellow2Reason { get; set; }
        public int? RedMinute { get; set; }
        public int? RedMinutePlus { get; set; }
        public string RedOffGameTypeName { get; set; }
        public string RedReason { get; set; }
    }
}