namespace PFL.Models.ViewModels
{
    public class AddPlayerToMatchViewModel
    {

        public int MatchId { get; set; }

        public int PlayerId { get; set; }
        
        public int PlayerClubTypeId { get; set; }

        public bool MainStaff { get; set; }

        public bool? Captain { get; set; }


        public bool AddStatus { get; set; }
    }
}