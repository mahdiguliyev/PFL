using System;

namespace PFL.Models.ViewModels
{
    public class MatchPlayerSelectionViewModel
    {
        public int ClubTypeId { get; set; }
        public long PlayerId { get; set; }
        public int? PlayerNumber { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public DateTime? PlayerBirthDate { get; set; }
        public bool MainStaff { get; set; }
        public bool Selected { get; set; }
        
        public string PositionLabel { get; set; }

        public int CitizenshipId { get; set; }
        public bool Captain { get; set; }
    }
}