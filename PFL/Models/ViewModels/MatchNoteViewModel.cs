namespace PFL.Models.ViewModels
{
    public class MatchNoteViewModel
    {
        public int MatchId { get; set; }
        public MatchNoteType MatchNoteType { get; set; }
        public string Note { get; set; }
    }

    public enum MatchNoteType
    {
        CupNote = 0,
        RefereeNote = 1,
        HomeClubNote = 2,
        GuestClubNote = 3
    }
}