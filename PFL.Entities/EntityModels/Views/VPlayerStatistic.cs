namespace PFL.Entities.EntityModels.Views
{
    //[Table("v_player_statistics")]
    public class VPlayerStatistic
    {



    //,[yellow_card_count]
    //,[yellow2_card_count]
    //,[red_card_count]

        //public Guid Id { get; set; }

        public string Season { get; set; }

        public int TournamentId { get; set; }

        public string TournamentName { get; set; }

        public string ClubName { get; set; }

        public int MatchCount { get; set; }

        public int MainStaffMatchCount { get; set; }

        public int ShiftInCount { get; set; }

        public int ShiftOutCount { get; set; }

        public int SecondaryStaffMatchCount { get; set; }

        public int Goals { get; set; }

        public int YellowCardCount { get; set; }

        public int Yellow2CardCount { get; set; }

        public int RedCardCount { get; set; }
    }
}