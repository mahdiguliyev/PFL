using System.Collections.Generic;
using PFL.Entities.EntityModels;


namespace PFL.Models.ViewModels
{
    public class PlayerTransfersViewModel
    {
        public long PlayerId { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public string PlayerFatherName { get; set; }
        public List<Transfer> Transfers { get; set; }
    }
}