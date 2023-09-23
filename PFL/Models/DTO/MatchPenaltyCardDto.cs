using PFL.Models.Enums;

namespace PFL.Models.DTO
{
    public class MatchPenaltyCardDto
    {
        public int MatchClubPlayerId { get; set; }
        public int MatchId { get; set; }
        public PenaltyCardTypeEnum PenaltyCardType { get; set; }
        public string ClubName { get; set; }
        public long PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
        public string OffGameTypeName { get; set; }
        public string Reason { get; set; }
    }
}