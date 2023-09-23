using System;
using PFL.Models.Enums;

namespace PFL.Models.ViewModels
{
    public class PlayerCardStatisticViewModel
    {
        public int TourNumber { get; set; }
        public int? MatchId { get; set; }
        public DateTime? MatchDate { get; set; }
        public long PlayerId { get; set; }
        public PenaltyCardTypeEnum PenaltyCardType { get; set; }
        public int? YellowMinute { get; set; }
        public int? YellowOffgameTypeId { get; set; }
        public int? Yellow2Minute { get; set; }
        public int? Yellow2OffgameTypeId { get; set; }
        public int? RedMinute { get; set; }
        public int? RedOffgameTypeId { get; set; }
    }
}