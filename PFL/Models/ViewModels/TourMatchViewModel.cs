using System;

namespace PFL.Models.ViewModels
{
    public class TourMatchViewModel
    {
        public int TourNumber { get; set; }
        public int? MatchId { get; set; }
        public DateTime? MatchDate { get; set; }
    }
}