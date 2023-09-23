using System.Collections.Generic;

namespace PFL.Models.ViewModels
{
    public class ToursViewModel
    {
        public int TournamentId { get; set; }
        public List<Tour> TourNumbers { get; set; }
        public int CurrentTourNumber { get; set; }
    }

    public class Tour
    {
        public int TourNumber { get; set; }
        public string TourLabel { get; set; }
    }
}