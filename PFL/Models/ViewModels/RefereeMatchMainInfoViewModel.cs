namespace PFL.Models.ViewModels
{
    public class RefereeMatchMainInfoViewModel
    {
        public int MatchId { get; set; }

        public string HomeClubName { get; set; }
        public int? HomeClubScore { get; set; }

        public string GuestClubName { get; set; }
        public int? GuestClubScore { get; set; }

        public int? RefereeId { get; set; }
        public string RefereeFullName { get; set; }

        public int? RefereeAssistant1Id { get; set; }
        public string RefereeAssistant1FullName { get; set; }

        public int? RefereeAssistant2Id { get; set; }
        public string RefereeAssistant2FullName { get; set; }

        public int? FourthRefereeId { get; set; }
        public string FourthRefereeFullName { get; set; }

        public int? AdditionalReferee1Id { get; set; }
        public string AdditionalReferee1FullName { get; set; }

        public int? AdditionalReferee2Id { get; set; }
        public string AdditionalReferee2FullName { get; set; }

        public string AffaRepresentative { get; set; }

        public string RefereeInspector { get; set; }

        public int HalfExtraTime { get; set; }
        public int FullExtraTime { get; set; }

        public int Half2ExtraTime { get; set; }
        public int Full2ExtraTime { get; set; }

        public string Stadium { get; set; }
        public int? AudienceCount { get; set; }

    }
}