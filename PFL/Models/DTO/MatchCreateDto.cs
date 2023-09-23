using System;


namespace PFL.Models.DTO
{
    public class MatchCreateDto
    {
        public int Id { get; set; }

        public int TournamentTourId { get; set; }

        public int? HomeClubId { get; set; }
        public int? GuestClubId { get; set; }

        public bool HomeClubTechnicalDefeat { get; set; }
        public string HomeClubTechnicalDefeatNote { get; set; }
        public bool GuestClubTechnicalDefeat { get; set; }
        public string GuestClubTechnicalDefeatNote { get; set; }

        public int? RefereeId { get; set; }
        public int? RefereeAssistant1Id { get; set; }
        public int? RefereeAssistant2Id { get; set; }
        public int? FourthRefereeId { get; set; }
        public int? AdditionalReferee1Id { get; set; }
        public int? AdditionalReferee2Id { get; set; }
        public int? RefereeVarId { get; set; }
        public int? RefereeAvarId { get; set; }
        public int? AffaRepresentativeId { get; set; }
        public int? RefereeInspectorId { get; set; }


        public int? StadiumId { get; set; }
        public DateTime? MatchDate { get; set; }
        public DateTime? MatchTime { get; set; }
    }



    //public class MatchEditDto
    //{
    //    public int TournamentTourId { get; set; }

    //    public int Id { get; set; }

    //    public int? HomeClubId { get; set; }

    //    public int? GuestClubId { get; set; }
    //    public int RefereeId { get; set; }
    //    public int? RefereeAssistant1Id { get; set; }
    //    public int? RefereeAssistant2Id { get; set; }
    //    public int? FourthRefereeId { get; set; }
    //    public int? AdditionalReferee1Id { get; set; }
    //    public int? AdditionalReferee2Id { get; set; }
    //    public int? AffaRepresentativeId { get; set; }
    //    public int? RefereeInspectorId { get; set; }




    //    public string Stadium { get; set; }

    //    public DateTime? MatchDate { get; set; }
    //}
}