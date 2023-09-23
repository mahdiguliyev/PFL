namespace PFL.Models.ViewModels
{
    public class MatchOfficialSelectionViewModel
    {
        public int ClubOfficialId { get; set; }
        public string OfficialFirstName { get; set; }
        public string OfficialLastName { get; set; }
        public string OfficialFatherName { get; set; }
        public string OfficialPositionName { get; set; }
        
        public int ClubTypeId { get; set; }
        public bool Selected { get; set; }
        public int MatchOfficialZoneTypeId { get; set; }

    }
}