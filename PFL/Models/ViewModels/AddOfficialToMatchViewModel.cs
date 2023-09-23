namespace PFL.Models.ViewModels
{
    public class AddOfficialToMatchViewModel
    {
        public int MatchId { get; set; }
        public int OfficialId { get; set; }
        public int OfficialClubTypeId { get; set; }
        public bool AddStatus { get; set; }

        public int MatchOfficialZoneTypeId { get; set; }
    }
}