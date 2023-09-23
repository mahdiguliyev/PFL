using System;

namespace PFL.Models.DTO
{
    public class AdminMatchListDto
    {

        public int Id { get; set; }

        public int? HomeClubId { get; set; }

        public string HomeClubName { get; set; }

        public int? GuestClubId { get; set; }

        public string GuestClubName { get; set; }

        public DateTime? HomeClubConfirmedDate { get; set; }

        public bool? HomeClubConfirm { get; set; }
        public bool HomeClubExpConfirmAllow { get; set; }

        public int? HomeClubScore { get; set; }

        public int? GuestClubScore { get; set; }

        public bool? GuestClubConfirm { get; set; }
        public bool GuestClubExpConfirmAllow { get; set; }

        public DateTime? GuestClubConfirmedDate { get; set; }

        public DateTime? MatchDate { get; set; }

        public bool RefereeConfirm { get; set; }

        public DateTime? RefereeConfirmedDate { get; set; }
    }
}