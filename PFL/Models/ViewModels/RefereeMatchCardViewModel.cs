namespace PFL.Models.ViewModels
{
    public class RefereeMatchCardViewModel
    {
        public int MatchPlayerId { get; set; }
        public int MatchId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public long PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public int? Yellow1Minute { get; set; }
        public int? Yellow1MinutePlus { get; set; }
        public string Yellow1Reason { get; set; }
        public int? Yellow2Minute { get; set; }
        public int? Yellow2MinutePlus { get; set; }
        public string Yellow2Reason { get; set; }
        public int? RedMinute { get; set; }
        public int? RedMinutePlus { get; set; }
        public string RedReason { get; set; }
    }

    public class RefereeAddMatchCard
    {
        public int Id { get; set; }
        public string CardType { get; set; }
        public int MatchId { get; set; }
        public long PlayerId { get; set; }
        public string PlayerFirstName{ get; set; }
        public string PlayerLastName{ get; set; }
        public string PlayerFatherName{ get; set; }
        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
        public string OffGameTypeName { get; set; }
        public int? OffGameTypeId { get; set; }
        public int CardReasonId { get; set; }
        public string CardReasonName { get; set; }
    }
}