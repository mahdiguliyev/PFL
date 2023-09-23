namespace PFL.Entities.EntityModels.Views
{
    //[Table("v_tournament_card_statistic")]
    public class VTournamentCardStatistic
    {
        //this id is just for key
        //public Guid Id { get; set; }
        public int TournamentId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public int PlayedGame { get; set; }
        public int YellowCardCount { get; set; }
        public int DoubleYellowCardCount { get; set; }
        public int RedCardCount { get; set; }
    }
}