namespace PFL.Models.ViewModels
{
    public class RefereeMatchGoalsViewModel
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public long? PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public int? Goal { get; set; }
        public string GoalTypeName { get; set; }
        public int? Minute { get; set; }
        public int? MinutePlus { get; set; }
    }


    public class RefereeMatchPenaltyGoalsViewModel
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public long? PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public int? Goal { get; set; }
        public string GoalTypeName { get; set; }
        public int? PenaltyOrder { get; set; }
    }

}