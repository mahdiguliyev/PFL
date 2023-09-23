namespace PFL.Models.ViewModels
{
    public class RefereePlayerShiftViewModel
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }

        public long? PlayerInId { get; set; }
        public int? PlayerInNumber { get; set; }
        public string PlayerInFirstName { get; set; }
        public string PlayerInLastName { get; set; }
        public string PlayerInFatherName { get; set; }

        public long? PlayerOutId { get; set; }
        public int? PlayerOutNumber { get; set; }
        public string PlayerOutFirstName { get; set; }
        public string PlayerOutLastName { get; set; }
        public string PlayerOutFatherName { get; set; }

        public int? MinuteIn { get; set; }
        public int? MinuteInPlus { get; set; }
    }

    public class RefereeAddPlayerShift
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }

        public long? PlayerInId { get; set; }
        public int? PlayerInNumber { get; set; }
        public string PlayerInFirstName { get; set; }
        public string PlayerInLastName { get; set; }
        public string PlayerInFatherName { get; set; }

        public long? PlayerOutId { get; set; }
        public int? PlayerOutNumber { get; set; }
        public string PlayerOutFirstName { get; set; }
        public string PlayerOutLastName { get; set; }
        public string PlayerOutFatherName { get; set; }

        public int? MinuteIn { get; set; }
        public int? MinuteInPlus { get; set; }
    }
}