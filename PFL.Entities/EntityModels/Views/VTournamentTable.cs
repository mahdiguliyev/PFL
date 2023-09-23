namespace PFL.Entities.EntityModels.Views
{
    //[Table("v_tournament_table")]
    public class VTournamentTable
    {
        //this id is just for key
        //public Guid Id { get; set; }
        public int TournamentId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int? PlayedGame { get; set; }
        public int? WinCount { get; set; }
        public int? ScorelessCount { get; set; }
        public int? DefeatCount { get; set; }

        //vurulan top
        public int? GoalCountTo { get; set; }

        //buraxilan top
        public int? GoalCountFrom { get; set; }

        public int? GoalDifference { get; set; }
        public int? Score { get; set; }

    }
}